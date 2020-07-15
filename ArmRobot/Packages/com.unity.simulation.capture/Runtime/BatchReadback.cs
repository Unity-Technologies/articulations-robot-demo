#if UNITY_2019_3_OR_NEWER
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using Unity.Simulation;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

public class BatchReadback
{
    private BatchReadback() { }

    private static BatchReadback _instance;

    public static BatchReadback Instance()
    {
        if (_instance == null)
        {
            _instance = new BatchReadback();
            _instance._useAsyncReadback = SystemInfo.supportsAsyncGPUReadback;
            Manager.Instance.ShutdownNotification += _instance.FlushRequestsPool;
        }

        return _instance;
    }

    public int                          BatchSize         = 50; 
    private Queue<ReadbackRequest>      _requestsBatch    = new Queue<ReadbackRequest>();
    private Queue<ReadbackRequest>      _requestsPool     = new Queue<ReadbackRequest>();
    private bool                        _useAsyncReadback;


    /// <summary>
    /// Queue a rendertexture for readback. The readback will happen after the number of requests reaches the batchsize.
    /// </summary>
    /// <param name="renderTexture">Render texture for readback</param>
    /// <param name="callback">Callback that needs to be invoked after the readback for provided render texture is complete</param>
    public void QueueReadback(RenderTexture renderTexture, Func<object, AsyncRequest.Result> callback)
    {
        var req = GetReadBackRequestFromPool(renderTexture);
        req.readbackAction = callback;
        Graphics.Blit(renderTexture, req.renderTexture);
        _requestsBatch.Enqueue(req);

        if (_requestsBatch.Count == BatchSize)
        {
            if (_useAsyncReadback)
            {
                ProcessBatchAsync();
            }
            else
            {
                ProcessBatch();
            }
        }
    }

    private ReadbackRequest GetReadBackRequestFromPool(RenderTexture renderTexture)
    {
        ReadbackRequest request;
        if (_requestsPool.Count > 0)
        {
            request = _requestsPool.Dequeue();
        }
        else
        {
            request = new ReadbackRequest()
            {
                renderTexture = new RenderTexture(renderTexture.width, renderTexture.height,  renderTexture.depth, renderTexture.graphicsFormat)
            };
        }
        
        Debug.Assert(request.renderTexture.width == renderTexture.width 
                     && request.renderTexture.height == renderTexture.height 
                     && request.renderTexture.graphicsFormat == renderTexture.graphicsFormat);
        
        return request;
    }

    private void ProcessBatchAsync()
    {
        Debug.Assert(_useAsyncReadback == SystemInfo.supportsAsyncGPUReadback);

        while (_requestsBatch.Count > 0)
        {
            var request = _requestsBatch.Dequeue();
            AsyncGPUReadback.Request(request.renderTexture, 0, (asyncRequest) =>
            {
                if (asyncRequest.hasError)
                {
                    Debug.LogError("Async GPUReadbackRequest failed!");
                }
                else
                {
                    request.readbackAction(asyncRequest.GetData<byte>().ToArray());
                }
                _requestsPool.Enqueue(request);
            });
        }
    }

    private void ProcessBatch()
    {
        
        while(_requestsBatch.Count > 0)
        {
            var request = _requestsBatch.Dequeue();
            var graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(request.renderTexture.format, false);
            var pixelSize = GraphicsFormatUtility.GetBlockSize(graphicsFormat);
            var channels = GraphicsFormatUtility.GetComponentCount(graphicsFormat);
            var channelSize = pixelSize / channels;
            var rect = new Rect(0, 0, request.renderTexture.width, request.renderTexture.height);

            if (channels >= 3 && channels <= 4)
            {
                if (request.texture == null)
                    request.texture = new Texture2D(request.renderTexture.width, request.renderTexture.height, request.renderTexture.graphicsFormat, TextureCreationFlags.None);
                RenderTexture.active = request.renderTexture;
                request.texture.ReadPixels(rect, 0, 0);
                request.InvokeCallback(request.texture.GetRawTextureData());
                RenderTexture.active = null;
            }
            else
            {
                Debug.Assert(channels == 1, "Can only handle a single channel RT.");

                // Read pixels must be one of RGBA32, ARGB32, RGB24, RGBAFloat or RGBAHalf.
                // So R16 and RFloat will be converted to RGBAFloat.
                var texture = new Texture2D(request.renderTexture.width, request.renderTexture.height, TextureFormat.RGBAFloat, false);
                RenderTexture.active = request.renderTexture;
                texture.ReadPixels(rect, 0, 0);
                RenderTexture.active = null;

                var length = request.renderTexture.width * request.renderTexture.height;
                var input  = ArrayUtilities.Cast<float>(texture.GetRawTextureData());
                UnityEngine.Object.Destroy(texture);

                int index = 0;
                switch (channelSize)
                {
                    case 2:
                        short[] shorts = ArrayUtilities.Allocate<short>(length);
                        var si = 0;
                        var numerator = (1<<16)-1;
                        while (index < length)
                        {
                            shorts[index++] = (short)(numerator * input[si]);
                            si += 4;
                        }
                        var shortOutputNativeArray = new NativeArray<byte>(ArrayUtilities.Cast<byte>(shorts), Allocator.Persistent);
                        request.InvokeCallback(ArrayUtilities.Cast<byte>(shorts));
                        break;
                    case 4:
                        float[] floats = ArrayUtilities.Allocate<float>(length);
                        var fi = 0;
                        while (index < length)
                        {
                            floats[index++] = input[fi];
                            fi += 4;
                        }
                        var floatOutputNativeArray = new NativeArray<byte>(ArrayUtilities.Cast<byte>(floats), Allocator.Persistent);
                        request.InvokeCallback(ArrayUtilities.Cast<byte>(floats));
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            
            _requestsPool.Enqueue(request);
        }
    }
    
    private void FlushRequestsPool()
    {
        while(_requestsBatch.Count > 0)
            ProcessBatch();
    }
}

public class ReadbackRequest
{
    /// <summary>
    /// Callback that needs to be invoked after readback request is finished executing.
    /// </summary>
    public Func<object, AsyncRequest.Result> readbackAction;
    
    /// <summary>
    /// Render texture for which the readback is requested
    /// </summary>
    public RenderTexture                     renderTexture;
    
    /// <summary>
    /// Texture to which the readback data is to be copied to.
    /// </summary>
    public Texture2D                         texture;

    /// <summary>
    /// Invoke the readbackAction callback upon completion of the request.
    /// </summary>
    /// <param name="data"></param>
    public void InvokeCallback(byte[] data)
    {
        readbackAction?.Invoke(data);
    }
}
#endif // UNITY_2019_3_OR_NEWER

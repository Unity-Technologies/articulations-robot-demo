using UnityEngine;
using Unity.Simulation;
using UnityEngine.Experimental.Rendering;
using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class VisionDataCollector : MonoBehaviour
{
    public Camera _camera;
    public int maxSamples;

    private Unity.Simulation.Logger dataLogger;
    private string screenCapturePath;

    private int sampleIndex;
    private bool haveQuit;
    private float minCaptureInterval = 0.0f;
    private float lastCaptureTime;

    private int width;
    private int height;


    void Start()
    {
        width = _camera.pixelWidth;
        height = _camera.pixelHeight;
        lastCaptureTime = Time.time;

        Debug.Log(Application.persistentDataPath + "/" + Configuration.Instance.GetAttemptId());
        screenCapturePath = Manager.Instance.GetDirectoryFor(DataCapturePaths.ScreenCapture);
        // Data logger defaults to the same run directory as ScreenCapture
        dataLogger = new Unity.Simulation.Logger("DataCapture");

    }


    // CONTROL

    public bool CaptureIfNecessary(string imageName, System.Object dataPoint)
    {
        bool belowSampleLimit = (sampleIndex < maxSamples);
        bool aboveMinCaptureInterval = (Time.time - lastCaptureTime) >= minCaptureInterval;
        print(aboveMinCaptureInterval);

        // take a sample 
        if (belowSampleLimit && aboveMinCaptureInterval)
        {   
            Capture(imageName, dataPoint);

            lastCaptureTime = Time.time;
            sampleIndex += 1;
            return true;
        }

        // quit
        if (!belowSampleLimit && !haveQuit)
        {
            haveQuit = true;
            StartCoroutine(Quit());
        }

        return false;
    }

    public string NextImageName()
    {
        return "image" + "_" + sampleIndex;
    }


    // HELPERS

    IEnumerator Quit()
    {
        //dataLogger.Flushall();
        yield return new WaitForSeconds(5);
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void Capture(string imageName, System.Object dataPoint)
    {
        // Call Screen Capture
        /*
        var screen = CaptureCamera.Capture(_camera, request =>
        {
            Debug.Log("beginning capture");
            string path = string.Format("{0}/{1}.jpg", screenCapturePath, imageName);
            bool flipY = false;

            // Convert the screen capture to a byte array
            Array image = CaptureImageEncoder.Encode(
                request.data.colorBuffer as Array,
                width, 
                height,
                GraphicsFormat.R8G8B8A8_UNorm,
                CaptureImageEncoder.ImageFormat.Jpg,
                flipY);

            // Write the screen capture to a file
            
            var result = FileProducer.Write(path, image);

            // Wait for Async screen capture request to return and then log data point
            Debug.Log("result" + result);
            if (result)
            {
                Debug.Log("datapoint" + dataPoint);
                // Log data point to file
                dataLogger.Log(dataPoint);

                return AsyncRequest.Result.Completed;
            }
            
            return AsyncRequest.Result.Error;
        });
        */
        bool flag_screen = false;
        while (flag_screen == false){

            var screen = CaptureCamera.Capture(_camera, request =>
            {
                // Convert the screen capture to a byte array
                byte[] image = ImageConversion.EncodeArrayToPNG(
                        (byte[])request.data.colorBuffer,
                        GraphicsFormat.R8G8B8A8_UNorm, 
                        (uint)width,
                        (uint)height);
                    // Write the screen capture to a file
                    string path = string.Format("{0}/{1}.png", screenCapturePath, imageName);
                    bool fileWrite = FileProducer.Write(path, image);
                    Debug.Log("Logging entry for image :  " + imageName);
                    dataLogger.Log(dataPoint);
                    flag_screen = true;
                    return fileWrite ? AsyncRequest.Result.Completed : AsyncRequest.Result.Error;
            }, flipY: _camera.targetTexture == null);
        }
    }


}

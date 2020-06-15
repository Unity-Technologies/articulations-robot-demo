using UnityEngine;
using Unity.Simulation;
using UnityEngine.Experimental.Rendering;
using System;
using System.IO;

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
            Quit();
        }

        return false;
    }

    public string NextImageName()
    {
        return "image" + "_" + sampleIndex;
    }


    // HELPERS

    private void Quit()
    {
        dataLogger.Flushall();
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private void Capture(string imageName, System.Object dataPoint)
    {
        // Call Screen Capture
        var screen = CaptureCamera.Capture(_camera, request =>
        {
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
            if (result)
            {
                // Log data point to file
                dataLogger.Log(dataPoint);

                return AsyncRequest.Result.Completed;
            }

            return AsyncRequest.Result.Error;
        });
    }


}

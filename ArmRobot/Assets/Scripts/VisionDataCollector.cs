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
    public GameObject cube;
    public int fileCounter;

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
    // method to take a capture of the scene and save it inside the ScreenCapture file but also save the data into the Log file
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = _camera.targetTexture;
 
        _camera.Render();
 
        Texture2D Image = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;
 
        var Bytes = Image.EncodeToPNG();
        Destroy(Image);

        string path = string.Format("{0}/{1}.png", screenCapturePath, imageName);
        File.WriteAllBytes(path, Bytes);
        dataLogger.Log(dataPoint);
        fileCounter++;
    }

}

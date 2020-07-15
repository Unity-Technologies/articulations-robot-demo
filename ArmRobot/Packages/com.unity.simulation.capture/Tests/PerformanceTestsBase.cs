#if ENABLE_PERFORMANCE_TESTS
using System;
using System.Collections;
using System.Collections.Generic;

using Unity.PerformanceTesting;
using Unity.Simulation;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

using NUnit.Framework;

using Object = UnityEngine.Object;

[Category("Performance")]
public abstract class  PerformanceTestsBase : ScreenCaptureBase
{
    // Time, in seconds, to allow settling after scene load, object creation, etc, before we start sampling metrics
    protected readonly float SettleTimeSeconds = 2f;

    protected ExistingMonobehaviourTest<T> SetupPerfTest<T>(Action<CameraGrab> action = null) where T : MonoBehaviour, IMonoBehaviourTest
    {
        var cameraGrabComponent = Object.FindObjectOfType<CameraGrab>();
        var mainCamera = Camera.main;
        Debug.Assert(mainCamera != null, "Main Camera is not set");

        if (cameraGrabComponent != null)
        {
            action?.Invoke(cameraGrabComponent);
        }
        else
        {
            var cameraGrab = Object.FindObjectOfType<Camera>().gameObject.AddComponent<CameraGrab>();
            cameraGrab._cameraSources = new[] {mainCamera};
            action?.Invoke(cameraGrab);
        }
        
        mainCamera.gameObject.AddComponent(typeof(T));
        var testComponent = Object.FindObjectOfType<T>();
        
        return new ExistingMonobehaviourTest<T>(testComponent);
    }

    protected void SetActiveScene(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
    }
}
#endif
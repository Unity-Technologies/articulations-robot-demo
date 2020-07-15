#if ENABLE_PERFORMANCE_TESTS
using System.Collections;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Unity.Simulation.Capture.Tests
{
    public class ScreenCapturePerformanceTests : PerformanceTestsBase
    {
        private string[] _samplerNames = {"Camera.Render", "Render.Mesh"};
        private const string _scene = "Roll-a-ball";
        
        [UnityTest, Performance]
        public IEnumerator CaptureImage_Perf_BatchReadback_Async()
        {
            SetupTest(1000, 1, DepthTextureMode.Depth, true);

            yield return new WaitForSeconds(SettleTimeSeconds);
            
            var perfTest = SetupPerfTest<CapturePerformanceBase>((cameraGab) =>
            {
                CaptureOptions.useAsyncReadbackIfSupported = true;
                var cameraGrab = Object.FindObjectOfType<CameraGrab>();
                if (cameraGrab != null)
                {
                    cameraGrab._batchReadback = true;
                    cameraGrab._batchSize = 100;
                    cameraGrab._screenCaptureInterval = 0;
                }  
            });
            
            perfTest.component.CaptureMetrics = true;
            yield return perfTest;
        }
        
        [UnityTest, Performance]
        public IEnumerator CaptureImage_Perf_NonBatchReadback_Async()
        {
            SetupTest(1000, 1, DepthTextureMode.Depth, true);

            yield return new WaitForSeconds(SettleTimeSeconds);
            
            var perfTest = SetupPerfTest<CapturePerformanceBase>((cameraGab) =>
            {
                CaptureOptions.useAsyncReadbackIfSupported = true;
                var cameraGrab = Object.FindObjectOfType<CameraGrab>();
                if (cameraGrab != null)
                {
                    cameraGrab._batchReadback = false;
                    cameraGrab._screenCaptureInterval = 0;
                }  
            });
            
            perfTest.component.CaptureMetrics = true;
            yield return perfTest;
        }
        
        [Performance, UnityTest]
        public IEnumerator CaptureImage_Perf_BatchReadback_Slow()
        {
            SetupTest(1000, 1, DepthTextureMode.Depth, true);
            yield return new WaitForSeconds(SettleTimeSeconds);
            
            var perfTest = SetupPerfTest<CapturePerformanceBase>((cameraGrab) =>
            {
                CaptureOptions.useAsyncReadbackIfSupported = false;
                if (cameraGrab != null)
                {
                    cameraGrab._batchReadback = true;
                    cameraGrab._batchSize = 100;
                    cameraGrab._screenCaptureInterval = 0;
                }    
            });
            perfTest.component.CaptureMetrics = true;
            yield return perfTest;
        }
        
        [UnityTest, Performance]
        public IEnumerator CaptureImage_Perf_NonBatchReadback_Slow()
        {
            SetupTest(1000, 1, DepthTextureMode.Depth, true);
            yield return new WaitForSeconds(SettleTimeSeconds);
            
            var perfTest = SetupPerfTest<CapturePerformanceBase>((cameraGrab) =>
            {
                CaptureOptions.useAsyncReadbackIfSupported = false;
                if (cameraGrab != null)
                {
                    cameraGrab._batchReadback = false;
                    cameraGrab._screenCaptureInterval = 0;
                }    
            });
            perfTest.component.CaptureMetrics = true;
            yield return perfTest;
        }
    }
}
#endif
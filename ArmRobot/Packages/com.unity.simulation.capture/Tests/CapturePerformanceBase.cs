#if ENABLE_PERFORMANCE_TESTS
using System;
using System.Diagnostics;
using System.Linq;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

public class CapturePerformanceBase : MonoBehaviour, IMonoBehaviourTest
{
    // Number of frames we capture and calculate metrics from
    public  int                NumCaptureFrames = 1000;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private int                _startFrameCount = 10;
    private int                _endFrameCount;
    private long               _totalElapsedViaStopWatch = 0L;
    private int                _startRenderFrameCount;

    private SampleGroup FpsViaSW = new SampleGroup("FPS vis SW", SampleUnit.Undefined, true);
    private SampleGroup ElapsedTime = new SampleGroup("Simulation Time", SampleUnit.Second, false);
    private SampleGroup FpsSg = new SampleGroup("FPS", SampleUnit.Undefined, true);
    private string[]    _samplerNames = {"Camera.Render", "Render.Mesh"};
    private int         _frameCount = 0;
    public bool         CaptureMetrics { get; set; }
    private float       _fps { get; set; }
    private bool        _isMetricsCaptured => _frameCount > (NumCaptureFrames + _startFrameCount);

    public bool IsTestFinished
    {
        get
        {
            bool isTestFinished = false;
            if (_isMetricsCaptured)
            {
                EndMetricCapture();
                Debug.Log("Finishing the test");
                isTestFinished = true;
            }

            return isTestFinished;
        }
    }

    private void Update()
    {
        if (CaptureMetrics)
        {
            if (_frameCount == _startFrameCount)
            {
                _stopwatch.Start();
            }
            _frameCount++;
            SampleFps();
        }
    }

    public void EndMetricCapture()
    {
        _stopwatch.Stop();
        _totalElapsedViaStopWatch = _stopwatch.ElapsedMilliseconds;
        Measure.Custom(ElapsedTime, _totalElapsedViaStopWatch/1000.0f);
        Measure.Custom(FpsViaSW, (NumCaptureFrames * 1000.0) / (1.0 * _totalElapsedViaStopWatch));
        CaptureMetrics = false;
    }

    private void SampleFps()
    {
        _fps = GetFps();
        Measure.Custom(FpsSg, _fps);
        _startRenderFrameCount = Time.renderedFrameCount;
    }

    private float GetFps()
    {
        return (Time.renderedFrameCount - _startRenderFrameCount) / Time.unscaledDeltaTime;
    }

    private void Start()
    {
        Measure.ProfilerMarkers(_samplerNames);
    }
}
#endif
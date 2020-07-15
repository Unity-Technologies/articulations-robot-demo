using UnityEngine;

namespace Unity.Simulation
{
    internal static class CaptureVersion
    {
        internal const string kVersionString = "v0.0.10-preview.8";

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            Log.I($"Started com.unity.simulation.capture {kVersionString}");
        }
    }
}

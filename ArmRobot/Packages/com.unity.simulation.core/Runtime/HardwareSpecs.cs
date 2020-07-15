using System;
using System.Collections.Generic;

using UnityEngine;

// UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
// UNITY_STANDALONE_LINUX || UNITY_STANDALONE_LINUX

namespace Unity.Simulation
{
    static class HardwareSpecs
    {
        // Non Public Members

        [RuntimeInitializeOnLoadMethod]
        static void LogHardwareSpecs()
        {
            ReportRelevantFeatures();
        }

        static void Report(Dictionary<string, string> features, string name, string key = null)
        {
            if (key == null)
                key = name;
            if (features.ContainsKey(key))
                Log.I($"{name} : {features[key]}");
        }

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        static void ReportRelevantFeatures()
        {
            var list = ExecUtility.Execute("", "sysctl",  "-a").Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var features = new Dictionary<string, string>(list.Length);
            foreach (var i in list)
            {
                var kv = i.Split(':');
                features.Add(kv[0].Trim(), kv[1].Trim());
            }

            Report(features, "CPU Brand",       "machdep.cpu.brand_string");
            Report(features, "CPU Features",    "machdep.cpu.features");
            Report(features, "Physical CPUs",   "hw.physicalcpu");
            Report(features, "Logical CPUs",    "hw.logicalcpu");
            Report(features, "Cache Line Size", "hw.cachelinesize");
        }

#elif UNITY_STANDALONE_LINUX
        static void ReportRelevantFeatures()
        {
            var list = ExecUtility.Execute("", "lscpu").Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var features = new Dictionary<string, string>(list.Length);
            foreach (var i in list)
            {
                var kv = i.Split(':');
                features.Add(kv[0].Trim(), kv[1].Trim());
            }

            Report(features, "CPU(s)");
            Report(features, "Thread(s) per core");
            Report(features, "Vendor ID");
            Report(features, "CPU family");
            Report(features, "Model");
            Report(features, "Model name");
            Report(features, "Flags");
        }
#else
        static void ReportRelevantFeatures()
        {}
#endif
    }
}


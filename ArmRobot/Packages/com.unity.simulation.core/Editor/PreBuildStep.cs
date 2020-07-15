#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Unity.Simulation
{
    class PreBuildStep : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            var objects = Object.FindObjectsOfType<TimeLimit>();
            if (objects == null || objects.Length == 0)
            {
                Log.W("Application has no TimeLimit component. If you are building for running on USim, it is recommended that you add a TimeLimit component.");
            }
        }
    }
}
#endif
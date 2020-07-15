using System.Diagnostics;

using UnityEngine;

namespace Unity.Simulation
{
    public class FrameLimit : MonoBehaviour
    {
        public int FrameLimitCount;
        int _frameCounter;

        void Start()
        {
            _frameCounter = 0;
        }

        void Update()
        {
            if (_frameCounter >= FrameLimitCount)
            {
                Log.V($"Frame limit reached {FrameLimitCount} frames.");
                Application.Quit();
            }
        }

        void OnValidate()
        {
            if (FrameLimitCount == 0)
            {
                Log.W($"FrameLimit must be set to a valid number of frames, where 0 < limit");
            }
        }
    }
}
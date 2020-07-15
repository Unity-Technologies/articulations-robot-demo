using System.Diagnostics;

using UnityEngine;

namespace Unity.Simulation
{
    public class TimeLimit : MonoBehaviour
    {
        public const float kMaximumTimeLimitInSeconds = 14400;

        public float TimeLimitInSeconds;

        Stopwatch _timer;

        void Start()
        {
            _timer = new Stopwatch();
            _timer.Start();
        }

        void Update()
        {
            var seconds = _timer.Elapsed.TotalSeconds;
            if (seconds >= TimeLimitInSeconds)
            {
                Log.V($"Time limit reached {TimeLimitInSeconds} seconds.");
                Application.Quit();
            }
        }

        void OnValidate()
        {
            if (TimeLimitInSeconds == 0 || TimeLimitInSeconds > kMaximumTimeLimitInSeconds)
            {
                Log.W($"TimeLimit must be set to a valid number of seconds, where 0 < limit < {kMaximumTimeLimitInSeconds}.");
            }
        }
    }
}
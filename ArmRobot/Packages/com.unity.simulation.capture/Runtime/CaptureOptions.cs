namespace Unity.Simulation
{
    /// <summary>
    /// Class with some options that can be changed at runtime.
    /// </summary>
    public static class CaptureOptions
    {
        /// <summary>
        /// When false, will force capturing to not use async readback even if supported by the platform.
        /// </summary>
        public static bool useAsyncReadbackIfSupported = true;

#if UNITY_2019_3_OR_NEWER
        public static bool useBatchReadback = false;
#endif
    }
}

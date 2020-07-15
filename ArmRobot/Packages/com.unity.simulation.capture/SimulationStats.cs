//using Unity.Simulation;
//using UnityEngine;
//using UnityEngine.Profiling;
//
//public class Stats : MonoBehaviour
//{
//    private Unity.Simulation.Log fixedUpdateMetricsLogger;
//    private Unity.Simulation.Logger snapshotMetricsLogger;
//    private int fixedUpdateCount = 0;
//    private int updateCount = 0;
//
//    private float quitAfterSeconds = 23;
//    private int snapshotCadanceUpdates = 10;
//    
//    private bool isRunning;
//    
//    Recorder playerLoop;
//    Recorder cameraRender;
//    Recorder physicsFixedUpdate;
//    
//    // Start is called before the first frame update
//    void Start()
//    {
//        
//        // Print the location where data will be written
//        Debug.LogFormat("DataCapture Path: {0}/{1}",
//            Application.persistentDataPath, Configuration.Instance.GetAttemptId());
//        
//        // Soft Editor quit
//        isRunning = true;
//
//        // Create new data logger with output files named DataCapture
//        snapshotMetricsLogger = new Unity.AI.Simulation.Logger("SnapshotMetrics");
//        fixedUpdateMetricsLogger = new Unity.AI.Simulation.Logger("FixedUpdateMetricsLogger");
//        
//        playerLoop = Recorder.Get("PlayerLoop");
//        cameraRender = Recorder.Get("Camera.Render");
//        physicsFixedUpdate = Recorder.Get("FixedUpdate.PhysicsFixedUpdate");
//        
//        playerLoop.enabled = true;
//        cameraRender.enabled = true;
//        physicsFixedUpdate.enabled = true;
//    }
//
//    void Update()
//    {
//        updateCount++;
//
//        if (updateCount % snapshotCadanceUpdates == 0 && isRunning)
//        {
//            SnapshotMetrics snapshotMetrics = new SnapshotMetrics(
//                DXManager.Instance.WallElapsedTime,
//                DXManager.Instance.SimulationElapsedTime,
//                DXManager.Instance.SimulationElapsedTimeUnscaled,
//                fixedUpdateCount,
//                updateCount,
//                Time.timeScale);
//            snapshotMetricsLogger.Log(snapshotMetrics);
//            
//        }
//
//        if (Manager.Instance.SimulationElapsedTime > quitAfterSeconds && isRunning)
//        {
//            fixedUpdateMetricsLogger.Flushall();
//            snapshotMetricsLogger.Flushall();
//            Debug.Log("Quitting");
//            isRunning = false;
//            Application.Quit();
//        }
//    }
//    
//    void FixedUpdate()
//    {
//        fixedUpdateCount++;
//
//        if (fixedUpdateCount % snapshotCadanceUpdates == 0 && isRunning)
//        {
//            // Write Metrics
//            float allocatedMemory = Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
//            float reservedMemory = Profiler.GetTotalReservedMemoryLong() / 1048576f;
//            float unusedReservedMemory = Profiler.GetTotalUnusedReservedMemoryLong() / 1048576f;
//            float memoryForGraphics = Profiler.GetAllocatedMemoryForGraphicsDriver() / 1048576f;
//
//            FixedUpdateMetrics fixedUpdateMetrics = new FixedUpdateMetrics(
//                playerLoop.elapsedNanoseconds,
//                cameraRender.elapsedNanoseconds,
//                physicsFixedUpdate.elapsedNanoseconds,
//                allocatedMemory, 
//                reservedMemory, 
//                unusedReservedMemory, 
//                memoryForGraphics);
//            Log.V(JsonUtility.ToJson(fixedUpdateMetrics));
//        }
//    }
//}
//
//// Metrics that are collected in FixedUpdate()
//[System.Serializable]
//public class FixedUpdateMetrics : System.Object
//{
//    public long playerLoop;
//    public long cameraRender;
//    public long physicsFixedUpdate;
//    public float totalAllocatedMemory;
//    public float totalReservedMemory;
//    public float totalUnusedReservedMemory;
//    public float allocatedMemoryForGraphicsDriver;
//    
//    public FixedUpdateMetrics(
//        long playerLoop,
//        long cameraRender,
//        long physicsFixedUpdate,
//        float totalAllocatedMemory, 
//        float totalReservedMemory, 
//        float totalUnusedReservedMemory, 
//        float allocatedMemoryForGraphicsDriver
//        )
//    {
//        this.playerLoop = playerLoop;
//        this.cameraRender = cameraRender;
//        this.physicsFixedUpdate = physicsFixedUpdate;
//        this.totalAllocatedMemory = totalAllocatedMemory;
//        this.totalReservedMemory = totalReservedMemory;
//        this.totalUnusedReservedMemory = totalUnusedReservedMemory;
//        this.allocatedMemoryForGraphicsDriver = allocatedMemoryForGraphicsDriver;
//
//    }
//}
//
//// Metrics that are collected at some interval like how the SDK currently collects Wall Time vs. Sim Time
//[System.Serializable]
//public class SnapshotMetrics : System.Object
//{
//    public double wallTime;
//    public double simTime;
//    public double simTimeUnscaled;
//    public int updateCount;
//    public int fixedUpdateCount;
//    public float timeScale;
//    
//    public SnapshotMetrics(
//        double wallTime, 
//        double simTime, 
//        double simTimeUnscaled, 
//        int fixedUpdateCount, 
//        int updateCount, 
//        float timeScale
//        )
//    {
//        this.wallTime = wallTime;
//        this.simTime = simTime;
//        this.simTimeUnscaled = simTimeUnscaled;
//        this.fixedUpdateCount = fixedUpdateCount;
//        this.updateCount = updateCount;
//        this.timeScale = timeScale;
//
//    }
//}
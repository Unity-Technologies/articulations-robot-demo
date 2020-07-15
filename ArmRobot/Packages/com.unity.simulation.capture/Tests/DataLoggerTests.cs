using System;
using System.Collections;
using System.IO;
using UnityEngine;

using Unity.Simulation;

using NUnit.Framework;
using UnityEngine.TestTools;

using Logger = Unity.Simulation.Logger;

public class DataLoggerTests
{
    struct TestLog
    {
        public string msg;
    }

    [UnityTest]
    [Timeout(10000)]
    public IEnumerator ProducerBuffer_TrimsEmptySpaces_IfPresentBeforeFlush()
    {
        string path = Path.Combine(Configuration.Instance.GetStoragePath(), "Logs", "log_0.txt");
        var inputLog = new TestLog() {msg = "Test"};
        var logger = new Logger("log.txt", 20);
        logger.Log(new TestLog() { msg = "Test"});
        logger.Log(new TestLog() { msg = "UnityTest"});
        while (!System.IO.File.Exists(path))
            yield return null;
        var fileInfo = new FileInfo(path);
        Assert.AreEqual(JsonUtility.ToJson(inputLog).Length + 1, fileInfo.Length);
    }

    [UnityTest]
    public IEnumerator DataLogger_FlushesToFileSystem_WithElapsedTimeSet()
    {
        var logger = new Logger("TestLog.txt", maxElapsedSeconds: 5);
        logger.Log(new TestLog() { msg = "Test 123"});
        yield return new WaitForSeconds(6);
        var path = Path.Combine(Configuration.Instance.GetStoragePath(), "Logs", "TestLog_0.txt");
        Assert.IsTrue(!File.Exists(path));
    }

    [UnityTest]
    public IEnumerator DataLogger_FlushesToFileSystemOnlyWithMaxBufferSize()
    {
        var logger = new Logger("SimLog", bufferSize: 65536, maxElapsedSeconds:-1);
        logger.Log(new TestLog() { msg = "Test Simulation Log!"});
        yield return new WaitForSeconds(5);
        var path = Path.Combine(Configuration.Instance.GetStoragePath(), "Logs", "SimLog_0.txt");
        Assert.IsTrue(!File.Exists(path));
        for (int i = 0; i < 100; i++)
        {
            logger.Log(new TestLog(){ msg = "Test Simulation Log"});
        }
        logger.Flushall(true);
        Assert.IsTrue(File.Exists(path));

        var fileContents = File.ReadAllLines(path);
        Assert.AreEqual(fileContents.Length, 101);
    }
}
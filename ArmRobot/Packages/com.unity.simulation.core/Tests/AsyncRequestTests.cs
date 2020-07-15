using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

using Unity.Simulation;
using UnityEngine.Assertions;
#if ENABLE_CLOUDTESTS
using Unity.Simulation.Tools;
#endif
using UnityEngine.TestTools;

public class AsyncRequestTests
{
    [UnityTest]
    public IEnumerator AsyncRequest_AllocatesAndReturnsToPool()
    {
        using (var req = Manager.Instance.CreateRequest<AsyncRequest<object>>())
        {
            req.Enqueue( (AsyncRequest<object> r) =>
            {
                return AsyncRequest.Result.Completed;
            });
            req.Execute();

            while (!req.completed)
                yield return null;
        }
        
        Assert.IsTrue(Manager.Instance.requestPoolCount == 1,  "requestPoolCount == 1");
        using (var req = Manager.Instance.CreateRequest<AsyncRequest<object>>())
        {
            Assert.IsTrue(Manager.Instance.requestPoolCount == 0, "requestPoolCount == 0");

            req.Enqueue( (AsyncRequest<object> r) =>
            {
                return AsyncRequest.Result.Completed;
            });
            req.Execute();

            while (!req.completed)
                yield return null;
        }

        Assert.IsTrue(Manager.Instance.requestPoolCount == 1, "requestPoolCount == 1");
    }

#if ENABLE_CLOUDTESTS
    [CloudTest]
#endif
    [UnityTest]
    public IEnumerator AsyncRequest_StartingRequestNTimesProducesNResults()
    {        
        using (var req = Manager.Instance.CreateRequest<AsyncRequest<object>>())
        {
            var N = UnityEngine.Random.Range(10, 1000);

            for (int i = 0; i < N; ++i)
            {
                req.Enqueue( (AsyncRequest<object> r) =>
                {
                    return AsyncRequest.Result.Completed;
                });
            }
            req.Execute();

            while (!req.completed)
                yield return null;

            Debug.Assert(req.results.Length == N);
        }
    }

#if ENABLE_CLOUDTESTS
    [CloudTest]
#endif
    [UnityTest]
    public IEnumerator AsyncRequest_JobSchedule_HappensOnMainThread()
    {
        var jobCompleted = false;

        var thread = new Thread(new ThreadStart(() =>
        {
            using (var req = Manager.Instance.CreateRequest<AsyncRequest<object>>())
            {
                req.Enqueue( r =>
                {
                    jobCompleted = true;
                    return AsyncRequest.Result.Completed;
                });
                req.Execute();
            }
        }));

        thread.Start();

        while (!jobCompleted)
            yield return null;

        thread.Join();
    }
}


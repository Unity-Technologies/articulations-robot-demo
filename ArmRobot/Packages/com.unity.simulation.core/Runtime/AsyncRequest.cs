using System;
using System.Collections.Concurrent;
using System.Threading;

using UnityEngine;
using UnityEngine.Profiling;

using Unity.Jobs;
using Unity.Collections;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Simulation
{
    /// <summary>
    /// Base class for representing an asynchronous request.
    /// AsyncRequest is a bit like Task, but doesn't always execute
    /// on the main thread like Task in Unity.
    /// Another difference, is that several different lambda functors
    /// can operate on different parts of the data in parallel.
    /// Once all lambda functors have completed, the request is marked as complete.
    /// </summary>
    public abstract class AsyncRequest
    {
        // Public Members

        /// <summary>
        /// An enum describing the result status.
        /// </summary>
        [Flags]
        public enum Result
        {
            /// <summary>
            /// No flag set.
            /// </summary>
            None = 0,

            /// <summary>
            /// Flag set when the lambda functor has completed.
            /// </summary>
            Completed = (1<<0),

            /// <summary>
            /// Flag set when the lambda functor returns an error.
            /// Note that Error implies completed.
            /// </summary>
            Error = (1<<1) | Completed
        }

        /// <summary>
        /// An enum describing execution context for lambda functors that need to be invoked.
        /// </summary>
        public enum ExecutionContext
        {
            /// <summary>
            /// Execution context is unspecified.
            /// </summary>
            None,

            /// <summary>
            /// Invoke the callback on the C# thread pool.
            /// </summary>
            ThreadPool,

            /// <summary>
            /// Invoke the callback at the end of the frame.
            /// </summary>
            EndOfFrame,

            /// <summary>
            /// Invoke the callback immediately.
            /// </summary>
            Immediate,

            /// <summary>
            /// Invoke the callback using the Unity Job System.
            /// </summary>
            JobSystem,
        }

        /// <summary>
        /// Property specifying the default ExecutionContext to use when executing requests.
        /// </summary>
        public static ExecutionContext defaultExecutionContext { get; set; } = ExecutionContext.ThreadPool;

        /// <summary>
        /// When using the Unity Job System, jobs will chain after previous jobs in such
        /// a way as to only utilize maxJobSystemParallelism worker threads at any time.
        /// i.e. Setting this to 1 will result in jobs chaining back to back with no parallelism.
        /// </summary>
        public static int maxJobSystemParallelism { get; set; } = JobsUtility.JobWorkerMaximumCount;

        // Non Public Members

        /// <summary>
        /// Flags for the _state field which holds whether or not the request has started and or has an error.
        /// </summary>
        [Flags]
        protected enum State
        {
            /// <summary>
            /// No flag set.
            /// </summary>
            None = 0,

            /// <summary>
            /// Set when the request has started.
            /// </summary>
            Started = (1<<0),

            /// <summary>
            /// Set when the request has encountered an error.
            /// </summary>
            Error = (1<<1)
        }

        /// <summary>
        /// Virtual method implemented by derived classes.
        /// Called to determine if any result had an error.
        /// </summary>
        protected virtual bool AnyResultHasError()
        {
            throw new NotSupportedException("AsyncRequest.AnyResultHasError must be overridden.");
        }

        /// <summary>
        /// Virtual method implemented by derived classes.
        /// Called to determine if all results are present and completed.
        /// </summary>
        protected virtual bool AllResultsAreCompleted()
        {
            throw new NotSupportedException("AsyncRequest.AllResultsAreCompleted must be overridden.");
        }

        /// <summary>
        /// Virtual method implemented by derived classes.
        /// Resets the request. Called when returning to the object pool.
        /// </summary>
        public virtual void Reset()
        {
            throw new NotSupportedException("AsyncRequest.Reset must be overridden.");
        }

        /// <summary>
        /// Returns true if the request has started.
        /// </summary>
        protected bool started
        {
            set { _state = value ? _state | State.Started : _state & ~State.Started; }
            get { return (_state & State.Started) == State.Started; }
        }

        /// <summary>
        /// Returns true if the request has encountered an error.
        /// </summary>
        public bool error
        {
            set { if (value) _state |= State.Error; }
            get { return (_state & State.Error) == State.Error || AnyResultHasError(); }
        }

        /// <summary>
        /// Returns true if the request has completed.
        /// </summary>
        public bool completed
        {
            get { return started && AllResultsAreCompleted(); }
        }

        /// <summary>
        /// Property which holds whether or not the request has started and or has an error.
        /// </summary>
        protected State _state;
    }

    /// <summary>
    /// Concrete AsyncRequest for specific type T.
    /// </summary>
    public class AsyncRequest<T> : AsyncRequest, IDisposable
    {
        /// <summary>
        /// Array of asynchronous results.
        /// </summary>
        public Result[] results { get { return _results.ToArray(); } }

        /// <summary>
        /// Operator overload for adding functors to the AsyncRequest queue.
        /// </summary>
        /// <param name="a">Current AsyncRequest</param>
        /// <param name="b">Functor to be added</param>
        /// <returns>AsyncRequest with updated functors queue.</returns>
        public static AsyncRequest<T> operator +(AsyncRequest<T> a, Func<AsyncRequest<T>, Result> b)
        {
            a.Enqueue(b);
            return a;
        }

        /// <summary>
        /// Returns a reference to the payload for this request.
        /// </summary>
        public ref T data { get { return ref _data; } }

        /// <summary>
        /// Default constructor for AsyncRequest.
        /// </summary>
        public AsyncRequest()
        {
            Reset();
        }

        /// <summary>
        /// Queues a unit of work that can be executed on start.
        /// </summary>
        /// <param name="functor">A callback that needs to be invoked</param>
        public void Enqueue(Func<AsyncRequest<T>, Result> functor)
        {
            if (functor != null)
                _functors.Enqueue(functor);
        }

        /// <summary>
        /// Queues a callback that needs to be executed in the given execution context, and starts the request.
        /// </summary>
        /// <param name="functor">A callback that needs to be invoked</param>
        /// <param name="executionContext">Execution context in which the functor needs to be invoked. viz. Threadpool, EnoOfFrame or Immediate, etc.</param>
        [Obsolete("Start(Func<...>, ExecutionContext) is deprecated. Use Enqueue(Func<...>) + Execute(ExecutionContext) instead.")]
        public void Start(Func<AsyncRequest<T>, Result> functor = null, ExecutionContext executionContext = ExecutionContext.None)
        {
            if (functor != null)
                _functors.Enqueue(functor);
            Execute(executionContext);
        }

        /// <summary>
        /// Starts executing all the queued callback functions in the given execution context.
        /// </summary>
        /// <param name="executionContext">Execution context in which the functions needs to be invoked.</param>
        public void Execute(ExecutionContext executionContext = ExecutionContext.None)
        {
            if (!_functors.IsEmpty)
            {
                if (executionContext == ExecutionContext.None)
                    executionContext = ExecutionContext.EndOfFrame;

                switch (executionContext)
                {
                    case ExecutionContext.EndOfFrame:
                        Dispatch((Func<AsyncRequest<T>, Result> f) => { Manager.Instance.QueueEndOfFrameItem(InvokeWaitCallback, f); });
                        break;

                    case ExecutionContext.Immediate:
                        Dispatch((Func<AsyncRequest<T>, Result> f) => { InvokeWaitCallback(f); });
                        break;

                    case ExecutionContext.JobSystem:
                        if (ThreadUtility.IsMainThread())
                            DispatchWithJobSystem();
                        else
                            Manager.Instance.QueueForMainThread(DispatchWithJobSystem);
                        break;

                    case ExecutionContext.ThreadPool:
                        Dispatch((Func<AsyncRequest<T>, Result> f) =>
                        {
                            // Lazily create this, since it will be the unlikely path once we start using the job system.
                            if (_waitCallback == null)
                                _waitCallback = new WaitCallback(InvokeWaitCallback);
                            ThreadPool.QueueUserWorkItem(_waitCallback, f);
                        });
                        break;

                    default:
                        throw new InvalidOperationException($"Unsupported ExecutionContext {executionContext}");
                }
            }

            this.started = true;
        }

        /// <summary>
        /// Resets the request. Called when adding back to the object pool.
        /// </summary>
        public override void Reset()
        {
            _state        = State.None;
            _disposed     = false;
            _waitCallback = null;
            _functors     = new ConcurrentQueue<Func<AsyncRequest<T>, Result>>();
            _results      = new ConcurrentBag<Result>();
            _data         = default(T);
        }

        /// <summary>
        /// Disposes the request. This will add the request back to the object pool.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            Manager.Instance.RecycleRequest(this);
            GC.ReRegisterForFinalize(this);
        }

        /// <summary>
        /// You can use this method to complete a request without needing a lambda function.
        /// Passing null will likely cause work to be skipped, but passing this will do it and complete.
        /// </summary>
        public static Result DontCare(AsyncRequest<T> r)
        {
            return Result.Completed;
        }

        // Non Public Members.

        /// <summary>
        /// Returns true if any of the results had an error.
        /// </summary>
        protected override bool AnyResultHasError()
        {
            if (_results == null)
                return false;
            foreach (var r in _results)
                if ((r & AsyncRequest.Result.Error) == AsyncRequest.Result.Error)
                    return true;
            return false;
        }

        /// <summary>
        /// All results are completed if...
        /// 1. The request was started.
        /// 2. There are no callbacks in flight.
        /// 3. All results are marked completed.
        /// </summary>
        protected override bool AllResultsAreCompleted()
        {
            if (!this.started)
                return false;
            if (Interlocked.Add(ref _requestsInFlight, 0) != 0)
                return false;
            Debug.Assert(_functors.IsEmpty);
            foreach (var r in _results)
                if ((r & AsyncRequest.Result.Completed) != AsyncRequest.Result.Completed)
                    return false;
            return true;
        }

        ~AsyncRequest()
        {
            Dispose();
        }

        void InvokeWaitCallback(object functor)
        {
            Debug.Assert(functor != null, "Functor cannot be null");
            Func<AsyncRequest<T>, Result> f = functor as Func<AsyncRequest<T>, Result>;
            Result result = Result.None;
            result = f(this);
            Debug.Assert(result != Result.None);
            _results.Add(result);
            Interlocked.Decrement(ref _requestsInFlight);
        }

        void Dispatch(Action<Func<AsyncRequest<T>, Result>> action)
        {
            while (!_functors.IsEmpty)
            {
                Func<AsyncRequest<T>, Result> f;
                if (_functors.TryDequeue(out f))
                {
                    Interlocked.Increment(ref _requestsInFlight);
                    action(f);
                }
            }
        }

        void DispatchWithJobSystem()
        {
            Func<AsyncRequest<T>, Result>[] functors = new Func<AsyncRequest<T>, Result>[_functors.Count];
            for (var i = 0; i < functors.Length && _functors.TryDequeue(out functors[i]); ++i)
                ;

            Debug.Assert(_functors.Count == 0, "_functors.Count changed while dispatching lambda functors.");

            lock (_mutex)
            {
                // Lazily handle maxJobSystemParallelism being changed while jobs are running.
                if (maxJobSystemParallelism != _previousJobHandles.Length)
                {
                    var jobHandles = new JobHandle[maxJobSystemParallelism];
                    if (_previousJobHandles != null)
                        Array.Copy(_previousJobHandles, jobHandles, Math.Min(jobHandles.Length, _previousJobHandles.Length));
                    _previousJobHandles = jobHandles;
                    _previousJobHandleIndex %= maxJobSystemParallelism;
                }

                foreach (var f in functors)
                {
                    var previousJobHandle = _previousJobHandles[_previousJobHandleIndex];

                    previousJobHandle = new LambdaJob(() =>
                    {
                        _profilerSampler.Begin();
                        InvokeWaitCallback(f);
                        _profilerSampler.End();
                    })
                    .Schedule(previousJobHandle);

                    _previousJobHandles[_previousJobHandleIndex] = previousJobHandle;
                    _previousJobHandleIndex = ++_previousJobHandleIndex % maxJobSystemParallelism;
                }
            }

            Interlocked.Add(ref _requestsInFlight, functors.Length);
        }

        // Member Variables

        // Custom sampler for profiling the lambda functor execution time.
        CustomSampler _profilerSampler = CustomSampler.Create("AsyncRequest.Job");

        // Mutex for protecting _previousJobHandles.
        object _mutex = new Mutex();

        // Jobs are chained one after the other to avoid saturating the worker pool.
        // To allow for parallelism, we maintain a pool of previous handles that are chained to round robin style.
        static JobHandle[] _previousJobHandles = new JobHandle[maxJobSystemParallelism];

        // Round-robin index of next job handle to pick.
        static int _previousJobHandleIndex = 0;

        // C# thread pool callback queue.
        WaitCallback _waitCallback;

        // Lockless container for storing results of lambda functor invocations.
        ConcurrentBag<Result> _results;

        // Lockless queue for lambda functors needing to be executed.
        ConcurrentQueue<Func<AsyncRequest<T>, Result>> _functors;

        // The number of requests in flight.
        // Decremented as each lambda functor completes.
        // When this reaches 0, the request is marked complete.
        int _requestsInFlight;

        // Sentinel for ensuring dispose is only invoked once.
        bool _disposed = false;

        // Payload data for this request.
        T _data;
    }
}

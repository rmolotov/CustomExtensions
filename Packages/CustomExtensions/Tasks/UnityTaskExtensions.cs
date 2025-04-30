using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomExtensions.Tasks
{
    public class UnityTaskExtensions
    {
        public static TaskScheduler UnityTaskScheduler { get; }
        public static int UnityThreadId { get; }
        public static SynchronizationContext UnitySynchronizationContext { get; }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (Thread.CurrentThread.ManagedThreadId == UnityThreadId)
            {
                Debug.Log("Unity thread context initialized");
            }
        }

        static UnityTaskExtensions()
        {
            UnityTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            UnityThreadId = Thread.CurrentThread.ManagedThreadId;
            UnitySynchronizationContext = SynchronizationContext.Current;
        }

        public static void EnsureMainThread()
        {
            if (Thread.CurrentThread.ManagedThreadId != UnityThreadId)
                throw new InvalidOperationException("Operation is not allowed on non-main thread");
        }
    }
}
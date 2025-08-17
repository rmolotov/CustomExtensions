using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomExtensions.Tasks
{
    public static class TaskExtensions
    {
        public static Task ProcessErrors(this Task task)
        {
            return task
                .ContinueWith(
                    t => Debug.LogException(t.Exception),
                    TaskContinuationOptions.OnlyOnFaulted
                );
        }

        public static Task ContinueWithUnitySynchronizationContext<TResult>(
            this Task<TResult> task,
            Action<Task<TResult>> continuationAction)
        {
            return task
                .ContinueWith(continuationAction, TaskScheduler.FromCurrentSynchronizationContext())
                .ProcessErrors();
        }
    }
}
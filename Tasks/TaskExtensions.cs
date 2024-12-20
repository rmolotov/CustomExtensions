using UnityEngine;

namespace Libs.CustomExtensions.Tasks;

public static class TaskExtensions
{
    public static Task ProcessErrors(this Task task)
    {
        return task.ContinueWith(
            t => Debug.LogException(t.Exception),
            TaskContinuationOptions.OnlyOnFaulted
        );
    }
}
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Libs.CustomExtensions.Tasks
{
    public class SynchronizationContextAwaiter : INotifyCompletion
    {
        private static readonly SendOrPostCallback PostCallback = state => ((Action)state)();
        private readonly SynchronizationContext _context;

        public SynchronizationContextAwaiter(SynchronizationContext context) =>
            _context = context;

        public bool IsCompleted => _context == SynchronizationContext.Current;

        public void OnCompleted(Action continuation) => _context.Post(PostCallback, continuation);

        public void GetResult()
        {
        }
    }

    public static class SynchronizationContextExtensions
    {
        public static SynchronizationContextAwaiter GetAwaiter(this SynchronizationContext context) =>
            new(context);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Diagnostics;

namespace ConcurrencyInCS.Reactive_Programming
{
    public static class ObservableWrapperAsync
    {
        public static IObservable<string> AsyncWrapperHotObservable(Stopwatch sw)
        {
            var task = RunAsync(milliseconds: 3000, stopwatch: sw);
            return task.ToObservable();
        }
        public static IObservable<string> AsyncWrapperColdObservable(Stopwatch sw)
        {
            return Observable.FromAsync(_ => RunAsync(milliseconds: 3000, stopwatch: sw));
        }

        private static async Task<string> RunAsync(int milliseconds, Stopwatch stopwatch)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
            return $"Task completed at {stopwatch.Elapsed.TotalMilliseconds} ms";
        }
    }
}

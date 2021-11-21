using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyInCS
{
    static class TasksExceptionHandling
    {
        public static async Task SingleTaskException()
        {
            try
            {
                //Simulate exception being raised for task
                await Task.Run(() => throw new Exception("Exception raised for Task"));
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task SingleTaskExceptionMethodCall()
        {
            try
            {
                await RunThisAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SingleTaskExceptionNonAsyncMethod()
        {
            try
            {
                var task = RunThisAsync().ConfigureAwait(false);
                task.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SingleTaskExceptionSyncMethod()
        {
            try
            {
                var task = Task.Run(() => throw new Exception("Exception raised for Task"));
                task.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void SingleTaskExceptionSyncMethod2()
        {
            try
            {
                var task = Task.Run(() => throw new Exception("Exception raised for Task"));
                task.Wait();

                if (task.Exception != null)
                    throw task.Exception;
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
        }

        public static async Task MultipleTasksSampleExceptions()
        {
            List<Task> tasks = new List<Task>();

            //Simulate exception being raised for each task
            tasks.Add(
                Task.Run(() => throw new Exception("Exception raised for first Task"))
            );

            tasks.Add(
                Task.Run(() => throw new Exception("Exception raised for second Task"))
            );

            tasks.Add(
                Task.Run(() => throw new Exception("Exception raised for third Task"))
            );

            Console.WriteLine("Waiting for all tasks to finish");

            var awaitableTask = Task.WhenAll(tasks);

            try
            {
                await awaitableTask;
            }
            catch (Exception)
            {
                //Capture each exception throw in tasks
                foreach(var e in awaitableTask.Exception.InnerExceptions)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static async Task RunThisAsync()
        {
            Console.WriteLine("Async Task started.");

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Async Task running.");
                if (i == 3)
                {
                    //Simulate exception being raised for task
                    throw new Exception("Exception raised for RunThisAsync() method");
                }
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            };

            Console.WriteLine("Async Task ended.");
        }
    }
}

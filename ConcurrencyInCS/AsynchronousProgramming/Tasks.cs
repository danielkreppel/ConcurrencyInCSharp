using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyInCS
{
    static class TasksSamples
    {
        public static async Task ProcessAsync()
        {
            Console.WriteLine("ProcessAsync method started");

            //1. 'await' mark this point to be awaited and pause the method
            //2. The thread that was processing the method returns to thread pool and is free to work on something else 
            //3. A syncronization context is created that will invoke a new thread from 
            //   thread pool to continue processing the rest of this method when RunThisAsync() is completed
            await RunThisAsync();

            for(int i=0; i<5; i++)
            {
                Console.WriteLine($"Execute some other operation {i}");
                await Task.Delay(TimeSpan.FromMilliseconds(200));
            }

            Console.WriteLine("ProcessAsync method finalized");
        }
        public static async Task<int> ProcessWithResultAsync()
        {
            Console.WriteLine("ProcessWithResultAsync method started");

            //1. 'await' mark this point to be awaited and pause the method
            //2. The thread that was processing the method returns to thread pool and is free to work on something else 
            //3. A syncronization context is created that will invoke a new thread from 
            //   thread pool to continue processing the rest of this method when GetSumAsync() is completed
            //4. "result" will contain the result of the task when it finishes

            int result = await GetSumAsync();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Execute some other operation {i}");
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }

            Console.WriteLine("ProcessWithResultAsync method finalized");

            return result;
        }

        public static async Task ProcessConcurrentlyAsync()
        {
            Console.WriteLine("ProcessConcurrentlyAsync method started");

            //1. With no 'await' to mark this point to be awaited, the execution will continue
            //2. An incomplete task is returned and stored in promiseTask variable
            //3. When the 'await' is used to await the promiseTask to finish,
            //   if the task is already finished the resulting task is returned,
            //   otherwise the method is paused, a sinchronization context is created and later a new thread will be invoked to continue the execution

            Task promiseTask = RunThisAsync();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Some other operation is running");
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }

            await promiseTask;

            Console.WriteLine("ProcessConcurrentlyAsync method finalized");

        }

        public static async Task<int> ProcessConcurrentlyWithResultAsync()
        {
            Console.WriteLine("ProcessConcurrentlyWithResultAsync method started");

            //1. With no 'await' to mark this point to be awaited, the execution will continue
            //2. An incomplete task is returned and stored in promiseTask variable
            //3. When the 'await' is used to await the promiseTask to finish,
            //   if the task is already finished the resulting task is returned,
            //   otherwise the method is paused, a sinchronization context is created and later a new thread will be invoked to continue the execution
            //4. "result" will contain the result of the task when it finishes

            Task<int> promiseTask = GetSumAsync();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Some other operation is running");
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }

            int result = await promiseTask;

            Console.WriteLine("ProcessConcurrentlyWithResultAsync method finalized");

            return result;
        }

        public static async Task MultipleTasksSample()
        {
            Task t1 = Task.Run(async () => 
            {
                await Task.Delay(3000);
                Console.WriteLine("First task completed");
            });

            Task t2 = Task.Run(async () =>
            {
                await Task.Delay(2000);
                Console.WriteLine("Second task completed");
            });

            Task t3 = Task.Run(async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("Third task completed");
            });

            Console.WriteLine("Waiting for all tasks to finish");

            await Task.WhenAll(t1, t2, t3);

            Console.WriteLine("All tasks finished");

        }

        public static async Task MultipleTasksSample2()
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(
                Task.Run(async() =>
                {
                    await Task.Delay(3000);
                    Console.WriteLine("First task completed");
                })
            );

            tasks.Add(
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    Console.WriteLine("Second task completed");
                })
            );

            tasks.Add(
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    Console.WriteLine("Third task completed");
                })
            );

            Console.WriteLine("Waiting for all tasks to finish");

            await Task.WhenAll(tasks);

            Console.WriteLine("All tasks finished");

        }

        public static async Task MultipleTasksSampleWithResult()
        {
            List<Task<int>> tasks = new List<Task<int>>();

            tasks.Add(
                Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    Console.WriteLine("First task completed");
                    return 1;
                })
            );

            tasks.Add(
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    Console.WriteLine("Second task completed");
                    return 2;
                })
            );

            tasks.Add(
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    Console.WriteLine("Third task completed");
                    return 3;
                })
            );

            Console.WriteLine("Waiting for all tasks to finish");

            //WhenAll wrap all results in an array
            int[] result = await Task.WhenAll(tasks);

            Console.WriteLine($"All tasks finished, results wrapped in array: {string.Join(", ", result)}");

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

        public static async Task MultipleTasksProcessAsCompleted()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(RunAsync(milliseconds: 3000, taskNumber: 1));
            tasks.Add(RunAsync(milliseconds: 1000, taskNumber: 2));
            tasks.Add(RunAsync(milliseconds: 2000, taskNumber: 3));

            var tasksProjection = tasks.Select(t => 
                AwaitToProcessNext(t, () => Console.WriteLine("Simulating some action to process after task completion"))
            );

            await Task.WhenAll(tasksProjection);
        }

        private static async Task AwaitToProcessNext(Task task, Action actionToProcess)
        {
            await task;
            actionToProcess();
        }

        private static async Task RunAsync(int milliseconds, int taskNumber)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
            Console.WriteLine($"Task {taskNumber} completed");
        }

        private static async Task RunThisAsync()
        {
            Console.WriteLine("Async Task started.");

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Async task running.");
                await Task.Delay(TimeSpan.FromMilliseconds(200));
            };

            Console.WriteLine("Async Task ended.");
        }

        private static async Task<int> GetSumAsync()
        {
            Console.WriteLine("Async Task started.");
            
            int sum = 0;

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Async task running.");
                sum += i;
                await Task.Delay(TimeSpan.FromMilliseconds(200));
            };

            Console.WriteLine("Async Task ended.");

            return sum;
        }
    }
}

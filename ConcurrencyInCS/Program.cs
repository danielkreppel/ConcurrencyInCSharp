using ConcurrencyInCS.AsynchronousProgramming;
using ConcurrencyInCS.ParallelProgramming;
using ConcurrencyInCS.Reactive_Programming;
using ConcurrencyInCS.TPL_Dataflows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyInCS
{
    class Program
    {
        #region THREADS - FOREGROUND VS BACKGROUND
        static void Main()
        {
        //  Threads.ForegroundSample();
        //  Threads.BackgroundSample();
        //  TasksExceptionHandling.SingleTaskExceptionNonAsyncMethod();
        }
        #endregion

        #region TASKS - PROCESS IN SEQUENCE OR CONCURRENTLY
        //static async Task Main(string[] args)
        //{
        //  await TasksSamples.ProcessAsync();
        //  await TasksSamples.ProcessConcurrentlyAsync();

        //  int result = await TasksSamples.ProcessWithResultAsync();
        //  Console.WriteLine($"Sample result: {result}");

        //  int result = await TasksSamples.ProcessConcurrentlyWithResultAsync();
        //  Console.WriteLine($"Sample result: {result}");

        //  await TasksSamples.MultipleTasksSample();
        //  await TasksSamples.MultipleTasksSample2();
        //  await TasksSamples.MultipleTasksSampleWithResult();
        //  await TasksSamples.MultipleTasksProcessAsCompleted();

        //  await TasksExceptionHandling.SingleTaskException();
        //  await TasksExceptionHandling.SingleTaskExceptionMethodCall();
        //  await TasksExceptionHandling.MultipleTasksSampleExceptions();
        //}
        #endregion

        #region AsyncEnumerable
        //static async Task Main(string[] args)
        //{
        //  Stopwatch stopWatch = new Stopwatch();
        //  stopWatch.Start();

        //  await foreach (int n in AsyncEnumerableSamples.GetElementsAsync(10))
        //  {
        //      //This will be printed in intervals of 1 second
        //      Console.WriteLine($"Receiving asynchronously element {n} after {stopWatch.ElapsedMilliseconds} ms");
        //  }

        //  stopWatch.Stop();
        //}
        #endregion

        #region AsyncEnumerable WITH FILTER
        //static async Task Main(string[] args)
        //{
        //  Stopwatch stopWatch = new Stopwatch();
        //  stopWatch.Start();

        //  //Some extension methods for Linq to process IAsyncEnumerables
        //  // can be used after installing Nuget package System.Linq.Async
        //  IAsyncEnumerable<int> asyncValues = AsyncEnumerableSamples
        //                                      .GetElementsAsync(10)
        //                                      .WhereAwait(async n => await Task.Run(() => n % 2 == 0));

        //  await foreach (int n in asyncValues)
        //  {
        //      //This will be printed in intervals of 1 second
        //      Console.WriteLine($"Receiving asynchronously element {n} after {stopWatch.ElapsedMilliseconds} ms");
        //  }

        //  stopWatch.Stop();
        //}
        #endregion

        #region AsyncEnumerable WITH FILTER AND PROJECTION
        //static async Task Main(string[] args)
        //{
        //  Stopwatch stopWatch = new Stopwatch();
        //  stopWatch.Start();

        //  //Some extension methods for Linq to process IAsyncEnumerables
        //  // can be used after installing Nuget package System.Linq.Async
        //  IAsyncEnumerable<int> asyncValues = AsyncEnumerableSamples
        //                                      .GetElementsAsync(10)
        //                                      .WhereAwait(async n => await Task.Run(() => n % 2 == 0))
        //                                      .SelectAwait(async n => await Task.Run(() => n * 2));

        //  await foreach (int n in asyncValues)
        //  {
        //      //This will be printed in intervals of 1 second
        //      Console.WriteLine($"Receiving asynchronously element {n} after {stopWatch.ElapsedMilliseconds} ms");
        //  }

        //  stopWatch.Stop();
        //}
        #endregion

        #region AsyncEnumerable WITH EXCEPTION HANDLING
        //static async Task Main(string[] args)
        //{
        //  try
        //  {
        //      Stopwatch stopWatch = new Stopwatch();
        //      stopWatch.Start();

        //      using var cancellationToken = new CancellationTokenSource(4000);
        //      CancellationToken token = cancellationToken.Token;

        //      await foreach (int n in AsyncEnumerableSamples.GetElementsAsync(10, token))
        //      {
        //          //This will be printed in intervals of 1 second
        //          Console.WriteLine($"Receiving asynchronously element {n} after {stopWatch.ElapsedMilliseconds} ms");
        //      }

        //      stopWatch.Stop();
        //  }
        //  catch(Exception ex)
        //  {
        //      Console.WriteLine(ex.Message);
        //  }
        //}
        #endregion

        #region DYNAMIC PARALLELISM
        //static void Main()
        //{
        //  int[] values = new int[] { 1, 2, 3, 4, 5 };
        //  BinaryTree tree = new BinaryTree(values);
        //       1
        //      / \
        //     2   3
        //    / \
        //   4   5

        //  DynamicParallelism.ProcessTree(tree);
        //}
        #endregion

        #region DATA AND TASK PARALLELISM
        //static void Main()
        //{
        //  var dataParallelism = new DataParallelism();
        //  dataParallelism.ReverseItemsSequentially();
        //  dataParallelism.ReverseItemsParallelForeach();
        //  dataParallelism.ReverseItemsParallelPLINQ();
        //  dataParallelism.ReverseItemsParallelForeachSharedState();

        //  var taskParallelism = new TaskParallelism();
        //  taskParallelism.ProcessSequentially();
        //  taskParallelism.ProcessWithParallelInvoke();
        //}
        #endregion

        #region OBSERVABLES
        //static void Main()
        //{
        //  //COLD OBSERVABLES
        //  var coldObservable = new ColdObservable();
        //  var notifyEvenObservable = coldObservable.NotifyEven(10);
        //  //You can subscribe using a Lambda
        //  notifyEvenObservable.Subscribe(number => Console.WriteLine(number));
        //  //or you can also subscribe using a delegate method
        //  notifyEvenObservable.Subscribe(PrintNumbers);

        //  //HOT OBSERVABLES
        //  var hotObservable = new HotObservable();
        //  var notifyObservable = hotObservable.NotifyContinuously();
        //  //Connect() starts the sequence
        //  notifyObservable.Connect();

        //  //Wait 2 seconds
        //  Thread.Sleep(2000);
        //  notifyObservable.Subscribe(i => Console.WriteLine($"Lambda observer: {i}"));

        //  //Wait more 2 seconds
        //  Thread.Sleep(2000);
        //  notifyObservable.Subscribe(PrintOnNotify);

        //  Console.ReadKey();
        //}

        static void PrintNumbers(int number)
        {
            Console.WriteLine(number);
        }
        static void PrintOnNotify(long number)
        {
            Console.WriteLine($"Delegate observer: {number}");
        }
        #endregion

        #region DATAFLOW
        //static async Task Main()
        //{
        //    var dataflowExamples = new DataflowExamples();

        //    //Console.WriteLine(await dataflowExamples.ProcessDataTransformBlock(2));

        //    //dataflowExamples.ProcessDataActionBlockWithoutParallelism();
        //    //dataflowExamples.ProcessDataActionBlockWithParallelism();

        //    //dataflowExamples.ProcessDataBatchBlock();

        //    dataflowExamples.ProcessDataJoinBlock();
        //}
        #endregion

        #region Wrapping async operation into Hot Observable
        //static void Main()
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    var observable = ObservableWrapperAsync.AsyncWrapperHotObservable(sw);

        //    Console.WriteLine($"Main(): Subscribing Observer 1 at {sw.Elapsed.TotalMilliseconds} ms");
        //    observable.Subscribe(result => Console.WriteLine($"Observer 1: {result}"));

        //    Thread.Sleep(4000);
        //    Console.WriteLine($"Main(): Subscribing Observer 2 at {sw.Elapsed.TotalMilliseconds} ms");
        //    observable.Subscribe(result => Console.WriteLine($"Observer 2: {result}"));

        //    Thread.Sleep(4000);
        //    Console.WriteLine($"Main(): Subscribing Observer 3 at {sw.Elapsed.TotalMilliseconds} ms");
        //    observable.Subscribe(result => Console.WriteLine($"Observer 3: {result}"));

        //    sw.Stop();

        //    Console.ReadKey();
        //}
        #endregion

        #region Wrapping async operation into Cold Observable
        //static void Main()
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    var observable = ObservableWrapperAsync.AsyncWrapperColdObservable(sw);

        //    Console.WriteLine($"Main(): Subscribing Observer 1 at {sw.Elapsed.TotalMilliseconds} ms");
        //    observable.Subscribe(result => Console.WriteLine($"Observer 1: {result}"));

        //    Thread.Sleep(4000);
        //    Console.WriteLine($"Main(): Subscribing Observer 2 at {sw.Elapsed.TotalMilliseconds} ms");
        //    observable.Subscribe(result => Console.WriteLine($"Observer 2: {result}"));

        //    Thread.Sleep(4000);
        //    Console.WriteLine($"Main(): Subscribing Observer 3 at {sw.Elapsed.TotalMilliseconds} ms");
        //    observable.Subscribe(result => Console.WriteLine($"Observer 3: {result}"));

        //    Console.ReadKey();
        //    sw.Stop();
        //}
        #endregion

        #region CONCURRENT COLLECTIONS AND CHANNEL SAMPLES
        //static void Main()
        //{
        //ConcurrentCollections.ConcurrentBagSample();
        //Console.WriteLine();
        //ConcurrentCollections.ConcurrentBagSampleSharedState();

        //ConcurrentCollections.BlockingCollectionProducerConsumer();

        //ConcurrentCollections.BlockingCollectionProducerConsumerThrottling();

        //ConcurrentCollections.ChannelSample();
        //ConcurrentCollections.ChannelSampleThrottling();

        //Console.ReadKey();
        //}
        #endregion  

    }
}

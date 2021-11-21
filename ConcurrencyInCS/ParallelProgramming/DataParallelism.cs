using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConcurrencyInCS.ParallelProgramming
{
    public class DataParallelism
    {
        private List<string> _dataItems = new List<string>();

        public DataParallelism()
        {
            _dataItems = GetDataItemsSample();
        }
        public void ReverseItemsSequentially()
        {
            List<string> dataItemsReversed = new List<string>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            foreach (var s in _dataItems)
            {
                dataItemsReversed.Add(s.Reverse().ToString());
            }

            stopWatch.Stop();
            Console.WriteLine("Time elapsed processing data items sequentially:" + stopWatch.Elapsed.Milliseconds);
        }

        public void ReverseItemsParallelForeach()
        {
            List<string> dataItemsReversed = new List<string>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Parallel.ForEach(_dataItems, item => dataItemsReversed.Add(item.Reverse().ToString()));
                
            stopWatch.Stop();
            Console.WriteLine("Time elapsed processing data items using Parallel.Foreach:" + stopWatch.Elapsed.Milliseconds);
        }
        public void ReverseItemsParallelForeachSharedState()
        {
            //This list is shared among the threads created by the Parallel.ForEach
            List<string> dataItemsReversed = new List<string>();

            //lock object which can be used to protect the shared state
            object _lock = new object();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Parallel.ForEach(_dataItems, item => {
                    lock (_lock)
                    {
                        //Example of how to protect this block to prevent access from multiple threads at same time
                        dataItemsReversed.Add(item.Reverse().ToString());
                    }
                }
            );

            stopWatch.Stop();
            Console.WriteLine("Time elapsed processing data items using Parallel.Foreach:" + stopWatch.Elapsed.Milliseconds);
        }

        public void ReverseItemsParallelPLINQ()
        {
            List<string> dataItemsReversed = new List<string>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            dataItemsReversed.AddRange(_dataItems.AsParallel().Select(item => item.Reverse().ToString()));

            stopWatch.Stop();
            Console.WriteLine("Time elapsed processing data items using PLINQ:" + stopWatch.Elapsed.Milliseconds);
        }

        public static List<string> GetDataItemsSample()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            List<string> dataItems = new List<string>();
            for(int i = 0; i < 15000000; i++)
            {
                dataItems.Add(
                    new string(
                        Enumerable.Repeat(chars, 7)
                        .Select(s => s[random.Next(s.Length)]).ToArray()
                        )
                    );
            }
            return dataItems;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConcurrencyInCS.ParallelProgramming
{
    public class TaskParallelism
    {
        private List<string> _dataItems = new List<string>();

        public TaskParallelism()
        {
            _dataItems = GetDataItemsSample();
        }

        public void ProcessSequentially()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            Reverse();
            CountChar('K');

            stopWatch.Stop();
            Console.WriteLine($"Time elapsed processing data items sequentially: {stopWatch.Elapsed.Milliseconds} \n");
        }

        public void ProcessWithParallelInvoke()
        {        
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Parallel.Invoke(
                () => Reverse(),
                () => CountChar('K')
            );          

            stopWatch.Stop();
            Console.WriteLine($"Time elapsed processing data items using Parallel.Invoke: {stopWatch.Elapsed.Milliseconds}" );
        }

        private IEnumerable<string> Reverse()
        {
            Console.WriteLine($"Reverse(): Processing Reverse part");

            List<string> dataItemsReversed = new List<string>();

            foreach (var word in _dataItems)
            {
                dataItemsReversed.Add(word.Reverse().ToString());
            }

            Console.WriteLine("Reverse(): All items reversed");

            return dataItemsReversed;
        }
        private int CountChar(char letter)
        {
            Console.WriteLine($"CountChar(): Processing CountChar part");

            int count = 0;

            foreach (var word in _dataItems)
            {
                count += word.Count(c => c == letter);
            }

            Console.WriteLine($"CountChar(): Count for '{letter}' found was: {count} ");

            return count;
        }

        private static List<string> GetDataItemsSample()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            List<string> dataItems = new List<string>();
            for(int i = 0; i < 16000000; i++)
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

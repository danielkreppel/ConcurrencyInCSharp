using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ConcurrencyInCS.TPL_Dataflows
{
    public class DataflowExamples
    {
        //This is a simple example to show the building process of the blocks, link and result.
        //Ideally you can implement specific factory objects to create the TransformBlocks in a more specialized way.
        public async Task<int> ProcessDataTransformBlock(int number)
        {
            var sumBlock = new TransformBlock<int, int>(item => item + 1);

            var multiplyBlock = new TransformBlock<int, int>(item => item * 2);

            var subtractBlock = new TransformBlock<int, int>(item => item - 1);

            sumBlock.LinkTo(multiplyBlock);
            multiplyBlock.LinkTo(subtractBlock);

            sumBlock.Post(number);

            return await subtractBlock.ReceiveAsync();
        }

        public void ProcessDataActionBlockWithoutParallelism()
        {
            var workerBlock = new ActionBlock<int>(ms => Task.Delay(ms));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i=0; i<5; i++)
            {
                workerBlock.Post(1000);
            }


            workerBlock.Complete();

            workerBlock.Completion.Wait();

            stopwatch.Stop();
            
            Console.WriteLine($"Elapsed seconds without parallelism: {stopwatch.Elapsed.Seconds}");
        }

        public void ProcessDataActionBlockWithParallelism()
        {
            var workerBlock = new ActionBlock<int>(ms => Task.Delay(ms), new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 5 });

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 5; i++)
            {
                workerBlock.Post(1000);
            }

            workerBlock.Complete();

            workerBlock.Completion.Wait();

            stopwatch.Stop();

            Console.WriteLine($"Elapsed seconds with parallelism: {stopwatch.Elapsed.Seconds}");
        }

        public void ProcessDataBatchBlock()
        {
            //Defining BatchBlock object which will output 5 int elements per batch
            var batchBlock = new BatchBlock<int>(5);

            //Post 15 elements to the BatchBlock object
            for (int i=1; i<15; i++)
            {
                batchBlock.Post(i);
            }

            //This will set the block to completed state and causes it to
            //propagate out any remaining values as a final batch.
            batchBlock.Complete();

            int countBatches = 0;
            while(batchBlock.OutputCount > 0)
            {
                Console.WriteLine($"The elements in batch {++countBatches} are: {string.Join(", ", batchBlock.Receive())}");
                
                //calling batchBlock.Receive() decrements batchBlock.OutputCount
            }
        }

        public void ProcessDataJoinBlock()
        {
            //JoinBlock object which requires one int element, one string and one char element;
            var joinBlock = new JoinBlock<int, string, char>();

            //At this point, joinBlock recognizes "Target1",
            //"Target2" and "Target3" to receive the corresponding data
            joinBlock.Target1.Post(1);
            joinBlock.Target2.Post("first string");
            joinBlock.Target3.Post('a');

            joinBlock.Target1.Post(2);
            joinBlock.Target2.Post("second string");
            joinBlock.Target3.Post('b');
            
            joinBlock.Target1.Post(3);
            joinBlock.Target2.Post("third string");
            joinBlock.Target3.Post('c');

            int countItems = 0;
            while (joinBlock.OutputCount > 0)
            {
                //Receiving the Tuples
                Tuple<int, string, char> data = joinBlock.Receive();
                Console.WriteLine($"Tuple {++countItems}: ({data.Item1}, {data.Item2}, {data.Item3})");
                //calling joinBlock.Receive() decrements joinBlock.OutputCount
            }
        }
    }
}

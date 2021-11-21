using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConcurrencyInCS.ParallelProgramming
{
    public static class ConcurrentCollections
    {
        public static void ConcurrentBagSample()
        {
            var concurrentBag = new ConcurrentBag<int>();
            var tasks = new List<Task>();

            //Concurrently add items through multiple Tasks
            for (int i=0; i<20; i++)
            {
                //Store the value of i in local variable before using it in tasks,
                //otherwise trying to add "i" directly in tasks would cause multiple tasks to share value of "i" at a moment
                //adding duplicates to the bag
                var number = i;
                tasks.Add(Task.Run(() => concurrentBag.Add(number)));
            }

            //Wait for all tasks to finish
            Task.WaitAll(tasks.ToArray());
            tasks.Clear();

            while (!concurrentBag.IsEmpty)
            {
                tasks.Add(
                    Task.Run(() =>
                    {
                        int item;
                        if (concurrentBag.TryTake(out item))
                        {
                            Console.Write($"-{item}");
                        }
                    })
                );
            }

            //Wait for all tasks to finish
            Task.WaitAll(tasks.ToArray());
        }

        public static void ConcurrentBagSampleSharedState()
        {
            var concurrentBag = new ConcurrentBag<int>();
            var tasks = new List<Task>();

            //Concurrently add items through multiple Tasks (shared "i" among threads issue)
            for (int i = 0; i < 20; i++)
            {
                //"i" will be shared among tasks.
                //While a task is still running (or still being created),
                //"i" can be incremented many times in meantime
                tasks.Add(Task.Run(() => concurrentBag.Add(i)));
            }

            //Wait for all tasks to finish
            Task.WaitAll(tasks.ToArray());
            tasks.Clear();

            while (!concurrentBag.IsEmpty)
            {
                tasks.Add(
                    Task.Run(() =>
                    {
                        int item;
                        if (concurrentBag.TryTake(out item))
                        {
                            Console.Write($"-{item}");
                        }
                    })
                );
            }
            
            //Wait for all tasks to finish
            Task.WaitAll(tasks.ToArray());
        }

        public static async void BlockingCollectionProducerConsumer()
        {
            using (BlockingCollection<int> bc = new BlockingCollection<int>())
            {
                //Start a concurrent producer Task without awaiting for it
                var producerTask = Task.Run(async () =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        bc.Add(i);
                        Console.WriteLine($"Producer: Adding item {i}");

                        await Task.Delay(1000); 
                    }

                    //Set the operation as finished to prevent
                    // consumers to keep waiting for more items
                    Console.WriteLine("Producer: Operation complete");
                    bc.CompleteAdding();
                });

                // Consume concurrently the blocking collection as new items are added by producer
                foreach (var item in bc.GetConsumingEnumerable())
                {
                    Console.WriteLine($"Consumer: Receiving item {item}");
                    Console.WriteLine("Waiting for more items...\n");
                }

                Console.WriteLine("No more items to read from blocking collection");

                await producerTask; 
            }
        }

        public static async void BlockingCollectionProducerConsumerThrottling()
        {
            //Setting a bounded capacity to the collection size
            //If consumers takes longer to consume the items in collection than
            //the producers takes to publish them, then the publishers will be forced to wait 
            using (BlockingCollection<int> bc = new BlockingCollection<int>(boundedCapacity: 2))
            {
                //Start a concurrent producer Task without awaiting for it
                var producerTask = Task.Run(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        //At this point, if the capacity of the collection is reached,
                        //the producer will have to wait some consumer remove some items
                        //before trying to add new items to the collection
                        if (bc.Count() == bc.BoundedCapacity)
                        {
                            Console.WriteLine("Capacity reached, waiting for memory release...");
                        }
                        bc.Add(i);
                        Console.WriteLine($"Producer: Item {i} added");
                    }

                    //Set the operation as finished to prevent
                    // consumers to keep waiting for more items
                    Console.WriteLine("Producer: Operation complete");
                    bc.CompleteAdding();
                });

                // Consume concurrently the blocking collection as new items are added by producer
                foreach (var item in bc.GetConsumingEnumerable())
                {
                    await Task.Delay(1000);
                    Console.WriteLine($"Consumer: Receiving item {item}");
                }

                Console.WriteLine("No more items to read from blocking collection");

                await producerTask;
            }
        }

        public static async void ChannelSample()
        {
            var channel = Channel.CreateUnbounded<int>();

            //Start a concurrent producer Task without awaiting for it
            var producerTask = Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await channel.Writer.WriteAsync(i);
                    Console.WriteLine($"Producer: Publish item: {i}");
                    await Task.Delay(200);
                }

                //Set the operation as finished to prevent
                //consumers to keep waiting for more items
                channel.Writer.Complete();
            });

            //Consume concurrently the channel as new items are added by producer
            while (await channel.Reader.WaitToReadAsync())
            {
                var item = await channel.Reader.ReadAsync();
                Console.WriteLine($"Consumer: Receive item: {item}");
            }

            Console.WriteLine("No more items to read from Channel");

            await producerTask;
        }

        public static async void ChannelSampleThrottling()
        {
            var channel = Channel.CreateBounded<int>(3);

            //Start a concurrent producer Task without awaiting for it
            var producerTask = Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    //At this point, if the capacity of the channel is reached,
                    //the producer will have to wait some consumer remove some items
                    //before trying to publish new items to the channel
                    await channel.Writer.WriteAsync(i);
                    Console.WriteLine($"Producer: Publish item: {i}");
                }

                //Set the operation as finished to prevent
                // consumers to keep waiting for more items
                channel.Writer.Complete();
            });

            // Consume concurrently the channel as new items are added by producer
            while (await channel.Reader.WaitToReadAsync())
            {
                await Task.Delay(500);
                var item = await channel.Reader.ReadAsync();
                Console.WriteLine($"Consumer: Receive item: {item}");
            }

            Console.WriteLine("No more items to read from Channel");

            await producerTask;
        }
    }
}

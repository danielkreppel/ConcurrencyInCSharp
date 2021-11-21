using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyInCS.ParallelProgramming
{
    public static class DynamicParallelism
    {
        public static void ProcessTree(BinaryTree root)
        {
            //Creating the main task to start traversing the binary tree
            //This will be the parent task
            Task task = Task.Factory.StartNew(
                () => Traverse(root),
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.Default);
            
            //If there are child tasks still running linked to this parent task 
            // they will be also waited at this point
            task.Wait();

            Console.WriteLine("Main task and all corresponding child tasks completed.");
        }

        static void Traverse(BinaryTree currentNode)
        {
            Console.WriteLine($"Node {currentNode.Value} traversed. Calling ProcessNode({currentNode.Value})");
            //Simulating some long running process for the node
            ProcessNode(currentNode);
            if (currentNode.Left != null)
            {
                //Creating a child task to recursively continue traversing the binary tree from the left node
                Task.Factory.StartNew(
                    () => Traverse(currentNode.Left),
                    CancellationToken.None,
                    TaskCreationOptions.AttachedToParent,
                    TaskScheduler.Default);
            }
            if (currentNode.Right != null)
            {
                //Creating a child task to recursively continue traversing the binary tree from the right node
                Task.Factory.StartNew(
                    () => Traverse(currentNode.Right),
                    CancellationToken.None,
                    TaskCreationOptions.AttachedToParent,
                    TaskScheduler.Default);
            }
        }

        static void ProcessNode(BinaryTree node)
        {
            //Creating a child task to simulate some process to be done in the node
            Task.Factory.StartNew(() =>
                    {
                        //Lets process nodes adding random delay to simulate some long operation in each one
                        var ms = new Random().Next(1000, 5000);
                        Task.Delay(ms).Wait();
                        Console.WriteLine($"Node {node.Value} processed after {ms} ms");
                    },
                    CancellationToken.None,
                    TaskCreationOptions.AttachedToParent,
                    TaskScheduler.Default);

            //The result expected is to see a concurrent processing of each node, since the
            // correspoding tasks will be executed in parallel 
        }
    }
}

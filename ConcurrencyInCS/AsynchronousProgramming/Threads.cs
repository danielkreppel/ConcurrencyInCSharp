using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyInCS
{
    public class Threads
    {
        //FOREGROUND THREAD EXAMPLE. IT KEEPS RUNNING EVEN WHEN THE MAIN THREAD IS COMPLETED.
        public static void ForegroundSample()
        {
            Console.WriteLine("Main thread started");

            Thread t = new Thread(() =>
            {
                Console.WriteLine("Foreground thread started.");

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("Foreground thread running.");
                    Thread.Sleep(100);
                };

                Console.Write("Foreground thread ended.");
            });

            t.Start();

            Thread.Sleep(100);

            Console.WriteLine("Main thread ended.");
        }

        //BACKGROUND THREAD EXAMPLE. IT IS INTERRUPTED WHEN THE MAIN THREAD IS COMPLETED.
        public static void BackgroundSample()
        {
            Console.WriteLine("Main thread started");

            Thread t = new Thread(() =>
            {
                Console.WriteLine("Background thread started.");

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("Background thread running.");
                    Thread.Sleep(100);
                };

                Console.Write("Background thread ended.");
            });

            t.IsBackground = true; //Defining this thread as a Background Thread
            t.Start();

            Thread.Sleep(100);

            Console.WriteLine("Main thread ended.");
        }
    }
}

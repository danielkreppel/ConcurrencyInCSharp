using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDeadlock
{
    public static class DeadlockSample
    {
        static async Task WaitAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
        }

        public static void Deadlock()
        {
            Console.WriteLine("Begin");
            Task task = WaitAsync();
            task.Wait();
            Console.WriteLine("End");

        }
    }
}

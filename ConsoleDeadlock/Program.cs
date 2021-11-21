using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDeadlock
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("begin");
            DeadlockSample.Deadlock();
            Console.WriteLine("end");
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyInCS.AsynchronousProgramming
{
    public static class AsyncEnumerableSamples
    {
        public static async IAsyncEnumerable<int> GetElementsAsync(
            int count, 
            [EnumeratorCancellation] CancellationToken cancelToken = default
        )
        {
            for(int i=0; i<count; i++)
            {
                //Just to simulate some work to be done before returning each element
                await Task.Delay(1000, cancelToken);

                yield return i;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyInCS.Reactive_Programming
{
    public class ColdObservable
    {
        //Example of Cold Observable. The sequence is started for each subscriber
        public IObservable<int> NotifyEven(int number)
        {
            return Observable.Create<int>(observer =>
            {
                for(int i=1; i<=number; i++)
                {
                    if (i % 2 == 0)
                        observer.OnNext(i);
                }

                observer.OnCompleted();
                return Disposable.Empty;
            });
        }
    }

    
}

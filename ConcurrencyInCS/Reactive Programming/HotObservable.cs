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
    public class HotObservable
    { 
        //Example of Hot Observable. 
        // Publish() method returns a IConnectableObservable, which inherits from IObservable
        // and adds a Connect() method for subscribers to connect and receive the events in a shared way. 
        public IConnectableObservable<long> NotifyContinuously()
        {
            return Observable.Interval(TimeSpan.FromSeconds(1)).Publish();
        }
    }

    
}

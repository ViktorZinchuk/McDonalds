using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace McDonald_s
{
    class McDonaldQueue
    {
        public ConcurrentQueue<IClient> queue = new ConcurrentQueue<IClient>();
        public  AutoResetEvent Queue_event = new AutoResetEvent(false);
    }
}

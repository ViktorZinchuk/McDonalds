using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace McDonald_s
{
    class ClientCreater
    {
        private Timer timer;
        private McDonalds mcdonald;
        private int numberOfclients;
        public ClientCreater(McDonalds mcdonald,double interval, int clients)
        {
            this.mcdonald = mcdonald;
            numberOfclients = clients;
            timer = new Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(CreateClient);
        }
        public void CreateClient(object source, ElapsedEventArgs e)
        {
            McDonaldQueue Queue = mcdonald.Queues[0];
            for(int i=0; i<numberOfclients;i++)
            {
                for (int j = 0; j < mcdonald.Queues.Count; j++)
                {
                    if (Queue.queue.Count > mcdonald.Queues[j].queue.Count)
                    {
                        Queue = mcdonald.Queues[j];
                    }
                } 
                Queue.queue.Enqueue(new Client());
                Queue.Queue_event.Set();
            }
        }
        public void StartTimer()
        {
            timer.Start();
        }
    }
}

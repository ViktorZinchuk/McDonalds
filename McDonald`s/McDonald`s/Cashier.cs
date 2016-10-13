using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace McDonald_s
{
    class Cashier
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(Cashier));  
        public AutoResetEvent StopWorkEvent = new AutoResetEvent(false);
        private double profit;
        private Dictionary<Menu, Table> tables;
        private McDonaldQueue Queue;
        private Dictionary<Menu, double> menu;
        private volatile bool work = true;
        private McDonalds mc;
        public Cashier(Dictionary<Menu, Table> tables, McDonaldQueue Queue, Dictionary<Menu, double> menu, McDonalds mc)
	    {
            this.tables = tables;
            this.Queue = Queue;
            this.menu = menu;
            this.mc = mc;
            profit = 0;
	    }
        private Dictionary<Menu,int> TakeOrder(IClient client)
        {
            Dictionary<Menu, int> order = client.MakeOrder();
            log.Info("I work");
            order.ToList().ForEach(z =>  log.Info(z));
            return order;
        }
        private List<Food> ExecuteOrder(IClient client,Dictionary<Menu,int> order)
        {
            List<Food> tray = new List<Food>();
            Array values = Enum.GetValues(typeof(Menu));
            foreach(Menu key in values)
            {
                if (order.ContainsKey(key))
                {
                    tables[key].GetFood(order[key], tray,key.ToString());
                }
            }
            return tray;
        }
        private void GetMoney(IClient client,Dictionary<Menu,int> order)
        {
             List<Menu> ClientsOrder= new List<Menu>(order.Keys);
             double payment = 0;
             foreach (var key in ClientsOrder)
             {
                 payment += menu[key] * order[key];
             }
             profit+=client.Pay(payment);
             log.Info("Payment: "+payment+" profit:" + profit);
        }
        public void Work()
        {
            while(work)
            {
                if (Queue.queue.Count == 0)
                {
                    log.Info("Free Service ");
                    Queue.Queue_event.Reset();
                    Queue.Queue_event.WaitOne();
                }

                IClient client;
                Queue.queue.TryDequeue(out client);
                Dictionary<Menu, int> clients_order = TakeOrder(client);
                GetMoney(client, clients_order);
                client.TakePlate(ExecuteOrder(client, clients_order));
            }
            SendProfit();      
        }
        public void StopWork()
        {
            work = false;
        }
        public void SendProfit()
        {
            mc.Profit(profit);
            log.Info("I go home profit:" + profit + "!");
            StopWorkEvent.Set();
        }
    }
}

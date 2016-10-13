using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace McDonald_s
{
    class Table
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(Cashier));
        public  ManualResetEvent TableNotFull = new ManualResetEvent(false);
        private  List<Food> table = new List<Food>();
        private volatile int numberoffood;
        public int TableCount
        {
            get
            {
                numberoffood = table.Count;
                return numberoffood;
            }
        }
        public ManualResetEvent Table_event = new ManualResetEvent(false);
        private Object tableLock = new Object();
        private Object CashierLock = new Object();

        public void AddFood(Food food)
        {
            lock (tableLock)
            {
                table.Add(food);
                Table_event.Set();
                if (TableCount == 10)
                {
                    TableNotFull.Reset();
                }
            }
        }
        public void GetFood(int numberOfFood, List<Food>tray, string food)
        {
            lock (CashierLock)
            {
                for (int i = 0; i < numberOfFood; i++)
                {
                    lock (tableLock)
                    {
                        if (TableCount == 0)
                        {
                            log.Info("I`m Wait " + food + "!");
                            TableNotFull.Set();
                            log.Info("Cook make for me " + food);
                            Table_event.Reset();
                        }
                    }
                    Table_event.WaitOne();
                    lock (tableLock)
                    {
                        log.Info("Table " + TableCount+" "+food);
                        tray.Add(table[0]);
                        table.RemoveAt(0);                     
                    } 
                }
            }         
        }
    }
}

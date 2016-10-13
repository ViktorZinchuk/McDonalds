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
    class McDonalds
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(Cashier)); 
        public List<McDonaldQueue> Queues;
        private Dictionary<Menu,Table> Tables;
        private List<Cashier> Cashiers;
        private List<Cook> Cooks;
        private Dictionary<Menu, double>menu;
        private AutoResetEvent[] CashiersStopWorkEvents;
        public Object Lock = new Object();
        public void Profit(double profit)
        {
            lock (Lock)
            {
                McDonaldsProfit += profit;
            }
        }             
        private double McDonaldsProfit;
        public McDonalds()
        {
            Queues = new List<McDonaldQueue>();
            Queues.Add(new McDonaldQueue());
            Queues.Add(new McDonaldQueue());
            Queues.Add(new McDonaldQueue());
            menu = new Dictionary<Menu, double>();
            menu.Add(Menu.Burger, 18);
            menu.Add(Menu.Cheeseburger, 20.20);
            menu.Add(Menu.ChippedPotato, 25.00);
            menu.Add(Menu.IceCream, 15.00);
            menu.Add(Menu.MacChiken,30.00);

            Tables = new Dictionary<Menu,Table>();
            Tables.Add(Menu.Burger,new Table());
            Tables.Add(Menu.Cheeseburger,new Table());
            Tables.Add(Menu.MacChiken,new Table());
            Tables.Add(Menu.ChippedPotato,new Table());
            Tables.Add(Menu.IceCream,new Table());

            Cashiers = new List<Cashier>();
            Cashiers.Add(new Cashier(Tables, Queues[0], menu, this));
            Cashiers.Add(new Cashier(Tables, Queues[1], menu, this));
            Cashiers.Add(new Cashier(Tables, Queues[2], menu, this));
            CashiersStopWorkEvents= new AutoResetEvent[Cashiers.Count];
            for (int i = 0; i < Cashiers.Count;i++ )
            {
                CashiersStopWorkEvents[i] = Cashiers[i].StopWorkEvent;
            }

            Cooks = new List<Cook>();
            Cooks.Add(new Cook(Tables[Menu.Burger], new BurgerRecip()));
            Cooks.Add(new Cook(Tables[Menu.Cheeseburger], new CheeseburgerRecip()));
            Cooks.Add(new Cook(Tables[Menu.MacChiken], new MacChikenRecip()));
            Cooks.Add(new Cook(Tables[Menu.ChippedPotato], new ChipedPotatoRecip()));
            Cooks.Add(new Cook(Tables[Menu.IceCream], new IceCreamRecip()));
            McDonaldsProfit = 0;         
        }

        public void Work()
        {
            log.Info("McDonald`s Open");
            int Cooknumber = 1;
            foreach (var cook in Cooks)
            {
                Thread CookThread = new Thread(cook.Work);
                CookThread.Name = "Cook_" + Cooknumber;
                CookThread.Start();
                Cooknumber++;
            }
            int Cashier_number = 1;
            foreach (var cashier in Cashiers)
            {
                Thread CashierThread = new Thread(cashier.Work);
                CashierThread.Name = "cashier_" + Cashier_number;
                CashierThread.Start();
                Cashier_number++;
           }
        }

        public void StopWork()
        {
            foreach (var cashier in Cashiers)
            {
                cashier.StopWork();
            }
            WaitHandle.WaitAll(CashiersStopWorkEvents);
            foreach (var cook in Cooks)
            {
                cook.StopWork();
            }
            log.Info("McDonald`s Profit is: "+ McDonaldsProfit);
        }

    }
}

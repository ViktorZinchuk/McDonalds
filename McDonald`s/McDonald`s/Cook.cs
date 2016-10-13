using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace McDonald_s
{
    class Cook
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(Cashier)); 
        private Table Table;
        private RecipBook recip;
        private volatile bool work= true;
        public Cook(Table Table, RecipBook recip)
        {
            this.Table = Table;
            this.recip = recip;
        }
        private void Prepare()
        {
            Table.AddFood(recip.PrepareFood());
            log.Info("I prepared food");  
        }
        private void Rest()
        {
            log.Info("Table is Full, I`m resting " + Table.TableCount);
            Table.TableNotFull.WaitOne();
            if (work)
            {
                log.Info("I go work");
            }
        }
        public void Work()
        {
            while(work)
            {     
                while (Table.TableCount < 10)
                {
                    if (!work)
                    {
                        break;
                    }                      
                    Prepare();
                }
                if (work)
                {
                    Rest();
                }
            }
            log.Info("I go home");
        }
        public void StopWork()
        {
            work = false;
            Table.TableNotFull.Set();
        }
    }
}

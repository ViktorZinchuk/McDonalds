using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;


namespace McDonald_s
{
    class Program
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(Program)); 
        static void Main(string[] args)
        {
            log4net.Config.DOMConfigurator.Configure();
            McDonalds mcDonalds = new McDonalds();
            mcDonalds.Work();
            ClientCreater creater = new ClientCreater(mcDonalds,10,100);
            creater.StartTimer();
            ConsoleKeyInfo key = Console.ReadKey(true);
            if(key.Key == ConsoleKey.X)
            {
                mcDonalds.StopWork();
            }
            Console.ReadLine();
        }
    }
}

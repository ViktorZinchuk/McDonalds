using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDonald_s
{
    class Client :IClient
    {
        public Dictionary<Menu, int> MakeOrder()
        {
            Dictionary<Menu, int> order = new Dictionary<Menu, int>();
            Random rand = new Random();
            int orders = rand.Next(1, 5);
            Array values = Enum.GetValues(typeof(Menu));
            for (int i = 1; i <= orders; i++)
            {
                var key = (Menu)values.GetValue(rand.Next(values.Length));
                int AmountOfFood = rand.Next(1, 10);
                if (!order.ContainsKey(key))
                {
                    order.Add(key, AmountOfFood);
                }
                else
                {
                    order[key] += AmountOfFood;
                }
            }           
            return order;
        }

        public double Pay(double paymant)
        {
           return paymant;
        }

        public void TakePlate(List<Food> plate)
        {
            plate.Clear();
        }
    }
}

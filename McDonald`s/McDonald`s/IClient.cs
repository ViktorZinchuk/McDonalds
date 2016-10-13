using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDonald_s
{
    enum Menu
    {
        Burger,
        Cheeseburger,
        MacChiken,
        ChippedPotato,
        IceCream
    }
    interface IClient
    {
        Dictionary<Menu,int> MakeOrder();
        double Pay(double paymant);
        void TakePlate(List<Food> plate);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace McDonald_s
{
    class BurgerRecip: RecipBook
    {
        public override Food PrepareFood()
        {
            Thread.Sleep(2);
            return new Food();
        }
    }
}

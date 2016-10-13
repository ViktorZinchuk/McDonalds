using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace McDonald_s
{
    class ChipedPotatoRecip : RecipBook
    {
        public override Food PrepareFood()
        {
            Thread.Sleep(5);
            return new Food();
        }
    }
}

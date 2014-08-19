using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace PizzaMilano
{
    class Order
    {
        public List<Pizza> myPizzas;
        public List<Sandwitch> mySandwicthes;
        public List<Other> myOthers;
        public Order()
        {
            myPizzas = new List<Pizza>();
            mySandwicthes = new List<Sandwitch>();
            myOthers = new List<Other>();
        }

        
    }
}

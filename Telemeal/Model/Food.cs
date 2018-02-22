using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemeal.Model
{
    public class Food
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public Main_Category MainCtgr { get; set; }
        public Sub_Category SubCtgr { get; set; }
    }

    // To represent the All Button
    public enum Main_Category
    {
        All
    }

    // To represent the differet categories of the food
    public enum Sub_Category
    {
        Drink,
        Appetizer,
        Main,
        Dessert
    }
}
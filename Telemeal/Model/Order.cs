using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemeal.Model
{
    public class Order
    {
        public int OrderID { get; set; }
        public double Total { get; set; }
        public double SalesTax { get; set; }
        public DateTime OrderDateTime { get; set; }
        public bool IsTakeOut { get; set; }
        public List<Food> Foods { get; set; }
    }
}

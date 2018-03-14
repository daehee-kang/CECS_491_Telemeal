using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemeal.Model
{
    public class Invoice
    {
        public int InvoiceNumber { get; set; }
        public DateTime DateOfInvoice { get; set; }
        public List<Order> Orders { get; set; }
        public double getTotal()
        {
            if (Orders.Count == 0) { return 0; }
            else
            {
                return Orders.Select(x => x.Total).Sum();
            }
        }
    }
}

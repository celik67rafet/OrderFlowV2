using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlowV2.ConsoleClient
{
    public class StockDto
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public decimal price { get; set; } 
        public int count { get; set; }
    }
}

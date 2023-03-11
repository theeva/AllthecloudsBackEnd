using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudProductsBackEnd.Models
{
    public class Product
    {
        public string name { get; set; }
        public double unitPrice { get; set; }
        public double markupPrice { get; set; }
        public string productId { get; set; }
        public string description { get; set; }
        public int? maximumQuantity { get; set; }
    }
}

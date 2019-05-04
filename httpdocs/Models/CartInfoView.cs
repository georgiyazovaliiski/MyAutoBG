using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class CartInfoView
    {
        public int Count { get; set; }
        public double WholePrice { get; set; }
        public CartInfoView(int Count, double WholePrice)
        {
            this.Count = Count;
            this.WholePrice = WholePrice;
        }
        public CartInfoView() {

        }
    }
}
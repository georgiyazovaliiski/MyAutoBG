using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class Price
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public double WholePrice { get; set; }
        public DateTime LastCheckOut { get; set; }
        public double LastPricePaid { get; set; }
    }
}
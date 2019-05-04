using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class AddressType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Order> Orders {get;set;}
    }
}
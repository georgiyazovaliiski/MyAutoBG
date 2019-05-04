using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ContactAddress { get; set; }

        public int AddressTypeId { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime DateOfOrderStart { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime? DateOfOrderFinish { get; set; }
        public bool termsAndConditionsAgreement { get; set; }
        public bool approved { get; set; }

        [ForeignKey("AddressTypeId")]
        public AddressType AddressType { get; set; }

        public Order(string userId,
            string name,
            string lastName,
            string phone,
            int addressType,
            DateTime start,
            bool termsAndConditionsAgreement,
            string contactAddress,
            string address)
        {
            this.Address = address;
            this.AddressTypeId = addressType;
            this.UserId = userId;
            this.Name = name;
            this.LastName = lastName;
            this.Phone = phone;
            this.DateOfOrderStart = start;
            this.termsAndConditionsAgreement = termsAndConditionsAgreement;
            this.approved = false;
            this.ContactAddress = contactAddress;
        }
        public Order()
        {

        }
    }
}
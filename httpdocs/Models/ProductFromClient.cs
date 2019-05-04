using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoPartsMVC.Models
{
    [Table("ProductsFromClient")]
    public class ProductFromClient : Product
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public String Email { get; set; }

        public String PhoneNumber { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Country { get; set; }

        public String City { get; set; }

        public bool Approved { get; set; }

        public ProductFromClient(String Email,
            String PhoneNumber,
            String FirstName,
            String LastName,
            double Price,
            string Name,
            string Description,
            int TimesBought,
            int CategoryId,
            string ProductImageName,
            double PriceEN,
            string NameEN,
            string DescriptionEN,
            string urlName
            ) : base(
                Price,
                Name,
                Description,
                TimesBought,
                5,
                ProductImageName,
                PriceEN,
                NameEN,
                DescriptionEN,
                urlName
                )
        {
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Approved = false;
        }
        public ProductFromClient()
        {

        }
    }
}
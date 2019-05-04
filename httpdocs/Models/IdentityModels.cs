using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AutoPartsMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFromClient> ProductsFromClients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Promo> Promos { get; set; }
        public DbSet<SpecialOffer> SpecialOffers { get; set; }
        public DbSet<ProductsInCarts> ProductsInCarts { get; set; }
        public DbSet<BoughtProducts> BoughtProducts { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<OrderProductQuantity> OrdersProductsQuantities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
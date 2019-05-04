namespace AutoPartsMVC.Migrations
{
    using AutoPartsMVC.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AutoPartsMVC.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AutoPartsMVC.Models.ApplicationDbContext";
        }

        protected override void Seed(AutoPartsMVC.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            Category category1 = new Category("За колата", 0, "Secondary", "For the car");
            Category category2 = new Category("За камиона", 0, "Secondary", "For the truck");
            Category category3 = new Category("Инструменти", 0, "Secondary", "Tools");
            Category category4 = new Category("Други", 0, "Secondary", "Other");
            Category category5 = new Category("От вас", 0, "Secondary", "From users");

            if (!context.Categories.Any()) {
                context.Categories.AddOrUpdate(
                                category1,
                                category2,
                                category3,
                                category4,
                                category5
                );
                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppAdmin" };

                manager.Create(role);
            }



            if (!context.Users.Any())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "shopadmin@abv.bg", Email = "shopadmin@abv.bg", LockoutEnabled = true };
                manager.Create(user,"123123");
                var user_Id = context.Users.Select(a=>a.Id).FirstOrDefault();
                manager.AddToRole(user_Id, "AppAdmin");
            }
            if(!context.Categories.Any())
            {
                var newCategory = new Category();
                newCategory.Name = "Каталог";
                newCategory.SubCategoryId = 0;
                newCategory.Type = "Primary";
                context.Categories.Add(newCategory);
            }
            context.SaveChanges();
            
        }
    }
}

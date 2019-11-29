using System;
using System.Linq;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;

namespace DAL.App.EF
{
    public static class DataSeeder
    {
        public static void SeedInitialData(AppDbContext ctx, UserManager<AppUser> userManager)
        {
            
            ctx.ProductTypes.Add(new ProductType()
            {
                ProductTypeName = "Pizza",
            });
            ctx.ProductTypes.Add(new ProductType()
            {
                ProductTypeName = "Drink",
            });
            ctx.SaveChanges();
            ctx.Transports.Add(new Transport()
            {
                TransportName = "Pizza Taxi 123",
                TransportValue = 3
            });
            ctx.Transports.Add(new Transport()
            {
                TransportName = "Fast delivery 123",
                TransportValue = 4
            });
            ctx.SaveChanges();
            ctx.OrderStates.Add(new OrderState()
            {
                OrderStateName = "Waiting"
            });
            ctx.OrderStates.Add(new OrderState()
            {
                OrderStateName = "Paid"
            });
            ctx.OrderStates.Add(new OrderState()
            {
                OrderStateName = "In Process"
            });
            ctx.OrderStates.Add(new OrderState()
            {
                OrderStateName = "Done"
            });
            ctx.SaveChanges();
            ctx.Components.Add(new Component()
            {
                ComponentName = "Bacon",
                ComponentValue = 1
            });
            ctx.Components.Add(new Component()
            {
                ComponentName = "Cheese",
                ComponentValue = 1
            });
            ctx.Components.Add(new Component()
            {
                ComponentName = "Olives",
                ComponentValue = 1
            });
            ctx.Components.Add(new Component()
            {
                ComponentName = "Onion",
                ComponentValue = 1
            });
            ctx.Components.Add(new Component()
            {
                ComponentName = "Pineapple",
                ComponentValue = 1
            });
            ctx.Components.Add(new Component()
            {
                ComponentName = "Tomato",
                ComponentValue = 1
            });
            ctx.SaveChanges();
            
            ctx.Products.Add(new Product()
            {
                ProductName = "Americana",
                ProductDescription = "Pizza sauce, cheese, salami, pineapple, olives",
                ProductValue = 4,
                ProductTypeId = 1
            });
            ctx.Products.Add(new Product()
            {
                ProductName = "Cananas",
                ProductDescription = "Pizza sauce, cheese, smoked chicken, sweet pepper, pineapple",
                ProductValue = 4,
                ProductTypeId = 1
            });
            ctx.Products.Add(new Product()
            {
                ProductName = "Francescana",
                ProductDescription = "Pizza sauce, cheese, ham, salted mushrooms, sweet pepper",
                ProductValue = 4,
                ProductTypeId = 1
            });
            ctx.Products.Add(new Product()
            {
                ProductName = "Mafioso",
                ProductDescription = "Pizza sauce, cheese, bacon, field mushrooms, onion, sweet pepper, olives",
                ProductValue = 4,
                ProductTypeId = 1
            });
            ctx.Products.Add(new Product()
            {
                ProductName = "Pepperone",
                ProductDescription = "Pizza sauce, cheese, pepperoni, tomato, field mushrooms, onion",
                ProductValue = 4,
                ProductTypeId = 1
            });
            ctx.Products.Add(new Product()
            {
                ProductName = "Siciliana",
                ProductDescription = "Pizza sauce, cheese, ham, garlic, onion",
                ProductValue = 4,
                ProductTypeId = 1
            });
            ctx.Products.Add(new Product()
            {
                ProductName = "Coca-cola",
                ProductDescription = "0.5L",
                ProductValue = 2,
                ProductTypeId = 2
            });
            ctx.Products.Add(new Product()
            {
                ProductName = "Sprite",
                ProductDescription = "0.5L",
                ProductValue = 2,
                ProductTypeId = 2
            });
            ctx.SaveChanges();
            
            var appUser = new AppUser();
            appUser.Email = "b@b.ee";
            appUser.UserName = appUser.Email;

            var res = userManager.CreateAsync(appUser, "Password123.").Result;
            if (!res.Succeeded)
            {
                throw new Exception("Identity error");
            }
            ctx.SaveChanges();

            ctx.FullOrders.Add(new FullOrder()
            {
                Sum = 5,
                AppUserId = 1
            });
            ctx.SaveChanges();
            ctx.OrderLines.Add(new OrderLine()
            {
                ProductQuantity = 1,
                ProductValue = 4,
                LineSum = 5,
                ProductId = 2,
                FullOrderId = 1
            });
            ctx.SaveChanges();
            ctx.ComponentInLines.Add(new ComponentInLine()
            {
                OrderLineId = 1,
                ComponentId = 1
            });
            ctx.SaveChanges();
            
            var appUser2 = new AppUser();
            appUser2.Email = "c@c.ee";
            appUser2.UserName = appUser2.Email;

            var res2 = userManager.CreateAsync(appUser2, "Password123.").Result;
            if (!res2.Succeeded)
            {
                throw new Exception("Identity error");
            }
            ctx.SaveChanges();

            ctx.FullOrders.Add(new FullOrder()
            {
                Sum = 5,
                AppUserId = 2
            });
            ctx.SaveChanges();
            ctx.OrderLines.Add(new OrderLine()
            {
                ProductQuantity = 1,
                ProductValue = 4,
                LineSum = 5,
                ProductId = 2,
                FullOrderId = 2
            });
            ctx.SaveChanges();
            ctx.ComponentInLines.Add(new ComponentInLine()
            {
                OrderLineId = 2,
                ComponentId = 1
            });
            ctx.SaveChanges();
        }
        
        public static void CreateRoles(this IApplicationBuilder app, UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (!roleManager.Roles.Any(r => r.NormalizedName == "ADMIN"))
            {
                var result = roleManager.CreateAsync(new AppRole() {Name = "Admin"}).Result;
                if (result == IdentityResult.Success)
                {
                }
            }
            var user = userManager.FindByEmailAsync("a@a.ee").Result;
            if (user == null)
            {
                var result = userManager.CreateAsync(new AppUser()
                {
                    Email = "a@a.ee",
                    UserName = "a@a.ee"
                }, "Password123.").Result;
                if (result == IdentityResult.Success)
                {
                    user = userManager.FindByEmailAsync("a@a.ee").Result;
                }
            }
            if (user != null && !userManager.IsInRoleAsync(user, "Admin").Result)
            {
                var result = userManager.AddToRoleAsync(user, "Admin").Result;
            }
            
            // User b
            if (!roleManager.Roles.Any(r => r.NormalizedName == "USER"))
            {
                var result = roleManager.CreateAsync(new AppRole() {Name = "User"}).Result;
                if (result == IdentityResult.Success)
                {
                }
            }
            var user2 = userManager.FindByEmailAsync("b@b.ee").Result;
            if (user2 != null && !userManager.IsInRoleAsync(user2, "User").Result)
            {
                var result = userManager.AddToRoleAsync(user2, "User").Result;
            }
            
            // User c
            var user3 = userManager.FindByEmailAsync("c@c.ee").Result;
            if (user3 != null && !userManager.IsInRoleAsync(user3, "User").Result)
            {
                var result = userManager.AddToRoleAsync(user3, "User").Result;
            }
        }
    }
}
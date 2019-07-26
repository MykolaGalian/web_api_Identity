using DAL.Identity;
using DAL.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class BDInitializer : CreateDatabaseIfNotExists<ProdDBContext>
    {
        protected override void Seed(ProdDBContext context)
        {
            var userManager = new ApplicationUserManager(new CustomUserStore(context));

            var roleManager = new ApplicationRoleManager(new CustomRoleStore(context));

            var role1 = new CustomRole("user");            
            var role2 = new CustomRole("admin");

            roleManager.Create(role1);           
            roleManager.Create(role2);

            var admin = new ApplicationUser()
            {
                Email = "myko@gmail.com",
                UserName = "Myko",
                UserInfo = new UserInfo() { Name = "Myk", Login = "Myko", DateRegistration = DateTime.Now }
            };
            var result = userManager.Create(admin, "111111");

            var user1 = new ApplicationUser()
            {
                Email = "kos@mail.ru",
                UserName = "Kostya",
                UserInfo = new UserInfo() { Name = "Kos", Login = "Kostya", DateRegistration = DateTime.Now }
            };
            var result2 = userManager.Create(user1, "222222"); 


            var user2 = new ApplicationUser()
            {
                Email = "Jen@mail.ru",
                UserName = "Jenya",
                UserInfo = new UserInfo() { Name = "Jen", Login = "Jenya", DateRegistration = DateTime.Now }
            };
            var result3 = userManager.Create(user2, "333333"); // min 6 characters
            context.SaveChanges();

            if (result.Succeeded && result2.Succeeded && result3.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);               
                userManager.AddToRole(user1.Id, role1.Name);
                userManager.AddToRole(user2.Id, role1.Name);
            }
            context.SaveChanges();


            Product pr1 = new Product()
                    {
                        ProductName = "FCV-123",
                        ProductPrice = 122.21f
                    };
                    Product pr2 = new Product()
                    {
                        ProductName = "CVB-1763",
                        ProductPrice = 100.10f
                    };
                    Product pr3 = new Product()
                    {
                        ProductName = "CFHB-17EB",
                        ProductPrice = 10000.10f
                    };
                    Product pr4 = new Product()
                    {
                        ProductName = "CFDFC-43E",
                        ProductPrice = 12000.10f
                    };
                    
                    context.Products_.AddRange(new[] { pr1, pr2, pr3, pr4 });
                    context.SaveChanges();


                    Order or1 = new Order()
                    {
                        OrderDate = new DateTime(2018, 5, 1, 8, 30, 52)
                    };
                    Order or2 = new Order()
                    {
                        OrderDate = new DateTime(2018, 6, 11, 8, 30, 50)
                    };


                    context.Orders_.AddRange(new[] { or1, or2});
                    context.SaveChanges();

                    OrderDetail od1 = new OrderDetail() {Product = pr1, Quantity = 2, Order = or1, UserInfoId = 2 };
                    OrderDetail od2 = new OrderDetail() {Product = pr2, Quantity = 3, Order = or1, UserInfoId = 2 };
                    OrderDetail od3 = new OrderDetail() { Product = pr3, Quantity = 1, Order = or2, UserInfoId = 3 };
                    OrderDetail od4 = new OrderDetail() { Product = pr4, Quantity = 4, Order = or2, UserInfoId = 3 };

                    context.OrderDetails_.AddRange(new[]{od1, od2, od3, od4});
                    context.SaveChanges();

                
            
        }
    }
}

using DAL.Identity;
using DAL.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class ProdDBContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {

        static ProdDBContext()
        {
            Database.SetInitializer<ProdDBContext>(new BDInitializer());  //remove row after Initializer
        }
        public ProdDBContext() : base("ProdConection")
        {
           
        }
        public DbSet<Product> Products_ { get; set; }
        public DbSet<Order> Orders_ { get; set; }
        public DbSet<OrderDetail> OrderDetails_ { get; set; }
        public DbSet<UserInfo> UserInfo_ { get; set; }
    }
}

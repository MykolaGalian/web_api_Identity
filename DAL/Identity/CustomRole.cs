using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.Identity
{
    public class CustomRole : IdentityRole<int, CustomUserRole>
    {

        public CustomRole(string name) { Name = name; }
        public CustomRole() { }
    }
}

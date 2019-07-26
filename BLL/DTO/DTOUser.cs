using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class DTOUser
    {
        public int Id { get; set; }
        public string Name { get; set; }            
        public string Password { get; set; } //from Identity
        public string Email { get; set; }
        public string Login { get; set; }       
        public bool IsBlocked { get; set; }
        public DateTime? DateRegistration { get; set; }       
        public List<string> Roles { get; set; }      

        public virtual ICollection<Order> Orders { get; set; }
    }
}

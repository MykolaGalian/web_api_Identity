using DAL.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    [Table("UserInfo")]
    public class UserInfo
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
        [ForeignKey("ApplicationUser")]
        public  int Id { get; set; }       
        public string Name { get; set; }
        public string Login { get; set; }
        public DateTime? DateRegistration { get; set; }

        public bool IsBlocked { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }


    }
}

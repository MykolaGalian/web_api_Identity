using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IDTOUnitOfWork
    {
       IProductService productServices { get; }
       IOrderService orderServices { get; }
       IUserInfoService userInfoService { get; }
       IAdminService adminService { get; }
       IUserManagerService userManagerService { get; }
}
}

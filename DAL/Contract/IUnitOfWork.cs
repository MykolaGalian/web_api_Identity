using DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contract
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }

        IOrderRepository orderRepository { get; }
        IProductRepository productRepository { get; }
        IOrederDetailRepository orederDetailRepository { get; }

        IUserInfoRepository userInfoRepository { get; }
        Task Save();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contract;
using DAL.EF;
using DAL.Identity;

namespace DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ProdDBContext _db;
        private OrderRepository _orderRepository;
        private ProductRepository _productRepository;
        private OrederDetailRepository _orederDetailRepository;
        private UserInfoRepository _userInfoRepository;

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public UnitOfWork(ProdDBContext db)
        {
            _db = db;
        }


        public ApplicationUserManager UserManager
        {
            get
            {
                if (this._userManager == null)
                {
                    this._userManager = new ApplicationUserManager(new CustomUserStore(_db));
                }
                return _userManager;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                if (this._roleManager == null)
                {
                    this._roleManager = new ApplicationRoleManager(new CustomRoleStore(_db));
                }
                return _roleManager;
            }
        }

        public IUserInfoRepository userInfoRepository
        {
            get
            {
                if (this._userInfoRepository == null)
                {
                    this._userInfoRepository = new UserInfoRepository(_db);
                }
                return _userInfoRepository;
            }
        }

        public IOrderRepository orderRepository
        {
            get
            {
                if (this._orderRepository == null)
                {
                    this._orderRepository = new OrderRepository(_db);
                }
                return _orderRepository;
            }
        }

        public IProductRepository productRepository
        {
            get
            {
                if (this._productRepository == null)
                {
                    this._productRepository = new ProductRepository(_db);
                }
                return _productRepository;
            }
        }

        public IOrederDetailRepository orederDetailRepository
        {
            get
            {
                if (this._orederDetailRepository == null)
                {
                    this._orederDetailRepository = new OrederDetailRepository(_db);
                }
                return _orederDetailRepository;
            }
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync(); // for service
        }


        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

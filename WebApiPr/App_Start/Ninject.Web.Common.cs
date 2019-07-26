[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApiPr.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebApiPr.App_Start.NinjectWebCommon), "Stop")]

namespace WebApiPr.App_Start
{
    using System;
    using System.Web;
    using System.Web.Http;
    using BLL.Contracts;
    using BLL.Infostructure;
    using BLL.Services;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using WebApiContrib.IoC.Ninject;

    public static class NinjectWebCommon 
    {
        public static IKernel Kernel { get; private set; }

        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var modules = new INinjectModule[] { new DIResolver("ProdConection") };
             Kernel = new StandardKernel(modules);
            try
            {
                Kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                Kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(Kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(Kernel); // Resolver DI -  WebApiContrib.IoC.Ninject/ Ninject.Web.WebApi
                return Kernel;
            }
            catch
            {
                Kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDTOUnitOfWork>().To<DTOUnitOfWork>();
        }        
    }
}
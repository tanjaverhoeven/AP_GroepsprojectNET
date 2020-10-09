using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using DotnetAcademy.API;
using DotnetAcademy.API.Controllers;
using DotnetAcademy.BLL;
using DotnetAcademy.BLL.Identity;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL;
using DotnetAcademy.DAL.Models;
using DotnetAcademy.DAL.Repositories;
using DotnetAcademy.DAL.Repositories.Identity;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace DotnetAcademy.API
{
    public static class NinjectWebCommon 
    {
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
            StandardKernel kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

            kernel.Bind<ICustomerRepository>().To<CustomerRepository>().InRequestScope();
            kernel.Bind<IDbRepository<Product>>().To<ProductRepository>().InRequestScope();
            kernel.Bind<IDbRepository<Invoice>>().To<InvoiceRepository>().InRequestScope();
            kernel.Bind<IDbRepository<DetailLine>>().To<DetailLineRepository>().InRequestScope();
            kernel.Bind<IAccountRepository>().To<AccountRepository>().InRequestScope();
            kernel.Bind<IManageRepository>().To<ManageRepository>().InRequestScope();

            kernel.Bind<ICustomerBusinessLogic<CustomerViewModel>>().To<CustomerBusinessLogic>().InRequestScope();
            kernel.Bind<IProductBusinessLogic<ProductViewModel>>().To<ProductBusinessLogic>().InRequestScope();
            kernel.Bind<IInvoiceBusinessLogic<InvoiceViewModel>>().To<InvoiceBusinessLogic>().InRequestScope();
            kernel.Bind<IDetailLineBusinessLogic<DetailLineViewModel>>().To<DetailLineBusinessLogic>().InRequestScope();
            kernel.Bind<IAccountBusinessLogic>().To<AccountBusinessLogic>().InRequestScope();
            kernel.Bind<IManageBusinessLogic>().To<ManageBusinessLogic>().InRequestScope();

            kernel.Bind<MainDbContext>().ToSelf().InRequestScope();
        }

    }
}

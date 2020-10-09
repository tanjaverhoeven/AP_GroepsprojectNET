using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using DotnetAcademy.BLL;
using DotnetAcademy.BLL.Identity;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL;
using DotnetAcademy.DAL.Models;
using DotnetAcademy.DAL.Repositories;
using DotnetAcademy.DAL.Repositories.Identity;
using Ninject;
using Ninject.Extensions.ChildKernel;
using Ninject.Syntax;
using Ninject.Web.Common;

namespace DotnetAcademy.API
{
    public class NinjectDependency : IDependencyScope
    {
        IResolutionRoot resolver;

        public NinjectDependency(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }
        public void Dispose()
        {
            IDisposable disposable = resolver as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            resolver = null;
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
            {
                throw new ObjectDisposedException("this", "scope is disposed");
            }
            return resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
            {
                throw new ObjectDisposedException("this", "scope is disposed");
            }
            return resolver.GetAll(serviceType);
        }
    }

    public class NinjectResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectResolver() : this(new StandardKernel())
        {
        }

        public NinjectResolver(IKernel ninjectKernel, bool scope = false)
        {
            kernel = ninjectKernel;
            if (!scope)
            {
                AddBindings(kernel);
            }
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectResolver(AddRequestBindings(new ChildKernel(kernel)), true);
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public void Dispose()
        {

        }

        private void AddBindings(IKernel kernel)
        {
            // singleton and transient bindings go here
        }

        private IKernel AddRequestBindings(IKernel kernel) {

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

            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<MainDbContext>().ToSelf().InRequestScope();

            return kernel;
        }

    }
}
namespace SportsStore.WebUI.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Moq;
    using Ninject;
    using SportsStore.Domain.Abstract;
    using SportsStore.Domain.Concrete;
    using SportsStore.Domain.Entities;

    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        private void AddBindings()
        {
            /*Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>{
                new Product {Name = "Piłka nożna", Price = 25},
                new Product {Name = "Deska surfingowa", Price = 180},
                new Product {Name = "But do biegania", Price = 95},
            });
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);*/
            kernel.Bind<IProductRepository>().To<EFProductRepository>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}
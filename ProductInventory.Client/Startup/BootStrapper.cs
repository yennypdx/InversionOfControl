using Autofac;
using ProductInventory.DataAccess;

namespace ProductInventory.Client.Startup
{
    public class BootStrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();

            return builder.Build();
        }
    }
}

using System.Collections.Generic;

namespace ProductInventory.DataAccess
{
    public interface IProductRepository
    {
        IList<Product> FindAll();
        Product FindByName(string productName);
        Product FindById(int productId);
        bool Save(Product targetProduct);
    }
}

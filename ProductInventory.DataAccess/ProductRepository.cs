using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProductInventory.DataAccess
{
    public class ProductRepository : IProductRepository
    {
        private const string StorageFile = "Database.txt";
 
        public IList<Product> FindAll()
        {
            var products = new List<Product>();
            var lines = File.ReadAllLines(StorageFile);

            foreach(var line in lines)
            {
                var productInfo = line.Split(';');
                var product = new Product
                {
                    Id = Convert.ToInt32(productInfo[0]),
                    Name = productInfo[1],
                    Price = Convert.ToDecimal(productInfo[2])
                };

                products.Add(product);
            }
            return products;
        }

        public Product FindById(int productId)
        {
            return FindAll().SingleOrDefault(product => product.Id.Equals(productId));
        }

        public Product FindByName(string productName)
        {
            return FindAll().SingleOrDefault(product => product.Name.Equals(productName));
        }

        public bool Save(Product targetProduct)
        {
            var products = FindAll();
            var productToUpdate = products.SingleOrDefault(product 
                => product.Id.Equals(targetProduct.Id));

            if (productToUpdate == null) return false;
            productToUpdate.Id = targetProduct.Id;
            productToUpdate.Name = targetProduct.Name;
            productToUpdate.Price = targetProduct.Price;

            var lines = new List<string>();
            products.ToList().ForEach(product => lines.Add(product.ToString()));

            File.WriteAllLines(StorageFile, lines);
            return true;
        }
    }
}

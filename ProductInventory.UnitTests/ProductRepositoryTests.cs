using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductInventory.DataAccess.Tests
{
    [TestClass()]
    public class ProductRepositoryTests
    {
        private Mock<IProductRepository> _dataRepo;
        private IList<Product> _productDb;

        [TestInitialize]
        public void Init()
        {
            _productDb = new List<Product>()
            {
                new Product{Id=1, Name="Dawn Dish Soap", Price=3.49m},
                new Product{Id=2, Name="Tostino's Pizza Rolls", Price=4.39m},
                new Product{Id=3, Name="Purina Dog Chow", Price=19.99m},
                new Product{Id=4, Name="Meow Cat Food", Price=9.99m},
                new Product{Id=5, Name="Jiffy Peanut Butter", Price=3.29m}
            };

            _dataRepo = new Mock<IProductRepository>();

            _dataRepo.Setup(m => m.FindAll()).Returns(_productDb);

            _dataRepo.Setup(m => m.FindById(It.IsAny<int>())).Returns(
              (int id) =>
              {
                  return _productDb.SingleOrDefault(p => p.Id.Equals(id));
              });

            _dataRepo.Setup(m => m.FindByName(It.IsAny<string>())).Returns(
                (string name) =>
                {
                    return _productDb.SingleOrDefault(p => p.Name.Equals(name));
                });

            _dataRepo.Setup(m => m.Save(It.IsAny<Product>())).Callback(
                (Product prod) =>
                {
                    if (prod == null) throw new InvalidOperationException("SaveProduct: null input.");
                    if(prod.Id <= 0)
                    {
                        prod.Id = _productDb.Max(p => p.Id) + 1;
                        _productDb.Add(prod);
                    }
                    else
                    {
                        var newProductToSave = prod;
                        if (newProductToSave == null)
                            throw new InvalidOperationException($"SaveProduct: Unable to find Product with ID: {prod.Id}");

                        _productDb.Add(newProductToSave);
                       
                    }
                });
        }

        [TestMethod]
        public void FindAll_ShouldReturnFiveProducts()
        {
            var product = _dataRepo.Object.FindAll();
            var count = product.Count();

            Assert.AreEqual(5, count);
        }

        [TestMethod]
        public void FindByIdTest_ShouldNotFindProductWithIdZero()
        {
            var product = _dataRepo.Object.FindById(0);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void FindByIdTest_ShouldFindProductWithIdOne()
        {
            var product = _dataRepo.Object.FindById(1);

            Assert.AreEqual("Dawn Dish Soap", product.Name);
        }

        [TestMethod]
        public void FindByIdTest_ShouldFindProductWithIdFive()
        {
            var product = _dataRepo.Object.FindById(5);

            Assert.AreEqual("Jiffy Peanut Butter", product.Name);
        }

        [TestMethod]
        public void FindByIdTest_ShouldNotFindProductWithIdTen()
        {
            var product = _dataRepo.Object.FindById(10);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void FindByIdTest_ShouldFindProductWithIdTwo()
        {
            var product = _dataRepo.Object.FindById(2);

            Assert.AreEqual("Tostino's Pizza Rolls", product.Name);
        }

        [TestMethod]
        public void FindByNameTest_ShouldFindProductIdWithNameInput()
        {
            var product = _dataRepo.Object.FindByName("Jiffy Peanut Butter");

            Assert.AreEqual(5, product.Id);
        }

        [TestMethod]
        public void FindByNameTest_ShouldNotFindUnlistedName()
        {
            var product = _dataRepo.Object.FindByName("Chapstick Lipbalm");

            Assert.IsNull(product);
        }

        [TestMethod]
        public void SaveTest_AddANewProduct()
        {
            var newProduct = new Product { Id = 7, Name = "Hawaiian Drinking Water", Price = 3.29m };
            _dataRepo.Object.Save(newProduct);

            Assert.AreEqual(6, _dataRepo.Object.FindAll().Count());
        }
    }
}
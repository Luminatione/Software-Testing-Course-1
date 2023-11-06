using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using ConsoleApp1.DataBase;
using Moq;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;
using Database;
using ConsoleApp1.Controllers;

namespace ConsoleApp1.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void CreateProduct_ValiDdata_AddsProduct ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var productController = new ProductController (detabaseServiecve);

            var productId = 1;
            var productName = "Test Product";
            var productPrice = 4.0m;
            var productAmount = 10;

            // Act
            productController.CreateProduct (productId, productName, productPrice, productAmount);

            // Assert
            Assert.Equal<Product> (
                new Product (productId, productName, productPrice, productAmount),
                detabaseServiecve.GetProductByIdOrNull (productId)
            );
        }

        [Fact]
        public void CreateProduct_ValiDdata_AddsProductThatExists ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var productController = new ProductController (detabaseServiecve);

            var productId = 1;
            var productName = "Test Product";
            var productPrice = 4.0m;
            var productAmount = 10;

            // Act
            productController.CreateProduct (productId, productName, productPrice, productAmount);
            Product sameProduct = new Product (productId, productName, productPrice, productAmount);

            // Assert
            Assert.Throws<ProductAlreadyExistsException> (() => detabaseServiecve.AddProduct (sameProduct));
        }

        [Fact]
        public void CreateProduct_InvalidName_ThrowsDbUpdateException ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var productController = new ProductController (detabaseServiecve);

            var productId = 1;
            string productName = null;
            decimal productPrice = 4.0m;
            int productAmount = 10;

            // Act and Assert
            Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException> (
                () => productController.CreateProduct (productId, productName, productPrice, productAmount)
            );
        }

        [Fact]
        public void CreateProduct_InvalidPrice_CreateProduct ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var productController = new ProductController (detabaseServiecve);

            var productId = 1;
            string productName = "Test Product";
            int productPrice = 4;
            int productAmount = 10;

            productController.CreateProduct (productId, productName, productPrice, productAmount);

            // Act and Assert
            Assert.Equal<Product> (
                new Product (productId, productName, productPrice, productAmount),
                detabaseServiecve.GetProductByIdOrNull (productId)
            );
        }

        [Fact]
        public void ReadProduct_ExistingData_GetsAllProducts ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var productController = new ProductController (detabaseServiecve);

            var productId = 1;
            string productName = "Test Product";
            int productPrice = 4;
            int productAmount = 10;

            productController.CreateProduct (productId, productName, productPrice, productAmount);
            List<Product> clientList = new List<Product> ()
            {
                new Product(productId, productName, productPrice, productAmount)
            };

            // Act and Assert
            Assert.Equal<List<Product>> (
                Enumerable.Repeat (clientList, 1),
                Enumerable.Repeat (productController.GetAllProducts (), 1)
            );
        }

        [Fact]
        public void ReadProduct_ExistingData_GetsById ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var productController = new ProductController (detabaseServiecve);

            var productId = 1;
            string productName = "Test Product";
            int productPrice = 4;
            int productAmount = 10;

            productController.CreateProduct (productId, productName, productPrice, productAmount);

            // Act and Assert
            Assert.Equal<Product> (
                new Product (productId, productName, productPrice, productAmount),
                detabaseServiecve.GetProductByIdOrNull (productId)
            );
        }

        [Fact]
        public void ReadProduct_NoData_GetsAllClients ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var productController = new ProductController (detabaseServiecve);

            List<Product> productList = new List<Product> ();

            // Act and Assert
            Assert.Equal<List<Product>> (
                Enumerable.Repeat (productList, 1),
                Enumerable.Repeat (productController.GetAllProducts (), 1)
            );
        }

        [Fact]
        public void ReadProduct_NoData_GetsById ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var productController = new ProductController (detabaseServiecve);

            // Act and Assert
            Assert.Null (detabaseServiecve.GetProductByIdOrNull (2));
        }

        [Fact]
        public void UpdateProduct_ValidData_UpdatesProduct ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var productController = new ProductController (detabaseServiecve);

            var productId = 1;
            string productName = "Test Product";
            int productPrice = 4;
            int productAmount = 10;

            productController.CreateProduct (productId, productName, productPrice, productAmount);

            // Act
            var newName = "New Test Product";
            productController.UpdateProduct (productId, newName, productPrice, productAmount);

            // Assert
            Assert.Equal<Product> (
                new Product (productId, newName, productPrice, productAmount),
                detabaseServiecve.GetProductByIdOrNull (productId)
            );
        }

        [Fact]
        public void UpdateConttroler_InvalidDataId_ThrowsNullReferenceException ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var productController = new ProductController (databaseService);

            var productId = 1;
            string productName = "Test Product";
            int productPrice = 4;
            int productAmount = 10;

            productController.CreateProduct (productId, productName, productPrice, productAmount);

            // Act and Assert
            Assert.Throws<NullReferenceException> (
                () =>
                    productController.UpdateProduct (
                        99,
                        productName,
                        productPrice,
                        productAmount
                    )
            );
        }

        [Fact]
        public void UpdateProduct_InvalidDataName_ThrowsArgumentException ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var productController = new ProductController (databaseService);

            var productId = 1;
            string productName = "Test Product";
            int productPrice = 4;
            int productAmount = 10;

            productController.CreateProduct (productId, productName, productPrice, productAmount);

            // Act and Assert
            Assert.Throws<ArgumentException> (
                () =>
                    productController.UpdateProduct (
                        99,
                        null,
                        productPrice,
                        productAmount
                    )
            );
        }

        [Fact]
        public void DelateProduct_ValidClient_DelateProduct ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var productController = new ProductController (databaseService);

            var productId = 1;
            string productName = "Test Product";
            int productPrice = 4;
            int productAmount = 10;

            productController.CreateProduct (productId, productName, productPrice, productAmount);

            // Act:
            productController.DeleteProduct (productId);

            // Assert:
            var removedOrder = databaseService.GetProductByIdOrNull (productId);
            Assert.Null (removedOrder);
        }

        [Fact]
        public void DelateProduct_InvalidData_ThrowsProductNotFoundException ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var productController = new ProductController (databaseService);

            // Act and Assert
            Assert.Throws<ProductNotFoundException> (() => productController.DeleteProduct (1));
        }
    }
}

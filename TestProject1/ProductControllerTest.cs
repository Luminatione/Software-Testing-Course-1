using ConsoleApp1.Controllers;
using ConsoleApp1.DataBase;
using ConsoleApp1.Model;
using Moq;

namespace ConsoleApp1.Controllers.Tests
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public void CreateProduct_AddsProductToContext ()
        {
            // Arrange
            int productId = 1;
            string name = "Product 1";
            decimal price = 10.0m;
            int storedAmount = 100;

            var mockContext = new Mock<IDatabaseService> ();
            var productController = new ProductController (mockContext.Object);

            // Act
            productController.CreateProduct (productId, name, price, storedAmount);

            // Assert
            mockContext.Verify (context => context.AddProduct (It.IsAny<Product> ()), Times.Once);
        }

        [TestMethod]
        public void GetProductById_ReturnsProductOrNull ()
        {
            // Arrange
            int productId = 1;
            var expectedProduct = new Product (productId, "Product 1", 10.0m, 100);

            var mockContext = new Mock<IDatabaseService> ();
            mockContext.Setup (context => context.GetProductByIdOrNull (productId)).Returns (expectedProduct);
            var productController = new ProductController (mockContext.Object);

            // Act
            var result = productController.GetProductById (productId);

            // Assert
            Assert.AreEqual (expectedProduct, result);
        }

        [TestMethod]
        public void GetAllProducts_ReturnsAllProducts ()
        {
            // Arrange
            var expectedProducts = new List<Product>
        {
            new Product(1, "Product 1", 10.0m, 100),
            new Product(2, "Product 2", 20.0m, 200)
        };

            var mockContext = new Mock<IDatabaseService> ();
            mockContext.Setup (context => context.GetAllProducts ()).Returns (expectedProducts);
            var productController = new ProductController (mockContext.Object);

            // Act
            var result = productController.GetAllProducts ();

            // Assert
            CollectionAssert.AreEqual (expectedProducts, result);
        }

        [TestMethod]
        public void UpdateProduct_UpdatesExistingProduct ()
        {
            // Arrange
            int productId = 1;
            string updatedName = "Updated Product";
            decimal updatedPrice = 15.0m;
            int updatedStoredAmount = 150;

            var existingProduct = new Product (productId, "Product 1", 10.0m, 100);

            var mockContext = new Mock<IDatabaseService> ();
            mockContext.Setup (context => context.GetProductByIdOrNull (productId)).Returns (existingProduct);
            var productController = new ProductController (mockContext.Object);

            // Act
            productController.UpdateProduct (productId, updatedName, updatedPrice, updatedStoredAmount);

            // Assert
            Assert.AreEqual (updatedName, existingProduct.Name);
            Assert.AreEqual (updatedPrice, existingProduct.Price);
            Assert.AreEqual (updatedStoredAmount, existingProduct.StoredAmount);
            mockContext.Verify (context => context.UpdateProduct (existingProduct), Times.Once);
        }

        [TestMethod]
        public void UpdateProduct_ThrowsArgumentExceptionForNonexistentProduct ()
        {
            // Arrange
            int productId = 1;

            var mockContext = new Mock<IDatabaseService> ();
            var productController = new ProductController (mockContext.Object);

            // Act and Assert
            Assert.ThrowsException<ArgumentException> (() => productController.UpdateProduct (productId, "Updated Product", 15.0m, 150));
        }

        [TestMethod]
        public void DeleteProduct_RemovesProductFromContext ()
        {
            // Arrange
            int productId = 1;

            var mockContext = new Mock<IDatabaseService> ();
            var productController = new ProductController (mockContext.Object);

            // Act
            productController.DeleteProduct (productId);

            // Assert
            mockContext.Verify (context => context.RemoveProduct (productId), Times.Once);
        }
    }
}

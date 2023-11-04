using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using ConsoleApp1.DataBase;
using Moq;

namespace ConsoleApp1.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void CreateOrder_ValidClientAndProducts_AddsOrder ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();
            var detabaseServiecve = new Mock<IDatabaseService> ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve.Object);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };
            var storedProduct = new Product (productId, "", 0, 10);


            clientRepositoryMock.Setup (repo => repo.GetClientById (clientId)).Returns (new Client (clientId, "", "", ""));
            productRepositoryMock.Setup (repo => repo.GetAllProducts ()).Returns (new List<Product> { storedProduct });
            productRepositoryMock.Setup (repo => repo.GetProductById (productId)).Returns (storedProduct);
            orderRepositoryMock.Setup (repo => repo.GetAllOrders ()).Returns (new List<Order> { new Order (0, clientId,
                                                                              new List<Product> (), Order.OrderStatus.Completed) });

            // Act
            orderController.CreateOrder (clientId, productList);

            // Assert
            detabaseServiecve.Verify (db => db.AddOrder (It.IsAny<Order> ()), Times.Once);
        }

        [Fact]
        public void CreateOrder_InvalidClient_ThrowsArgumentException ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();
            var detabaseServiecve = new Mock<IDatabaseService> ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve.Object);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            clientRepositoryMock.Setup (repo => repo.GetClientById (clientId)).Returns ((Client)null);
            productRepositoryMock.Setup (repo => repo.GetAllProducts ()).Returns (new List<Product> ());

            // Act and Assert
            Assert.Throws<ArgumentException> (() => orderController.CreateOrder (clientId, productList));
        }

        [Fact]
        public void CreateOrder_InvalidProduct_ThrowsArgumentException ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();
            var detabaseServiecve = new Mock<IDatabaseService> ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve.Object);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };
            var product = new Product (productId + 1, "", 0, 10);

            clientRepositoryMock.Setup (repo => repo.GetClientById (clientId)).Returns (new Client (clientId, "", "", ""));
            productRepositoryMock.Setup (repo => repo.GetAllProducts ()).Returns (productList);
            productRepositoryMock.Setup (repo => repo.GetProductById (productId)).Returns ((Product)null);
            orderRepositoryMock.Setup (repo => repo.GetAllOrders ()).Returns (new List<Order> ());

            // Act and Assert
            Assert.Throws<ArgumentException> (() => orderController.CreateOrder (clientId, new List<Product> { product }));
        }
    }
}

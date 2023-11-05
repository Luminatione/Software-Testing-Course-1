using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using ConsoleApp1.DataBase;
using Moq;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

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

            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act
            orderController.CreateOrder (clientId, productList);

            // Assert
            Assert.Equal<Order> (new Order (2, clientId, productList, Order.OrderStatus.New),
                                 detabaseServiecve.GetOrderByIdOrNull (2));
        }

        [Fact]
        public void CreateOrder_ValidClientAndProducts_AddsOrderThatExists ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();

            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act
            orderController.CreateOrder (clientId, productList);
            Order sameOrder = new Order (2, clientId, productList, Order.OrderStatus.New);

            // Assert
            Assert.Throws<OrderAlreadyExistsException> (() => detabaseServiecve.AddOrder (sameOrder));
        }

        [Fact]
        public void CreateOrder_InvalidClient_ThrowsArgumentException ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();

            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act and Assert
            Assert.Throws<ArgumentException> (() => orderController.CreateOrder (clientId + 1, productList));
        }

        [Fact]
        public void CreateOrder_InvalidProduct_ThrowsArgumentException ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();

            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };
            var product = new Product (productId + 1, "", 0, 10);

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act and Assert
            Assert.Throws<ArgumentException> (() => orderController.CreateOrder (clientId, new List<Product> { product }));
        }

        [Fact]
        public void CreateOrder_NullProduct_ThrowsArgumentNullException ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();

            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };
            var product = new Product (productId + 1, "", 0, 10);

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act and Assert
            Assert.Throws<ArgumentNullException> (() => orderController.CreateOrder (clientId, null));
        }

        [Fact]
        public void CreateOrder_NegativeClientId_ThrowsArgumentException ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();

            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };
            var product = new Product (productId + 1, "", 0, 10);

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act and Assert
            Assert.Throws<ArgumentException> (() => orderController.CreateOrder (-1, productList));
        }

        [Fact]
        public void ReadOrder_ExistingData_GetsAllOrder ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();

            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            orderController.CreateOrder (clientId, productList);
            var orderList = new List<Order> () { new Order (2, clientId, productList, Order.OrderStatus.New) };

            // Act and Assert
            Assert.Equal<List<Order>> (Enumerable.Repeat (orderList, 1),
                                       Enumerable.Repeat (detabaseServiecve.GetAllOrdersAsync ().Result, 1));
        }

        [Fact]
        public void ReadOrder_ExistingData_GetsById ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();

            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            detabaseServiecve.AddOrder (new Order (2, clientId, productList, Order.OrderStatus.New));

            // Act and Assert
            Assert.Equal<Order> (new Order (2, clientId, productList, Order.OrderStatus.New),
                                 detabaseServiecve.GetOrderByIdOrNull (2));
        }

        [Fact]
        public void ReadOrder_NoData_GetsAllOrder ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();

            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            var orderList = new List<Order> ();

            // Act and Assert
            Assert.Equal<List<Order>> (Enumerable.Repeat (orderList, 1),
                                       Enumerable.Repeat (detabaseServiecve.GetAllOrdersAsync ().Result, 1));
        }

        [Fact]
        public void ReadOrder_NotExistingData_GetsById ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();

            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act and Assert
            Assert.Null (detabaseServiecve.GetOrderByIdOrNull (2));
        }
    }
}

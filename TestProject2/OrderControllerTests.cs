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

        [Fact]
        public void UpdateOrder_ValidData_UpdatesOrder ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();
            var databaseServiceMock = new Mock<IDatabaseService> ();

            var orderController = new OrderController (
                orderRepositoryMock.Object,
                clientRepositoryMock.Object,
                productRepositoryMock.Object,
                databaseServiceMock.Object
            );

            var orderId = 1;
            var customerId = 1;
            var product = new Product (1, "", 4, 5);
            var productList = new List<Product> { product };
            var status = Order.OrderStatus.Completed;
            var existingOrder = new Order (
                orderId,
                customerId,
                new List<Product> (),
                Order.OrderStatus.New
            );

            databaseServiceMock
                .Setup (db => db.GetClientByIdOrNull (customerId))
                .Returns (new Client (customerId, "", "", ""));
            databaseServiceMock.Setup (db => db.GetAllProductsAsync ().Result).Returns (productList);
            databaseServiceMock.Setup (db => db.GetOrderByIdOrNull (orderId)).Returns (existingOrder);

            databaseServiceMock.Setup (db => db.GetProductByIdOrNull (product.Id)).Returns (product);

            // Act
            orderController.UpdateOrder (orderId, customerId, productList, status);

            // Assert
            orderRepositoryMock.Verify (repo => repo.UpdateOrder (It.IsAny<Order> ()), Times.Once);
        }

        [Fact]
        public void UpdateOrder_InvalidData_ThrowsArgumentException ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();
            var databaseServiceMock = new Mock<IDatabaseService> ();

            var orderController = new OrderController (
                orderRepositoryMock.Object,
                clientRepositoryMock.Object,
                productRepositoryMock.Object,
                databaseServiceMock.Object
            );

            var orderId = 1;
            var customerId = 1;
            var productList = new List<Product> { new Product (1, "", 4, 5) };
            var status = Order.OrderStatus.Completed;
            var existingOrder = new Order (
                orderId,
                customerId,
                new List<Product> (),
                Order.OrderStatus.New
            );

            databaseServiceMock
                .Setup (db => db.GetClientByIdOrNull (customerId))
                .Returns ((Client)null);
            databaseServiceMock.Setup (db => db.GetAllProductsAsync ().Result).Returns (productList);
            databaseServiceMock.Setup (db => db.GetOrderByIdOrNull (orderId)).Returns (existingOrder);

            Assert.Throws<ArgumentException> (
                () => orderController.UpdateOrder (orderId, customerId, productList, status)
            );
        }

        [Fact]
        public void CancelOrder_ValidOrder_CancelsOrder ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();
            var databaseServiceMock = new Mock<IDatabaseService> ();

            var orderController = new OrderController (
                orderRepositoryMock.Object,
                clientRepositoryMock.Object,
                productRepositoryMock.Object,
                databaseServiceMock.Object
            );

            var orderId = 1;
            var customerId = 1;
            var product = new Product (1, "", 4, 5);
            var productList = new List<Product> { product };
            var status = Order.OrderStatus.Completed;
            var existingOrder = new Order (
                orderId,
                customerId,
                new List<Product> (),
                Order.OrderStatus.New
            );

            databaseServiceMock.Setup (db => db.GetOrderByIdOrNull (orderId)).Returns (existingOrder);

            databaseServiceMock.Setup (db => db.GetProductByIdOrNull (product.Id)).Returns (product);

            // Act
            orderController.CancelOrder (orderId);

            // Assert
            databaseServiceMock.Verify (service => service.RemoveOrder (orderId), Times.Once);
        }

        [Fact]
        public void CancelOrder_NonExistentOrder_ThrowsArgumentException ()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository> ();
            var clientRepositoryMock = new Mock<IClientRepository> ();
            var productRepositoryMock = new Mock<IProductRepository> ();
            var databaseServiceMock = new Mock<IDatabaseService> ();

            var orderController = new OrderController (
                orderRepositoryMock.Object,
                clientRepositoryMock.Object,
                productRepositoryMock.Object,
                databaseServiceMock.Object
            );

            var orderId = 1;

            databaseServiceMock.Setup (db => db.GetOrderByIdOrNull (orderId)).Returns ((Order)null);

            Assert.Throws<ArgumentException> (() => orderController.CancelOrder (orderId));
        }
    }
}

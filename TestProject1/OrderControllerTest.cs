using Castle.Core.Resource;
using ConsoleApp1.DataBase;
using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using Moq;

namespace ConsoleApp1.Tests
{
	[TestClass]
	public class OrderControllerTests
	{
		[TestMethod]
		public void CreateOrder_ValidClientAndProducts_AddsOrder()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();
            var detabaseServiecve = new Mock<IDatabaseService> ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve.Object);

            var clientId = 1;
			var productId = 1;
			var productList = new List<Product> { new Product(productId, "", 4, 5) };
			var storedProduct = new Product(productId, "", 0, 10);

            detabaseServiecve.Setup (db => db.GetClientByIdOrNull (clientId)).Returns (new Client (clientId, "", "", ""));
            detabaseServiecve.Setup (db => db.GetAllProductsAsync ()).Returns (Task.FromResult (productList));
            detabaseServiecve.Setup (db => db.GetProductByIdOrNull (productId)).Returns (storedProduct);

            // Act
            orderController.CreateOrder(clientId, productList);

            // Assert
            detabaseServiecve.Verify (db => db.AddOrder(It.IsAny<Order>()), Times.Once);
		}

		[TestMethod]
		public void CancelOrder_ExistingOrder_DeletesOrder()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();
            var detabaseServiecve = new Mock<IDatabaseService> ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve.Object);

            var orderId = 1;

            detabaseServiecve.Setup (db => db.GetOrderByIdOrNull (orderId))
                .Returns (new Order (orderId, 0, new List<Product> (), Order.OrderStatus.New));

            // Act
            orderController.CancelOrder(orderId);

            // Assert
            detabaseServiecve.Verify(db => db.RemoveOrder (orderId), Times.Once);
		}

		[TestMethod]
		public void UpdateOrder_ValidData_UpdatesOrder()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();
            var detabaseServiecve = new Mock<IDatabaseService> ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve.Object);

            var orderId = 1;
			var customerId = 2;
			var productId = 1;
			var productList = new List<Product> { new Product(1, "", 4, 5) };
			var status = Order.OrderStatus.InProgress;
			var storedProduct = new Product(1, "", 4, 10);

            detabaseServiecve.Setup (db => db.GetClientByIdOrNull (customerId)).Returns (new Client (customerId, "", "", ""));
            detabaseServiecve.Setup (db => db.GetAllProductsAsync ()).Returns (Task.FromResult (new List<Product> { storedProduct }));
            detabaseServiecve.Setup (db => db.GetProductByIdOrNull (productId)).Returns (storedProduct);
            detabaseServiecve.Setup (db => db.GetOrderByIdOrNull (orderId))
                .Returns (new Order (orderId, customerId, productList, Order.OrderStatus.New));

            // Act
            orderController.UpdateOrder(orderId, customerId, productList, status);

            // Assert
            detabaseServiecve.Verify(db => db.UpdateOrder (It.IsAny<Order>()), Times.Once);
		}

		[TestMethod]
		public void CreateOrder_InvalidClient_ThrowsArgumentException()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();
            var detabaseServiecve = new Mock<IDatabaseService> ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve.Object);

            var clientId = 1;
			var productId = 1;
			var productList = new List<Product> { new Product(productId, "", 4, 5) };

			detabaseServiecve.Setup(db => db.GetClientByIdOrNull(clientId)).Returns((Client)null);
			detabaseServiecve.Setup (db => db.GetAllProductsAsync ()).Returns(Task.FromResult(new List<Product> ()));

			// Act and Assert
			Assert.ThrowsException<ArgumentException>(() => orderController.CreateOrder(clientId, productList));
		}

		[TestMethod]
		public void CreateOrder_InvalidProduct_ThrowsArgumentException()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();
            var detabaseServiecve = new Mock<IDatabaseService> ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve.Object);

            var clientId = 1;
			var productId = 1;
			var productList = new List<Product> { new Product(productId, "", 4, 5) };
			var product = new Product(productId + 1, "", 0, 10);

            detabaseServiecve.Setup (db => db.GetClientByIdOrNull (clientId)).Returns (new Client (clientId, "", "", ""));
            detabaseServiecve.Setup (db => db.GetAllProductsAsync ()).Returns (Task.FromResult (productList));
            detabaseServiecve.Setup (db => db.GetProductByIdOrNull (productId)).Returns ((Product)null);
			detabaseServiecve.Setup (db => db.GetAllOrdersAsync ()).Returns (Task.FromResult (new List<Order> ()));

            // Act and Assert
            Assert.ThrowsException<ArgumentException>(() => orderController.CreateOrder(clientId, new List<Product> { product }));
		}

		[TestMethod]
		public void UpdateOrder_InvalidClient_NullReferenceException ()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();
            var detabaseServiecve = new Mock<IDatabaseService> ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve.Object);

            var orderId = 1;
			var customerId = 2;
			var productId = 1;
			var productList = new List<Product> { new Product(1, "", 4, 5) };
			var status = Order.OrderStatus.InProgress;
			var storedProduct = new Product(1, "", 4, 10);

            detabaseServiecve.Setup (db => db.GetClientByIdOrNull (customerId)).Returns (new Client (customerId, "", "", ""));
            detabaseServiecve.Setup (db => db.GetAllProductsAsync ()).Returns (Task.FromResult (productList));
            detabaseServiecve.Setup (db => db.GetProductByIdOrNull (productId)).Returns ((Product)null);
            detabaseServiecve.Setup (db => db.GetOrderByIdOrNull (orderId))
                .Returns (new Order (orderId, customerId, productList, Order.OrderStatus.New));

            // Act and Assert
            Assert.ThrowsException<NullReferenceException> (() => orderController.UpdateOrder(orderId, customerId, productList, status));
		}

		[TestMethod]
		public void UpdateOrder_InvalidProduct_ThrowsArgumentException()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();
            var detabaseServiecve = new Mock<IDatabaseService> ();

            var orderController = new OrderController (orderRepositoryMock.Object, clientRepositoryMock.Object,
                                                        productRepositoryMock.Object, detabaseServiecve.Object);

            var orderId = 1;
			var customerId = 2;
			var productId = 1;
			var storedProduct = new Product(1, "", 4, 10);
			var productList = new List<Product> { storedProduct };
			var status = Order.OrderStatus.InProgress;
			var newProduct = new Product(storedProduct.Id++, "", 3, 3);

            detabaseServiecve.Setup (db => db.GetClientByIdOrNull (customerId)).Returns (new Client (customerId, "", "", ""));
            detabaseServiecve.Setup (db => db.GetAllProductsAsync ()).Returns (Task.FromResult (productList));
            detabaseServiecve.Setup (db => db.GetProductByIdOrNull (productId)).Returns ((Product)null);
            detabaseServiecve.Setup (db => db.GetOrderByIdOrNull (orderId))
                .Returns (new Order (orderId, customerId, productList, Order.OrderStatus.New));

            // Act and Assert
            Assert.ThrowsException<ArgumentException>(() => orderController.UpdateOrder(orderId, customerId, new List<Product> { newProduct }, status));
		}
	}
}
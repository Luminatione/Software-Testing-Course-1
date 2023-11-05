using ConsoleApp1.Controllers;
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
			var dbServiceMock = new Mock<IDatabaseService>();

			var orderController = new OrderController(dbServiceMock.Object);

			var clientId = 1;
			var productId = 1;
			var productList = new List<Product> { new Product(productId, "", 4, 5) };
			var storedProduct = new Product(productId, "", 0, 10);

			dbServiceMock.Setup(repo => repo.GetClientByIdOrNull(clientId)).Returns(new Client(clientId, "", "", ""));
			dbServiceMock.Setup(repo => repo.GetAllProducts()).Returns(new List<Product> { storedProduct });
			dbServiceMock.Setup(repo => repo.GetProductByIdOrNull(productId)).Returns(storedProduct);
			dbServiceMock.Setup(repo => repo.GetAllOrders()).Returns(new List<Order> { new Order(0, clientId, new List<Product>(), Order.OrderStatus.Completed) });

			// Act
			orderController.CreateOrder(clientId, productList);

			// Assert
			dbServiceMock.Verify(repo => repo.AddOrder(It.IsAny<Order>()), Times.Once);
		}

		[TestMethod]
		public void CancelOrder_ExistingOrder_DeletesOrder()
		{
			// Arrange
			var dbServiceMock = new Mock<IDatabaseService>();

			var orderController = new OrderController(dbServiceMock.Object);


			var orderId = 1;

			dbServiceMock.Setup(repo => repo.GetOrderByIdOrNull(orderId)).Returns(new Order(0, 0, new List<Product>(), Order.OrderStatus.Completed));

			// Act
			orderController.CancelOrder(orderId);

			// Assert
			dbServiceMock.Verify(repo => repo.RemoveOrder(orderId), Times.Once);
		}

		[TestMethod]
		public void UpdateOrder_ValidData_UpdatesOrder()
		{
			// Arrange
			var dbServiceMock = new Mock<IDatabaseService>();

			var orderController = new OrderController(dbServiceMock.Object);

			var orderId = 1;
			var customerId = 2;
			var productId = 1;
			var productList = new List<Product> { new Product(1, "", 4, 5) };
			var status = Order.OrderStatus.InProgress;
			var storedProduct = new Product(1, "", 4, 10);

			dbServiceMock.Setup(repo => repo.GetClientByIdOrNull(customerId)).Returns(new Client(customerId, "", "", ""));
			dbServiceMock.Setup(repo => repo.GetAllProducts()).Returns(new List<Product> { storedProduct });
			dbServiceMock.Setup(repo => repo.GetProductByIdOrNull(productId)).Returns(storedProduct);
			dbServiceMock.Setup(repo => repo.GetOrderByIdOrNull(orderId)).Returns(new Order(orderId, customerId, productList, Order.OrderStatus.New));

			// Act
			orderController.UpdateOrder(orderId, customerId, productList, status);

			// Assert
			dbServiceMock.Verify(repo => repo.UpdateOrder(It.IsAny<Order>()), Times.Once);
		}

		[TestMethod]
		public void CreateOrder_InvalidClient_ThrowsArgumentException()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();
			var dbServiceMock = new Mock<IDatabaseService>();
			
			var orderController = new OrderController(dbServiceMock.Object);

			var clientId = 1;
			var productId = 1;
			var productList = new List<Product> { new Product(productId, "", 4, 5) };

			dbServiceMock.Setup(repo => repo.GetClientByIdOrNull(clientId)).Returns((Client)null);
			dbServiceMock.Setup(repo => repo.GetAllProducts()).Returns(new List<Product>());

			// Act and Assert
			Assert.ThrowsException<ArgumentException>(() => orderController.CreateOrder(clientId, productList));
		}

		[TestMethod]
		public void CreateOrder_InvalidProduct_ThrowsArgumentException()
		{
			// Arrange
			var dbServiceMock = new Mock<IDatabaseService>();

			var orderController = new OrderController(dbServiceMock.Object);

			var clientId = 1;
			var productId = 1;
			var productList = new List<Product> { new Product(productId, "", 4, 5) };
			var product = new Product(productId + 1, "", 0, 10);

			dbServiceMock.Setup(repo => repo.GetClientByIdOrNull(clientId)).Returns(new Client(clientId, "", "", ""));
			dbServiceMock.Setup(repo => repo.GetAllProducts()).Returns(new List<Product> { product });
			dbServiceMock.Setup(repo => repo.GetProductByIdOrNull(productId)).Returns((Product)null);
			dbServiceMock.Setup(repo => repo.GetAllOrders()).Returns(new List<Order>());

			// Act and Assert
			Assert.ThrowsException<ArgumentException>(() => orderController.CreateOrder(clientId, new List<Product> { product }));
		}

		[TestMethod]
		public void UpdateOrder_InvalidClient_ThrowsArgumentException()
		{
			// Arrange
			var dbServiceMock = new Mock<IDatabaseService>();

			var orderController = new OrderController(dbServiceMock.Object);

			var orderId = 1;
			var customerId = 2;
			var productId = 1;
			var productList = new List<Product> { new Product(1, "", 4, 5) };
			var status = Order.OrderStatus.InProgress;
			var storedProduct = new Product(1, "", 4, 10);

			dbServiceMock.Setup(repo => repo.GetClientByIdOrNull(customerId)).Returns((Client)null);
			dbServiceMock.Setup(repo => repo.GetAllProducts()).Returns(new List<Product> { storedProduct });
			dbServiceMock.Setup(repo => repo.GetProductByIdOrNull(productId)).Returns(storedProduct);
			dbServiceMock.Setup(repo => repo.GetOrderByIdOrNull(orderId)).Returns(new Order(orderId, customerId, productList, Order.OrderStatus.New));

			// Act and Assert
			Assert.ThrowsException<ArgumentException>(() => orderController.UpdateOrder(orderId, customerId, productList, status));
		}

		[TestMethod]
		public void UpdateOrder_InvalidProduct_ThrowsArgumentException()
		{
			// Arrange
			var dbServiceMock = new Mock<IDatabaseService>();

			var orderController = new OrderController(dbServiceMock.Object);

			var orderId = 1;
			var customerId = 2;
			var productId = 1;
			var storedProduct = new Product(1, "", 4, 10);
			var productList = new List<Product> { storedProduct };
			var status = Order.OrderStatus.InProgress;
			var newProduct = new Product(storedProduct.Id++, "", 3, 3);

			dbServiceMock.Setup(repo => repo.GetClientByIdOrNull(customerId)).Returns(new Client(customerId, "", "", ""));
			dbServiceMock.Setup(repo => repo.GetAllProducts()).Returns(productList);
			dbServiceMock.Setup(repo => repo.GetProductByIdOrNull(productId)).Returns(storedProduct);
			dbServiceMock.Setup(repo => repo.GetOrderByIdOrNull(orderId)).Returns(new Order(orderId, customerId, productList, Order.OrderStatus.New));

			// Act and Assert
			Assert.ThrowsException<ArgumentException>(() => orderController.UpdateOrder(orderId, customerId, new List<Product> { newProduct }, status));
		}
	}
}
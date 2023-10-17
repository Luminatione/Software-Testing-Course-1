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

			var orderController = new OrderController(orderRepositoryMock.Object, clientRepositoryMock.Object, productRepositoryMock.Object);

			var clientId = 1;
			var productId = 1;
			var productList = new List<Product> { new Product(productId, "", 4, 5) };
			var storedProduct = new Product(productId, "", 0, 10);


			clientRepositoryMock.Setup(repo => repo.GetClientById(clientId)).Returns(new Client(clientId, "", "", ""));
			productRepositoryMock.Setup(repo => repo.GetAllProducts()).Returns(new List<Product> { storedProduct });
			productRepositoryMock.Setup(repo => repo.GetProductById(productId)).Returns(storedProduct);
			orderRepositoryMock.Setup(repo => repo.GetAllOrders()).Returns(new List<Order> { new Order(0, clientId, new List<Product>(), Order.OrderStatus.Completed) });

			// Act
			orderController.CreateOrder(clientId, productList);

			// Assert
			orderRepositoryMock.Verify(repo => repo.AddOrder(It.IsAny<Order>()), Times.Once);
		}

		[TestMethod]
		public void CancelOrder_ExistingOrder_DeletesOrder()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();

			var orderController = new OrderController(orderRepositoryMock.Object, clientRepositoryMock.Object, productRepositoryMock.Object);

			var orderId = 1;

			orderRepositoryMock.Setup(repo => repo.GetOrderById(orderId)).Returns(new Order(orderId, 0, new List<Product>(), Order.OrderStatus.New));

			// Act
			orderController.CancelOrder(orderId);

			// Assert
			orderRepositoryMock.Verify(repo => repo.DeleteOrder(orderId), Times.Once);
		}

		[TestMethod]
		public void UpdateOrder_ValidData_UpdatesOrder()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();

			var orderController = new OrderController(orderRepositoryMock.Object, clientRepositoryMock.Object, productRepositoryMock.Object);

			var orderId = 1;
			var customerId = 2;
			var productId = 1;
			var productList = new List<Product> { new Product(1, "", 4, 5) };
			var status = Order.OrderStatus.InProgress;
			var storedProduct = new Product(1, "", 4, 10);


			clientRepositoryMock.Setup(repo => repo.GetClientById(customerId)).Returns(new Client(customerId, "", "", ""));
			productRepositoryMock.Setup(repo => repo.GetAllProducts()).Returns(new List<Product> { storedProduct });
			productRepositoryMock.Setup(repo => repo.GetProductById(productId)).Returns(storedProduct);
			orderRepositoryMock.Setup(repo => repo.GetOrderById(orderId)).Returns(new Order(orderId, customerId, productList, Order.OrderStatus.New));

			// Act
			orderController.UpdateOrder(orderId, customerId, productList, status);

			// Assert
			orderRepositoryMock.Verify(repo => repo.UpdateOrder(It.IsAny<Order>()), Times.Once);
		}

		[TestMethod]
		public void CreateOrder_InvalidClient_ThrowsArgumentException()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();

			var orderController = new OrderController(orderRepositoryMock.Object, clientRepositoryMock.Object, productRepositoryMock.Object);

			var clientId = 1;
			var productId = 1;
			var productList = new List<Product> { new Product(productId, "", 4, 5) };

			clientRepositoryMock.Setup(repo => repo.GetClientById(clientId)).Returns((Client)null);
			productRepositoryMock.Setup(repo => repo.GetAllProducts()).Returns(new List<Product>());

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

			var orderController = new OrderController(orderRepositoryMock.Object, clientRepositoryMock.Object, productRepositoryMock.Object);

			var clientId = 1;
			var productId = 1;
			var productList = new List<Product> { new Product(productId, "", 4, 5) };
			var product = new Product(productId + 1, "", 0, 10);

			clientRepositoryMock.Setup(repo => repo.GetClientById(clientId)).Returns(new Client(clientId, "", "", ""));
			productRepositoryMock.Setup(repo => repo.GetAllProducts()).Returns(productList);
			productRepositoryMock.Setup(repo => repo.GetProductById(productId)).Returns((Product)null);
			orderRepositoryMock.Setup(repo => repo.GetAllOrders()).Returns(new List<Order>());

			// Act and Assert
			Assert.ThrowsException<ArgumentException>(() => orderController.CreateOrder(clientId, new List<Product> { product }));
		}

		[TestMethod]
		public void UpdateOrder_InvalidClient_ThrowsArgumentException()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();

			var orderController = new OrderController(orderRepositoryMock.Object, clientRepositoryMock.Object, productRepositoryMock.Object);

			var orderId = 1;
			var customerId = 2;
			var productId = 1;
			var productList = new List<Product> { new Product(1, "", 4, 5) };
			var status = Order.OrderStatus.InProgress;
			var storedProduct = new Product(1, "", 4, 10);

			clientRepositoryMock.Setup(repo => repo.GetClientById(customerId)).Returns((Client)null);
			productRepositoryMock.Setup(repo => repo.GetAllProducts()).Returns(new List<Product> { storedProduct });
			productRepositoryMock.Setup(repo => repo.GetProductById(productId)).Returns(storedProduct);
			orderRepositoryMock.Setup(repo => repo.GetOrderById(orderId)).Returns(new Order(orderId, customerId, productList, Order.OrderStatus.New));

			// Act and Assert
			Assert.ThrowsException<ArgumentException>(() => orderController.UpdateOrder(orderId, customerId, productList, status));
		}

		[TestMethod]
		public void UpdateOrder_InvalidProduct_ThrowsArgumentException()
		{
			// Arrange
			var orderRepositoryMock = new Mock<IOrderRepository>();
			var clientRepositoryMock = new Mock<IClientRepository>();
			var productRepositoryMock = new Mock<IProductRepository>();

			var orderController = new OrderController(orderRepositoryMock.Object, clientRepositoryMock.Object, productRepositoryMock.Object);

			var orderId = 1;
			var customerId = 2;
			var productId = 1;
			var storedProduct = new Product(1, "", 4, 10);
			var productList = new List<Product> { storedProduct };
			var status = Order.OrderStatus.InProgress;
			var newProduct = new Product(storedProduct.Id++, "", 3, 3);

			clientRepositoryMock.Setup(repo => repo.GetClientById(customerId)).Returns(new Client(customerId, "", "", ""));
			productRepositoryMock.Setup(repo => repo.GetAllProducts()).Returns(productList);
			productRepositoryMock.Setup(repo => repo.GetProductById(productId)).Returns(storedProduct);
			orderRepositoryMock.Setup(repo => repo.GetOrderById(orderId)).Returns(new Order(orderId, customerId, productList, Order.OrderStatus.New));

			// Act and Assert
			Assert.ThrowsException<ArgumentException>(() => orderController.UpdateOrder(orderId, customerId, new List<Product> { newProduct }, status));
		}
	}
}
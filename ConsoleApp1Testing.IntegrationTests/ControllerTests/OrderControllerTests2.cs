using System;
using System.Collections.Generic;
using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using ConsoleApp1.DataBase;
using Moq;
using Xunit;

namespace ConsoleApp1.Tests
{
    public class OrderControllerTests2
    {
        [Fact]
        public void UpdateOrder_ValidData_UpdatesOrder()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var clientRepositoryMock = new Mock<IClientRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var databaseServiceMock = new Mock<IDatabaseService>();

            var orderController = new OrderController(
                orderRepositoryMock.Object,
                clientRepositoryMock.Object,
                productRepositoryMock.Object,
                databaseServiceMock.Object
            );

            var orderId = 1;
            var customerId = 1;
            var product = new Product(1, "", 4, 5);
            var productList = new List<Product> { product };
            var status = Order.OrderStatus.Completed;
            var existingOrder = new Order(
                orderId,
                customerId,
                new List<Product>(),
                Order.OrderStatus.New
            );

            databaseServiceMock
                .Setup(db => db.GetClientByIdOrNull(customerId))
                .Returns(new Client(customerId, "", "", ""));
            databaseServiceMock.Setup(db => db.GetAllProductsAsync().Result).Returns(productList);
            databaseServiceMock.Setup(db => db.GetOrderByIdOrNull(orderId)).Returns(existingOrder);

            databaseServiceMock.Setup(db => db.GetProductByIdOrNull(product.Id)).Returns(product);

            // Act
            orderController.UpdateOrder(orderId, customerId, productList, status);

            // Assert
            orderRepositoryMock.Verify(repo => repo.UpdateOrder(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public void UpdateOrder_InvalidData_ThrowsArgumentException()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var clientRepositoryMock = new Mock<IClientRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var databaseServiceMock = new Mock<IDatabaseService>();

            var orderController = new OrderController(
                orderRepositoryMock.Object,
                clientRepositoryMock.Object,
                productRepositoryMock.Object,
                databaseServiceMock.Object
            );

            var orderId = 1;
            var customerId = 1;
            var productList = new List<Product> { new Product(1, "", 4, 5) };
            var status = Order.OrderStatus.Completed;
            var existingOrder = new Order(
                orderId,
                customerId,
                new List<Product>(),
                Order.OrderStatus.New
            );

            databaseServiceMock
                .Setup(db => db.GetClientByIdOrNull(customerId))
                .Returns((Client)null);
            databaseServiceMock.Setup(db => db.GetAllProductsAsync().Result).Returns(productList);
            databaseServiceMock.Setup(db => db.GetOrderByIdOrNull(orderId)).Returns(existingOrder);

            Xunit.Assert.Throws<ArgumentException>(
                () => orderController.UpdateOrder(orderId, customerId, productList, status)
            );
        }

        [Fact]
        public void CancelOrder_ValidOrder_CancelsOrder()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var clientRepositoryMock = new Mock<IClientRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var databaseServiceMock = new Mock<IDatabaseService>();

            var orderController = new OrderController(
                orderRepositoryMock.Object,
                clientRepositoryMock.Object,
                productRepositoryMock.Object,
                databaseServiceMock.Object
            );

            var orderId = 1;
            var customerId = 1;
            var product = new Product(1, "", 4, 5);
            var productList = new List<Product> { product };
            var status = Order.OrderStatus.Completed;
            var existingOrder = new Order(
                orderId,
                customerId,
                new List<Product>(),
                Order.OrderStatus.New
            );

            databaseServiceMock.Setup(db => db.GetOrderByIdOrNull(orderId)).Returns(existingOrder);

            databaseServiceMock.Setup(db => db.GetProductByIdOrNull(product.Id)).Returns(product);

            // Act
            orderController.CancelOrder(orderId);

            // Assert
            databaseServiceMock.Verify(service => service.RemoveOrder(orderId), Times.Once);
        }

        [Fact]
        public void CancelOrder_NonExistentOrder_ThrowsArgumentException()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var clientRepositoryMock = new Mock<IClientRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var databaseServiceMock = new Mock<IDatabaseService>();

            var orderController = new OrderController(
                orderRepositoryMock.Object,
                clientRepositoryMock.Object,
                productRepositoryMock.Object,
                databaseServiceMock.Object
            );

            var orderId = 1;

            databaseServiceMock.Setup(db => db.GetOrderByIdOrNull(orderId)).Returns((Order)null);

            Xunit.Assert.Throws<ArgumentException>(() => orderController.CancelOrder(orderId));
        }
    }
}
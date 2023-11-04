﻿using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using ConsoleApp1.DataBase;
using Moq;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace ConsoleApp1.Tests
{
    public class OrderControllerTests
    {
        private readonly ITestOutputHelper output;

        public OrderControllerTests (ITestOutputHelper output)
        {
            this.output = output;
        }


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

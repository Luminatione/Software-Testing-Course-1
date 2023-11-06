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
    public class OrderControllerTests
    {
        [Fact]
        public void CreateOrder_ValidClientAndProducts_AddsOrder ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4.0m, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act
            orderController.CreateOrder (clientId, productList);
            int orderId = (orderController.GetAllOrders ().LastOrDefault ()?.ID ?? 0);

            // Assert
            Assert.Equal<Order> (
                new Order (orderId, clientId, productList, Order.OrderStatus.New),
                detabaseServiecve.GetOrderByIdOrNull (orderId)
            );
        }

        [Fact]
        public void CreateOrder_ValidClientAndProducts_AddsOrderThatExists ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act
            orderController.CreateOrder (clientId, productList);
            int orderId = (orderController.GetAllOrders ().LastOrDefault ()?.ID ?? 0);
            Order sameOrder = new Order (orderId, clientId, productList, Order.OrderStatus.New);

            // Assert
            Assert.Throws<OrderAlreadyExistsException> (() => detabaseServiecve.AddOrder (sameOrder));
        }

        [Fact]
        public void CreateOrder_InvalidClient_ThrowsArgumentException ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act and Assert
            Assert.Throws<ArgumentException> (
                () => orderController.CreateOrder (clientId + 1, productList)
            );
        }

        [Fact]
        public void CreateOrder_InvalidProduct_ThrowsArgumentException ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };
            var product = new Product (productId + 1, "", 0, 10);

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            // Act and Assert
            Assert.Throws<ArgumentException> (
                () => orderController.CreateOrder (clientId, new List<Product> { product })
            );
        }

        [Fact]
        public void CreateOrder_NullProduct_ThrowsArgumentNullException ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (detabaseServiecve);

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
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (detabaseServiecve);

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
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4.0m, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            orderController.CreateOrder (clientId, productList);
            int orderId = (orderController.GetAllOrders ().LastOrDefault ()?.ID ?? 0);
            var orderList = new List<Order> ()
            {
                new Order(orderId, clientId, productList, Order.OrderStatus.New)
            };

            // Act and Assert
            Assert.Equal<List<Order>> (
                Enumerable.Repeat (orderList, 1),
                Enumerable.Repeat (detabaseServiecve.GetAllOrders (), 1)
            );
        }

        [Fact]
        public void ReadOrder_ExistingData_GetsById ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();

            var orderController = new OrderController (detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            int orderId = (orderController.GetAllOrders ().LastOrDefault ()?.ID ?? 0);

            // Act and Assert
            Assert.Equal<Order> (
                new Order (orderId, clientId, productList, Order.OrderStatus.New),
                detabaseServiecve.GetOrderByIdOrNull (orderId)
            );
        }

        [Fact]
        public void ReadOrder_NoData_GetsAllOrder ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            var orderList = new List<Order> ();

            // Act and Assert
            Assert.Equal<List<Order>> (
                Enumerable.Repeat (orderList, 1),
                Enumerable.Repeat (detabaseServiecve.GetAllOrders(), 1)
            );
        }

        [Fact]
        public void ReadOrder_NotExistingData_GetsById ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (detabaseServiecve);

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
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var orderController = new OrderController (detabaseServiecve);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            detabaseServiecve.AddClient (new Client (clientId, "", "", ""));
            detabaseServiecve.AddProduct (productList[0]);

            detabaseServiecve.AddOrder (new Order (2, clientId, productList, Order.OrderStatus.New));

            // Act
            orderController.UpdateOrder (2, clientId, productList, Order.OrderStatus.Completed);

            // Assert
            Assert.Equal<Order> (
                new Order (2, clientId, productList, Order.OrderStatus.Completed),
                detabaseServiecve.GetOrderByIdOrNull (2)
            );
        }

        [Fact]
        public void UpdateOrder_InvalidData_ThrowsException ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var orderController = new OrderController (databaseService);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "", 4, 5) };

            databaseService.AddClient (new Client (clientId, "", "", ""));
            databaseService.AddProduct (productList[0]);

            // Act and Assert
            Assert.Throws<ArgumentException> (
                () =>
                    orderController.UpdateOrder (
                        2,
                        clientId,
                        productList,
                        Order.OrderStatus.Completed
                    )
            );
        }

        [Fact]
        public void UpdateOrder_ChangeProductQuantity ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var orderController = new OrderController (databaseService);

            var clientId = 1;
            var productId = 1;
            var productList = new List<Product> { new Product (productId, "ProductA", 5, 10) };

            databaseService.AddClient (
                new Client (clientId, "ClientA", "AddressA", "clientA@example.com")
            );
            databaseService.AddProduct (productList[0]);
            databaseService.AddOrder (new Order (1, clientId, productList, Order.OrderStatus.New));

            // Act:
            var updatedProductList = new List<Product>
            {
                new Product(productId, "ProductA", 7, 10)
            };
            orderController.UpdateOrder (
                1,
                clientId,
                updatedProductList,
                Order.OrderStatus.Completed
            );

            // Assert:
            var updatedOrder = databaseService.GetOrderByIdOrNull (1);
            Assert.NotNull (updatedOrder);
            Assert.Equal (updatedProductList, updatedOrder.Products);
        }

        [Fact]
        public void UpdateOrder_ChangeMultipleProducts ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var orderController = new OrderController (databaseService);

            var clientId = 1;
            var product1 = new Product (1, "ProductA", 5, 10);
            var product2 = new Product (2, "ProductB", 3, 15);
            var productList = new List<Product> { product1, product2 };

            databaseService.AddClient (
                new Client (clientId, "ClientA", "AddressA", "clientA@example.com")
            );
            databaseService.AddProduct (product1);
            databaseService.AddProduct (product2);
            databaseService.AddOrder (new Order (1, clientId, productList, Order.OrderStatus.New));

            // Act:
            var updatedProduct1 = new Product (1, "ProductA", 7, 10);
            var updatedProductList = new List<Product> { updatedProduct1, product2 };
            orderController.UpdateOrder (
                1,
                clientId,
                updatedProductList,
                Order.OrderStatus.Completed
            );

            // Assert:
            var updatedOrder = databaseService.GetOrderByIdOrNull (1);
            Assert.NotNull (updatedOrder);
            Assert.Equal (updatedProductList, updatedOrder.Products);
        }

        [Fact]
        public void CancelOrder_ValidOrder_CancelsOrder ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            OrderController orderController = new OrderController (databaseService);

            // Arrange:
            var client = new Client (1, "TestClient", "TestAddress", "TestEmail");
            databaseService.AddClient (client);
            var product = new Product (1, "TestProduct", 10, 5);
            databaseService.AddProduct (product);
            var order = new Order (
                1,
                client.Id,
                new List<Product> { product },
                Order.OrderStatus.New
            );
            databaseService.AddOrder (order);

            // Act:
            orderController.CancelOrder (order.ID);

            // Assert:
            var removedOrder = databaseService.GetOrderByIdOrNull (order.ID);
            Assert.Null (removedOrder);
        }

        [Fact]
        public void CancelOrder_OrderWithMultipleProducts_RestoresProductQuantities ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            OrderController orderController = new OrderController (databaseService);

            // Arrange:
            var client = new Client (1, "TestClient", "TestAddress", "TestEmail");
            databaseService.AddClient (client);
            var product1 = new Product (1, "ProductA", 10, 5);
            var product2 = new Product (2, "ProductB", 7, 3);
            var productList = new List<Product> { product1, product2 };
            databaseService.AddProduct (product1);
            databaseService.AddProduct (product2);
            var order = new Order (1, client.Id, productList, Order.OrderStatus.New);
            databaseService.AddOrder (order);
            // Act: 
            orderController.CancelOrder (order.ID);

            // Assert:
            var removedOrder = databaseService.GetOrderByIdOrNull (order.ID);
            Assert.Null (removedOrder);

            var restoredProduct1 = databaseService.GetProductByIdOrNull (product1.Id);
            var restoredProduct2 = databaseService.GetProductByIdOrNull (product2.Id);
            Assert.NotNull (restoredProduct1);
            Assert.NotNull (restoredProduct2);
            Assert.Equal (10, restoredProduct1.StoredAmount);
            Assert.Equal (6, restoredProduct2.StoredAmount);
        }

        [Fact]
        public void CancelOrder_InvalidOrder_ThrowsException ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();

            OrderController orderController = new OrderController (databaseService);

            // Act and Assert
            Assert.Throws<ArgumentException> (() => orderController.CancelOrder (1));
        }
    }
}

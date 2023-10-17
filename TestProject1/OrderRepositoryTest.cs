using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Repository.Tests
{
	[TestClass]
	public class OrderRepositoryTests
	{
		[TestMethod]
		public void AddOrder_ValidOrder_AddsOrder()
		{
			// Arrange
			var orderRepository = new OrderRepository();
			var orderToAdd = new Order(1, 1, new List<Product>(), Order.OrderStatus.New);

			// Act
			orderRepository.AddOrder(orderToAdd);

			// Assert
			CollectionAssert.Contains(orderRepository.GetAllOrders(), orderToAdd);
		}

		[TestMethod]
		public void GetAllOrders_EmptyRepository_ReturnsEmptyList()
		{
			// Arrange
			var orderRepository = new OrderRepository();

			// Act
			var result = orderRepository.GetAllOrders();

			// Assert
			CollectionAssert.AreEqual(new List<Order>(), result);
		}

		[TestMethod]
		public void GetOrderById_ExistingOrder_ReturnsOrder()
		{
			// Arrange
			var orderRepository = new OrderRepository();
			var orderToAdd = new Order(1, 1, new List<Product>(), Order.OrderStatus.New);
			orderRepository.AddOrder(orderToAdd);

			// Act
			var result = orderRepository.GetOrderById(1);

			// Assert
			Assert.AreEqual(orderToAdd, result);
		}

		[TestMethod]
		public void GetOrderById_NonexistentOrder_ReturnsNull()
		{
			// Arrange
			var orderRepository = new OrderRepository();

			// Act
			var result = orderRepository.GetOrderById(1);

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void UpdateOrder_ExistingOrder_UpdatesOrder()
		{
			// Arrange
			var orderRepository = new OrderRepository();
			var existingOrder = new Order(1, 1, new List<Product>(), Order.OrderStatus.New);
			orderRepository.AddOrder(existingOrder);

			var updatedOrder = new Order(1, 2, new List<Product>(), Order.OrderStatus.InProgress);

			// Act
			orderRepository.UpdateOrder(updatedOrder);

			// Assert
			var result = orderRepository.GetOrderById(1);
			Assert.AreEqual(updatedOrder.Status, result.Status);
			Assert.AreEqual(updatedOrder.ProductList, result.ProductList);
			Assert.AreEqual(updatedOrder.CustomerID, result.CustomerID);
		}

		[TestMethod]
		public void UpdateOrder_NonexistentOrder_ThrowsInvalidOperationException()
		{
			// Arrange
			var orderRepository = new OrderRepository();
			var updatedOrder = new Order(1, 2, new List<Product>(), Order.OrderStatus.InProgress);

			// Act and Assert
			Assert.ThrowsException<InvalidOperationException>(() => orderRepository.UpdateOrder(updatedOrder));
		}

		[TestMethod]
		public void DeleteOrder_ExistingOrder_RemovesOrder()
		{
			// Arrange
			var orderRepository = new OrderRepository();
			var orderToRemove = new Order(1, 1, new List<Product>(), Order.OrderStatus.New);
			orderRepository.AddOrder(orderToRemove);

			// Act
			orderRepository.DeleteOrder(1);

			// Assert
			CollectionAssert.DoesNotContain(orderRepository.GetAllOrders(), orderToRemove);
		}

		[TestMethod]
		public void DeleteOrder_NonexistentOrder_ThrowsInvalidOperationException()
		{
			// Arrange
			var orderRepository = new OrderRepository();

			// Act and Assert
			Assert.ThrowsException<InvalidOperationException>(() => orderRepository.DeleteOrder(1));
		}
	}
}

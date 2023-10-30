using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using ConsoleApp1.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public class OrderController(IOrderRepository orderRepository, IClientRepository clientRepository, IProductRepository productRepository, IDatabaseService databaseService)
	{
		private void ValidateClientId(int clientId)
		{
			if (databaseService.GetClientByIdOrNull(clientId) == null)
			{
				throw new ArgumentException("Client doesn't exist");
			}
		}

		private void ValidateProductList(List<Product> productList)
		{
			if (!productList.All(p => databaseService.GetAllProductsAsync().Select(e => e.Id).Contains(p.Id)))
			{
				throw new ArgumentException("Product doesn't exist");
			}

			bool canFullifyOrder = true;
			foreach (var product in productList)
			{
				canFullifyOrder &= databaseService.GetProductById(product.Id).StoredAmount >= product.StoredAmount;
			}

			if (!canFullifyOrder)
			{
				throw new ArgumentException("Order cannot be fullified");
			}
		}

		private void ValidateOrder(int orderId)
		{
			if (databaseService.GetOrderById(orderId) == null)
			{
				throw new ArgumentException("No order with such Id");
			}
		}

		public void CreateOrder(int clientId, List<Product> productList)
		{
			ValidateClientId(clientId);
			ValidateProductList(productList);
			// Nie da rady, bo brak metody GetAllOrders w DatabaseService
			int orderId = orderRepository.GetAllOrders().LastOrDefault()?.ID + 1 ?? 0;
			foreach (var product in productList)
			{
				productRepository.GetProductById(product.Id).StoredAmount -= product.StoredAmount;
			}
			orderRepository.AddOrder(new Order(orderId, clientId, productList, Order.OrderStatus.New));
		}

		public Order GetOrderById(int orderId)
		{
			return databaseService.GetOrderById(orderId);
		}

		public List<Order> GetAllOrders()
		{
			return databaseService.GetAllOrders();
		}

		public void CancelOrder(int orderId)
		{
			ValidateOrder(orderId);
			foreach (var product in databaseService.GetOrderById(orderId).ProductList)
			{
				databaseService.GetProductById(product.Id).StoredAmount += product.StoredAmount;
			}
			databaseService.RemoveOrder(orderId);
		}

		public void UpdateOrder(int orderId, int customerId, List<Product> productList, Order.OrderStatus status)
		{
			// Nie da rady bo brak update'ów w DatabaseService
			ValidateClientId(customerId);
			ValidateProductList(productList);
			ValidateOrder(orderId);
			foreach (var product in productList)
			{
				productRepository.GetProductById(product.Id).StoredAmount -= product.StoredAmount -
					orderRepository.GetOrderById(orderId).ProductList.Where(p => p.Id == product.Id).First()?.StoredAmount ?? 0;
			}
			orderRepository.UpdateOrder(new Order(orderId, customerId, productList, status));
		}
	}
}

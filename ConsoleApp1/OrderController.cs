using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public class OrderController(IOrderRepository orderRepository, IClientRepository clientRepository, IProductRepository productRepository)
	{
		private void ValidateClientId(int clientId)
		{
			if (clientRepository.GetClientById(clientId) == null)
			{
				throw new ArgumentException("Client doesn't exist");
			}
		}

		private void ValidateProductList(List<Product> productList)
		{
			if (!productList.All(p => productRepository.GetAllProducts().Select(e => e.Id).Contains(p.Id)))
			{
				throw new ArgumentException("Product doesn't exist");
			}

			bool canFullifyOrder = true;
			foreach (var product in productList)
			{
				canFullifyOrder &= productRepository.GetProductById(product.Id).StoredAmount >= product.StoredAmount;
			}
			
			if (!canFullifyOrder) 
			{
				throw new ArgumentException("Order cannot be fullified");
			}
		}

		private void ValidateOrder(int orderId)
		{
			if (orderRepository.GetOrderById(orderId) == null)
			{
				throw new ArgumentException("No order with such Id");
			}
		}

		public void CreateOrder(int clientId, List<Product> productList)
		{
			ValidateClientId(clientId);
			ValidateProductList(productList);
			int orderId = orderRepository.GetAllOrders().LastOrDefault()?.ID + 1 ?? 0;
			foreach(var product in productList)
			{
				productRepository.GetProductById(product.Id).StoredAmount -= product.StoredAmount;
			}
			orderRepository.AddOrder(new Order(orderId, clientId, productList, Order.OrderStatus.New));
		}

		public void CancelOrder(int orderId)
		{
			ValidateOrder(orderId);
            foreach (var product in orderRepository.GetOrderById(orderId).ProductList)
            {
                productRepository.GetProductById(product.Id).StoredAmount += product.StoredAmount;
            }
            orderRepository.DeleteOrder(orderId);	
		}

		public void UpdateOrder(int orderId, int customerId, List<Product> productList, Order.OrderStatus status)
		{
			ValidateClientId(customerId);
			ValidateProductList(productList);
			ValidateOrder(orderId);
            foreach (var product in productList)
            {
                productRepository.GetProductById(product.Id).StoredAmount -= product.StoredAmount -
					orderRepository.GetOrderById(orderId).ProductList.Where(p => p.Id == product.Id).First()?.StoredAmount ?? 0;
            }
            orderRepository.UpdateOrder(new Order(orderId, customerId , productList, status));
		}
	}
}

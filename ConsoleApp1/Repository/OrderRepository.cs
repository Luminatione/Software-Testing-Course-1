using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repository
{
    public class OrderRepository : IOrderRepository
	{
		private List<Order> orders;

		public OrderRepository()
		{
			orders = new List<Order>();
		}

		public void AddOrder(Order order)
		{
			orders.Add(order);
		}

		public List<Order> GetAllOrders()
		{
			return orders.ToList();
		}

		public Order? GetOrderById(int orderId)
		{
			return orders.FirstOrDefault(order => order.ID == orderId);
		}

		public void UpdateOrder(Order updatedOrder)
		{
			Order existingOrder = orders.FirstOrDefault(order => order.ID == updatedOrder.ID);

			if (existingOrder != null)
			{
				existingOrder.CustomerID = updatedOrder.CustomerID;
				existingOrder.ProductList = updatedOrder.ProductList;
				existingOrder.Status = updatedOrder.Status;
			}
			else
			{
				throw new InvalidOperationException("Order not found");
			}
		}

		public void DeleteOrder(int orderId)
		{
			int index = orders.FindIndex(order => order.ID == orderId);

			if (index != -1)
			{
				orders.RemoveAt(index);
			}
			else
			{
				throw new InvalidOperationException("Order not found");
			}
		}
	}
}

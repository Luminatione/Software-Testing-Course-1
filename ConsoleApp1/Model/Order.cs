using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp1.Model.Order;

namespace ConsoleApp1.Model
{
	public class Order(int id, int customerId, List<Product> productList, OrderStatus status)
	{
		public enum OrderStatus
		{
			New,
			InProgress,
			Completed
		}

		public int ID { get; set; } = id;
		public int CustomerID { get; set; } = customerId;
		public List<Product> Products { get; set; } = productList;
		public OrderStatus Status { get; set; } = status;

		public Order() : this(0, 0, new List<Product>(), OrderStatus.New) { }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static ConsoleApp1.Model.Order;

namespace ConsoleApp1.Model
{
	public class Order
	{
		public enum OrderStatus
		{
			New,
			InProgress,
			Completed
		}

		public int ID { get; set; }
		public int CustomerID { get; set; }
		public List<Product> ProductList = new List<Product>();
		public OrderStatus Status { get; set; }

		public Order()
		{

		}

		public Order (int id, int customerId, List<Product> productList, OrderStatus status)
		{
			ID = id;
			CustomerID = customerId;
			ProductList = productList;
			Status = status;
		}

        public override bool Equals (object obj)
        {
            if (obj == null || !(obj is Order other))
            {
                return false;
            }

            return this.ID == other.ID
                && this.CustomerID == other.CustomerID
                && this.ProductList.Equals (other.ProductList)
                && this.Status == other.Status;
        }
    }
}

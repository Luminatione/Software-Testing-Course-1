using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model
{
	public class Product(int id, string name, decimal price, int storedAmount)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public decimal Price { get; set; } = price;
        public int StoredAmount { get; set; } = storedAmount;

        public override bool Equals (object obj)
        {
            if (obj == null || !(obj is Product other))
            {
                return false;
            }

            return this.Id == other.Id
                && this.Name == other.Name
                && this.Price == other.Price
                && this.StoredAmount == other.StoredAmount;
        }
    }
}

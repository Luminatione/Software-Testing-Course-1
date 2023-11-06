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

        public Product() : this(0, "", 0, 0) { }    
    }
}

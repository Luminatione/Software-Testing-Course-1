using ConsoleApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repository.Interface
{
	public interface IProductRepository
	{
		void AddProduct(Product product);
		List<Product> GetAllProducts();
		Product GetProductById(int productId);
		void UpdateProduct(Product updatedProduct);
		void DeleteProduct(int productId);
	}
}

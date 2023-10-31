using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repository
{
	public class ProductRepository : IProductRepository
	{
		private List<Product> products;

		public ProductRepository()
		{
			products = new List<Product>();
		}

		public void AddProduct(Product product)
		{
			products.Add(product);
		}

		public List<Product> GetAllProducts()
		{
			return products.ToList();
		}

		public Product? GetProductById(int productId)
		{
			return products.FirstOrDefault(product => product.Id == productId);
		}

		public void UpdateProduct(Product updatedProduct)
		{
			Product existingProduct = products.FirstOrDefault(product => product.Id == updatedProduct.Id);

			if (existingProduct != null)
			{
				existingProduct.Name = updatedProduct.Name;
				existingProduct.Price = updatedProduct.Price;
				existingProduct.StoredAmount = updatedProduct.StoredAmount;
			}
			else
			{
				throw new InvalidOperationException("Product not found");
			}
		}

		public void DeleteProduct(int productId)
		{
			int index = products.FindIndex(product => product.Id == productId);

			if (index != -1)
			{
				products.RemoveAt(index);
			}
			else
			{
				throw new InvalidOperationException("Product not found");
			}
		}
	}
}

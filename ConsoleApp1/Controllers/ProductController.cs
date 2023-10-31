using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using System;
using System.Collections.Generic;

namespace ConsoleApp1.Controllers
{
    public class ProductController
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepo)
        {
            this.productRepository = productRepo;
        }

        public void CreateProduct(int id, string name, decimal price, int storedAmount)
        {
            Product newProduct = new Product(id, name, price, storedAmount);
            productRepository.AddProduct(newProduct);
        }

        public Product? GetProductById(int productId)
        {
            return productRepository.GetProductById(productId);
        }

        public List<Product> GetAllProducts()
        {
            return productRepository.GetAllProducts();
        }

        public void UpdateProduct(int productId, string name, decimal price, int storedAmount)
        {
            Product existingProduct = productRepository.GetProductById(productId);
            existingProduct.Name = name;
            existingProduct.Price = price;
            existingProduct.StoredAmount = storedAmount;
            productRepository.UpdateProduct(existingProduct);
        }

        public void DeleteProduct(int productId)
        {
            productRepository.DeleteProduct(productId);
        }
    }
}
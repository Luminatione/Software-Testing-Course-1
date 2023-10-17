using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Repository.Tests
{
	[TestClass]
	public class ProductRepositoryTests
	{
		[TestMethod]
		public void AddProduct_ValidProduct_AddsProduct()
		{
			// Arrange
			var productRepository = new ProductRepository();
			var productToAdd = new Product(1, "Product1", 10, 20);

			// Act
			productRepository.AddProduct(productToAdd);

			// Assert
			CollectionAssert.Contains(productRepository.GetAllProducts(), productToAdd);
		}

		[TestMethod]
		public void GetAllProducts_EmptyRepository_ReturnsEmptyList()
		{
			// Arrange
			var productRepository = new ProductRepository();

			// Act
			var result = productRepository.GetAllProducts();

			// Assert
			CollectionAssert.AreEqual(new List<Product>(), result);
		}

		[TestMethod]
		public void GetProductById_ExistingProduct_ReturnsProduct()
		{
			// Arrange
			var productRepository = new ProductRepository();
			var productToAdd = new Product(1, "Product1", 10, 20);
			productRepository.AddProduct(productToAdd);

			// Act
			var result = productRepository.GetProductById(1);

			// Assert
			Assert.AreEqual(productToAdd, result);
		}

		[TestMethod]
		public void GetProductById_NonexistentProduct_ReturnsNull()
		{
			// Arrange
			var productRepository = new ProductRepository();

			// Act
			var result = productRepository.GetProductById(1);

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void UpdateProduct_ExistingProduct_UpdatesProduct()
		{
			// Arrange
			var productRepository = new ProductRepository();
			var existingProduct = new Product(1, "Product1", 10, 20);
			productRepository.AddProduct(existingProduct);

			var updatedProduct = new Product(1, "ProductUpdated", 15, 30);

			// Act
			productRepository.UpdateProduct(updatedProduct);

			// Assert
			var result = productRepository.GetProductById(1);
			Assert.AreEqual(updatedProduct.Name, result.Name);
			Assert.AreEqual(updatedProduct.Price, result.Price);
			Assert.AreEqual(updatedProduct.StoredAmount, result.StoredAmount);
		}

		[TestMethod]
		public void UpdateProduct_NonexistentProduct_ThrowsInvalidOperationException()
		{
			// Arrange
			var productRepository = new ProductRepository();
			var updatedProduct = new Product(1, "ProductUpdated", 15, 30);

			// Act and Assert
			Assert.ThrowsException<InvalidOperationException>(() => productRepository.UpdateProduct(updatedProduct));
		}

		[TestMethod]
		public void DeleteProduct_ExistingProduct_RemovesProduct()
		{
			// Arrange
			var productRepository = new ProductRepository();
			var productToRemove = new Product(1, "Product1", 10, 20);
			productRepository.AddProduct(productToRemove);

			// Act
			productRepository.DeleteProduct(1);

			// Assert
			CollectionAssert.DoesNotContain(productRepository.GetAllProducts(), productToRemove);
		}

		[TestMethod]
		public void DeleteProduct_NonexistentProduct_ThrowsInvalidOperationException()
		{
			// Arrange
			var productRepository = new ProductRepository();

			// Act and Assert
			Assert.ThrowsException<InvalidOperationException>(() => productRepository.DeleteProduct(1));
		}
	}
}
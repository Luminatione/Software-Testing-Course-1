using ConsoleApp1.DataBase;
using ConsoleApp1.Model;

namespace ConsoleApp1.Controllers
{
    public class ProductController
    {
        private readonly IDatabaseService context;

        public ProductController(IDatabaseService context)
        {
            this.context = context;
        }

        public void CreateProduct(int id, string name, decimal price, int storedAmount)
        {
            Product newProduct = new Product(id, name, price, storedAmount);
            context.AddProduct(newProduct);
        }

        public Product? GetProductById(int productId)
        {
            return context.GetProductByIdOrNull(productId);
        }

        public List<Product> GetAllProducts()
        {
            return context.GetAllProducts();
        }

        public void UpdateProduct(int productId, string name, decimal price, int storedAmount)
        {
            Product existingProduct = GetProductById(productId) ?? throw new ArgumentException("No product with such Id");
            existingProduct.Name = name;
            existingProduct.Price = price;
            existingProduct.StoredAmount = storedAmount;
            context.UpdateProduct(existingProduct);
        }

        public void DeleteProduct(int productId)
        {
            context.RemoveProduct(productId);
        }
    }
}
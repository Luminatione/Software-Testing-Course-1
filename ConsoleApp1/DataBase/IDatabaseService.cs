using ConsoleApp1.Model;

namespace ConsoleApp1.DataBase
{
    public interface IDatabaseService
    {
        public void AddOrder(Order newOrder);
        public void AddClient(Client newClient);
        public void AddProduct(Product newProduct);

        public void RemoveOrder(int orderId);
        public void RemoveClient(int clientId);
        public void RemoveProduct(int productId);

        public Order? GetOrderByIdOrNull(int orderId);
        public Client? GetClientByIdOrNull(int clientId);
        public Product? GetProductByIdOrNull(int productId);
        public Task<List<Product>> GetAllProductsAsync();
    }
}


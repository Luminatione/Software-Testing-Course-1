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

        public List<Product> GetAllProducts();
        public List<Order> GetAllOrders();
        public List<Client> GetAllClients();

        public void UpdateOrder(Order order);
        public void UpdateClient(Client client);
        public void UpdateProduct(Product product);
    }
}


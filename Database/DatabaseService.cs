using ConsoleApp1.Model;

namespace Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseContext _databaseContext;

        public DatabaseService()
        {
            _databaseContext = new DatabaseContext();
        }

        public void AddOrder(Order newOrder)
        {
            if (_databaseContext.Orders.Contains(newOrder))
            {
                throw new OrderAlreadyExistsException(newOrder.ID);
            }

            _databaseContext.Orders.Add(newOrder);
            _databaseContext.SaveChanges();
        }

        public void AddClient(Client newClient)
        {
            if (_databaseContext.Clients.Contains(newClient))
            {
                throw new ClientAlreadyExistsException(newClient.Id);
            }

            _databaseContext.Clients.Add(newClient);
            _databaseContext.SaveChanges();
        }

        public void AddProduct(Product newProduct)
        {
            if (_databaseContext.Products.Contains(newProduct))
            {
                throw new ProductAlreadyExistsException(newProduct.Id);
            }

            _databaseContext.Products.Add(newProduct);
            _databaseContext.SaveChanges();
        }

        public void RemoveOrder(int orderId)
        {
            var orderToRemove = _databaseContext.Orders.FirstOrDefault(o => o.ID == orderId);
            if (orderToRemove == null)
            {
                throw new OrderNotFoundException(orderId);
            }

            _databaseContext.Orders.Remove(orderToRemove);
        }

        public void RemoveClient(int clientId)
        {
            var clientToRemove = _databaseContext.Clients.FirstOrDefault(c => c.Id == clientId);
            if (clientToRemove == null)
            {
                throw new ClientNotFoundException(clientId);
            }

            _databaseContext.Clients.Remove(clientToRemove);
        }

        public void RemoveProduct(int productId)
        {
            var productToRemove = _databaseContext.Products.FirstOrDefault(p => p.Id == productId);
            if (productToRemove == null)
            {
                throw new ProductNotFoundException(productId);
            }

            _databaseContext.Products.Remove(productToRemove);
        }

        public Order? GetOrderByIdOrNull(int orderId)
        {
            return _databaseContext.Orders.FirstOrDefault(o => o.ID == orderId);
        }

        public Client? GetClientByIdOrNull(int clientId)
        {
            return _databaseContext.Clients.FirstOrDefault(c => c.Id == clientId);
        }

        public Product? GetProductByIdOrNull(int productId)
        {
            return _databaseContext.Products.FirstOrDefault(p => p.Id == productId);
        }
    }
}

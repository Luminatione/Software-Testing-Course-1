using ConsoleApp1.DataBase;
using ConsoleApp1.Model;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseContext _databaseContext;

        public DatabaseService()
        {
            _databaseContext = new DatabaseContext();
            _databaseContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _databaseContext.ChangeTracker.Clear();
        }

        public void AddOrder(Order newOrder)
        {
            if (_databaseContext.Orders.Contains(newOrder))
            {
                throw new OrderAlreadyExistsException(newOrder.ID);
            }
            List<Product> products = new List<Product>();
            foreach (var product in newOrder.Products)
            {
                products.Add(GetProductByIdOrNull(product.Id) ?? throw new ProductNotFoundException(product.Id));
            }
            newOrder.Products = products;
            _databaseContext.ChangeTracker.Clear();
            newOrder.Products.ForEach(p => _databaseContext.Entry(p).State = EntityState.Unchanged);
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
            _databaseContext.SaveChanges();
        }

        public void RemoveClient(int clientId)
        {
            var clientToRemove = _databaseContext.Clients.FirstOrDefault(c => c.Id == clientId);
            if (clientToRemove == null)
            {
                throw new ClientNotFoundException(clientId);
            }

            _databaseContext.Entry(clientToRemove).State = EntityState.Deleted;
            _databaseContext.Clients.Remove(clientToRemove);
            _databaseContext.SaveChanges();
        }

        public void RemoveProduct(int productId)
        {
            var productToRemove = _databaseContext.Products.FirstOrDefault(p => p.Id == productId);
            if (productToRemove == null)
            {
                throw new ProductNotFoundException(productId);
            }
            _databaseContext.Entry(productToRemove).State = EntityState.Deleted;
            _databaseContext.Products.Remove(productToRemove);
            _databaseContext.SaveChanges();
        }

        public Order? GetOrderByIdOrNull(int orderId)
        {
            return _databaseContext.Orders.Include(e => e.Products).FirstOrDefault(o => o.ID == orderId);
        }

        public Client? GetClientByIdOrNull(int clientId)
        {
            return _databaseContext.Clients.FirstOrDefault(c => c.Id == clientId);
        }

        public Product? GetProductByIdOrNull(int productId)
        {
            return _databaseContext.Products.FirstOrDefault(p => p.Id == productId);
        }

        public void UpdateOrder(Order order)
        {
            List<Product> products = new List<Product>();
            foreach (var product in order.Products)
            {
                products.Add(GetProductByIdOrNull(product.Id) ?? throw new ProductNotFoundException(product.Id));
            }
            order.Products = products;
            _databaseContext.ChangeTracker.Clear();
            order.Products.ForEach(p => _databaseContext.Entry(p).State = EntityState.Unchanged);
            _databaseContext.Orders.Update(order);
            _databaseContext.SaveChanges();
        }

        public void UpdateClient(Client client)
        {
            _databaseContext.Entry(client).State = EntityState.Modified;
            _databaseContext.Clients.Update(client);
            _databaseContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _databaseContext.Entry(product).State = EntityState.Modified;
            _databaseContext.Products.Update(product);
            _databaseContext.SaveChanges();
        }

        public List<Product> GetAllProducts()
        {
            return _databaseContext.Products.ToList();
        }

        public List<Order> GetAllOrders()
        {
            return _databaseContext.Orders.Include(e => e.Products).ToList();
        }

        public List<Client> GetAllClients()
        {
            return _databaseContext.Clients.ToList();
        }
    }
}
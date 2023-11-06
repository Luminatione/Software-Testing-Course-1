using ConsoleApp1.DataBase;
using ConsoleApp1.Model;

namespace ConsoleApp1.Controllers
{
    public class OrderController(IDatabaseService context)
    {
        private void ValidateClientId(int clientId)
        {
            if (context.GetClientByIdOrNull(clientId) == null)
            {
                throw new ArgumentException("Client doesn't exist");
            }
        }

        private void ValidateProductList(List<Product> productList)
        {
            if (!productList.All(p => context.GetAllProducts().Select(e => e.Id).Contains(p.Id)))
            {
                throw new ArgumentException("Product doesn't exist");
            }

            bool canFulfillOrder = true;
            foreach (var product in productList)
            {
                canFulfillOrder &= context.GetProductByIdOrNull(product.Id).StoredAmount >= product.StoredAmount;
            }

            if (!canFulfillOrder)
            {
                throw new ArgumentException("Order cannot be fulfilled");
            }
        }

        private void ValidateOrder(int orderId)
        {
            if (GetOrderById(orderId) == null)
            {
                throw new ArgumentException("No order with such Id");
            }
        }

        public void CreateOrder(int clientId, List<Product> productList)
        {
            ValidateClientId(clientId);
            ValidateProductList(productList);
            int orderId = GetAllOrders().LastOrDefault()?.ID + 1 ?? 0;
            foreach (var product in productList)
            {
                var storedProduct = context.GetProductByIdOrNull(product.Id);
                storedProduct.StoredAmount -= product.StoredAmount;
                //context.UpdateProduct(storedProduct);
            }
            context.AddOrder(new Order(orderId, clientId, productList, Order.OrderStatus.New));
        }

        public Order? GetOrderById(int orderId)
        {
            return context.GetOrderByIdOrNull(orderId);
        }

        public List<Order> GetAllOrders()
        {
            return context.GetAllOrders();
        }

        public void CancelOrder(int orderId)
        {
            ValidateOrder(orderId);
            foreach (var product in GetOrderById(orderId).Products)
            {
                context.GetProductByIdOrNull(product.Id).StoredAmount += product.StoredAmount;
            }
            context.RemoveOrder(orderId);
        }

        public void UpdateOrder(int orderId, int customerId, List<Product> productList, Order.OrderStatus status)
        {
            ValidateClientId(customerId);
            ValidateProductList(productList);
            ValidateOrder(orderId);
            Order order = GetOrderById(orderId);
            order.CustomerID = customerId;
            order.Products = productList;
            order.Status = status;
            foreach (var product in productList)
            {
                context.GetProductByIdOrNull(product.Id).StoredAmount -= product.StoredAmount -
                     GetOrderById(orderId).Products.Where(p => p.Id == product.Id).First()?.StoredAmount ?? 0;
            }
            context.UpdateOrder(order);
        }
    }
}

using ConsoleApp1.Model;

namespace ConsoleApp1.Repository.Interface
{
	public interface IOrderRepository
    {
        void AddOrder(Order order);
        List<Order> GetAllOrders();
        Order GetOrderById(int orderId);
        void UpdateOrder(Order updatedOrder);
        void DeleteOrder(int orderId);
    }
}
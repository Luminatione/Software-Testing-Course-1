using ConsoleApp1.DataBase;

namespace ConsoleApp1.DataBase
{
    public class DatabaseException : Exception
    {
        private protected DatabaseException(string message): base(message)
        {
        }
    }
    
}

public class OrderNotFoundException : DatabaseException
{
    public int OrderId { get; }

    public OrderNotFoundException(int orderId) : base($"Order with ID {orderId} was not found.")
    {
        OrderId = orderId;
    }
}

public class ProductNotFoundException : DatabaseException
{
    public int ProductId { get; }

    public ProductNotFoundException(int productId) : base($"Product '{productId}' was not found.")
    {
        ProductId = productId;
    }
}

public class ClientNotFoundException : DatabaseException
{
    public int ClientId { get; }

    public ClientNotFoundException(int clientId) : base($"Client with ID {clientId} was not found.")
    {
        ClientId = clientId;
    }
}

public class ClientAlreadyExistsException : DatabaseException
{
    public int CllientId { get; }

    public ClientAlreadyExistsException(int clientId) : base($"Client '{clientId}' already exists.")
    {
        CllientId = clientId;
    }
}

public class OrderAlreadyExistsException : DatabaseException
{
    public int OrderId { get; }

    public OrderAlreadyExistsException(int orderId) : base($"Order with ID {orderId} already exists.")
    {
        OrderId = orderId;
    }
}

public class ProductAlreadyExistsException : DatabaseException
{
    public int ProductId { get; }

    public ProductAlreadyExistsException(int productId) : base($"Product '{productId}' already exists.")
    {
        ProductId = productId;
    }
}

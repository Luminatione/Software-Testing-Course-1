using ConsoleApp1;
using ConsoleApp1.Model;
using ConsoleApp1.Repository;
using Database;

public class Program
{
    private static int getClient(ClientRepository clientRepository)
    {
        Console.WriteLine("List of clients:");
        foreach (Client client in clientRepository.GetAllClients())
        {
            Console.WriteLine($"ID: {client.Id}, Name: {client.Name}, SecondName: {client.SecondName}, Email: {client.Email}");
        }
        while (true)
        {
            Console.WriteLine($"Enter the ID of client:");
            string clientIdInput = Console.ReadLine();
            if (int.TryParse(clientIdInput, out int clientId))
            {
                return clientId;
            }
            else
            {
                Console.WriteLine("Client with the given ID not found. Please try again.");
                continue;
            }
        }
    }
    private static int getOrder(OrderRepository orderRepository)
    {
        Console.WriteLine("List of orders:");
        foreach (Order order in orderRepository.GetAllOrders())
        {
            Console.WriteLine($"ID: {order.ID}, CustomerID: {order.CustomerID}, Status: {order.Status}, Products:");
            foreach (Product product in order.ProductList)
            {
                Console.WriteLine($"-{product.Name}, Quantity: {product.StoredAmount}");
            }
        }
        while (true)
        {
            Console.WriteLine($"Enter the ID of order:");
            string orderIdInput = Console.ReadLine();
            if (int.TryParse(orderIdInput, out int orderId))
            {
                return orderId;
            }
            else
            {
                Console.WriteLine("Order with the given ID not found. Please try again.");
                continue;
            }
        }
    }

    private static List<Product> getProducts(ProductRepository productRepository)
    {
        List<Product> orderedProducts = new List<Product>();

        Console.WriteLine("Available products:");
        foreach (Product product in productRepository.GetAllProducts())
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price:C}, Stored Amount: {product.StoredAmount}");
        }
        while (true)
        {
            Console.WriteLine("Enter the number of articles you would like to order");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int productCount))
            {
                if (productCount <= 0)
                {
                    Console.WriteLine("Please enter a valid positive number of articles to order.");
                    continue;
                }
                for (int i = 0; i < productCount; i++)
                {
                    Console.WriteLine($"Please enter id of {i + 1} product");
                    string productIdInput = Console.ReadLine();

                    if (int.TryParse(productIdInput, out int productId))
                    {
                        Product selectedProduct = productRepository.GetProductById(productId);

                        if (selectedProduct != null)
                        {
                            Console.WriteLine($"Selected product: {selectedProduct.Name}, Price: {selectedProduct.Price:C}, Available Quantity: {selectedProduct.StoredAmount}");

                            Console.WriteLine("Enter the quantity you would like to order:");
                            string quantityInput = Console.ReadLine();

                            if (int.TryParse(quantityInput, out int quantity))
                            {
                                if (quantity > 0 && quantity <= selectedProduct.StoredAmount)
                                {
                                    selectedProduct.StoredAmount -= quantity;
                                    Product orderedProduct = new Product
                                    (
                                        selectedProduct.Id,
                                        selectedProduct.Name,
                                        selectedProduct.Price,
                                        quantity
                                    );

                                    orderedProducts.Add(orderedProduct);
                                    Console.WriteLine($"Added '{orderedProduct.Name}' ({quantity}x) to your order.");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid quantity. Please enter a valid positive number within the available amount.");
                                    i--;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid quantity. Please enter a valid integer.");
                                i--;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Product with the given ID not found. Please try again.");
                            i--;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid integer.");
                        i--;
                    }
                }
                break;
            }
            else
            {
                Console.WriteLine("The entered value is not an integer.");
            }
        }

        return orderedProducts;
    }

    private static Order.OrderStatus getStatus()
    {
        Console.WriteLine("Possible options:");
        Console.WriteLine("1. NEW");
        Console.WriteLine("2. IN PROGRESS");
        Console.WriteLine("3. COMPLETED");
        char input = Console.ReadKey().KeyChar;

        while (true)
        {
            switch (input)
            {
                case '1':
                    return Order.OrderStatus.New;
                case '2':
                    return Order.OrderStatus.InProgress;
                case '3':
                    return Order.OrderStatus.Completed;
                default:
                    Console.WriteLine("Invalid choice. Please choose a number from 1 to 3.");
                    break;
            }

        }
    }



    private static void Main(string[] args)
    {
        List<Product>? productList;
        int clientId;
        int orderToUpdateId;
        int orderToDeleteId;
        Order? order;
        char input;
        Order.OrderStatus status;

        while (true)
        {
            // Trzeba wstrzykiwanie zrobić elegancko
            ClientRepository clientRepository = new ClientRepository();
            ProductRepository productRepository = new ProductRepository();
            OrderRepository orderRepository = new OrderRepository();
            DatabaseService databaseService = new DatabaseService ();

            OrderController orderController = new OrderController(orderRepository, clientRepository, productRepository, databaseService);

            Console.WriteLine("MENU");
            Console.WriteLine("1. View orders");
            Console.WriteLine("2. Create order");
            Console.WriteLine("3. Update order");
            Console.WriteLine("4. Delete order");
            Console.WriteLine("5. Exit");

            char key = Console.ReadKey().KeyChar;

            switch (key)
            {
                case '1':
                    Console.Clear();
                    Console.WriteLine("List of orders:");
                    foreach (Order obj in orderRepository.GetAllOrders())
                    {
                        Console.WriteLine($"ID: {obj.ID}, CustomerID: {obj.CustomerID}, Status: {obj.Status}, Products:");
                        foreach (Product product in obj.ProductList)
                        {
                            Console.WriteLine($"-{product.Name}, Quantity: {product.StoredAmount}");
                        }
                    }
                    Console.WriteLine("\n\n");
                    break;

                case '2':
                    Console.Clear();
                    Console.WriteLine("Creating order");
                    productList = getProducts(productRepository);
                    clientId = getClient(clientRepository);
                    orderController.CreateOrder(clientId, productList);
                    Console.WriteLine("Order created succesfully");
                    break;

                case '3':
                    Console.Clear();
                    Console.WriteLine("Updating order");
                    orderToUpdateId = getOrder(orderRepository);
                    order = orderRepository.GetOrderById(orderToUpdateId);
                    Console.WriteLine("Choosen order:");
                    Console.WriteLine($"ID: {order.ID}, CustomerID: {order.CustomerID}, Status: {order.Status}, Products:");
                    foreach (Product product in order.ProductList)
                    {
                        Console.WriteLine($"-{product.Name}, Quantity: {product.StoredAmount}");
                    }
                    Console.WriteLine("\n\n");
                    Console.WriteLine("Want to change client? (Y/N)");
                    input = Console.ReadKey().KeyChar;
                    if (input == 'Y')
                    {
                        clientId = getClient(clientRepository);
                    }
                    else
                    {
                        clientId = order.CustomerID;
                    }
                    Console.WriteLine("Want to change products? (Y/N)");
                    input = Console.ReadKey().KeyChar;
                    if (input == 'Y')
                    {
                        productList = getProducts(productRepository);
                    }
                    else
                    {
                        productList = order.ProductList;
                    }
                    Console.WriteLine("Want to chagne status? (Y/N)");
                    input = Console.ReadKey().KeyChar;
                    if (input == 'Y')
                    {
                        status = getStatus();
                    }
                    else
                    {
                        status = order.Status;
                    }
                    orderController.UpdateOrder(orderToUpdateId, clientId, productList, status);
                    break;

                case '4':
                    Console.Clear();
                    Console.WriteLine("Deleting order");
                    orderToDeleteId = getOrder(orderRepository);
                    orderController.CancelOrder(orderToDeleteId);
                    Console.WriteLine("Order deleted succesfully");
                    break;

                case '5':
                    Console.Clear();
                    return;

                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice. Please choose a number from 1 to 5.");
                    continue;
            }
        }
    }
}
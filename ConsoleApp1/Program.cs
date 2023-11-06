using ConsoleApp1.Controllers;
using ConsoleApp1.DataBase;
using ConsoleApp1.Model;
using ConsoleApp1.Repository;
using Database;
using System.ComponentModel;

public class Program
{
    private static int getProduct(List<Product> products)
    {
        Console.WriteLine("List of products:");
        foreach (Product product in products)
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Stored Amount: {product.StoredAmount}");
        }
        while (true)
        {
            Console.WriteLine($"Enter the ID of product:");
            string productIdInput = Console.ReadLine();
            if (int.TryParse(productIdInput, out int productId))
            {
                return productId;
            }
            else
            {
                Console.WriteLine("Client with the given ID not found. Please try again.");
                continue;
            }
        }
    }
    private static int getClient(List<Client> clients)
    {
        Console.WriteLine("List of clients:");
        foreach (Client client in clients)
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
    private static int getOrder(List<Order> orders)
    {
        Console.WriteLine("List of orders:");
        foreach (Order order in orders)
        {
            Console.WriteLine($"ID: {order.ID}, CustomerID: {order.CustomerID}, Status: {order.Status}, Products:");
            foreach (Product product in order.Products)
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

    private static List<Product> getProducts(List<Product> products)
    {
        List<Product> orderedProducts = new List<Product>();

        Console.WriteLine("Available products:");
        foreach (Product product in products)
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
                        Product selectedProduct = products.FirstOrDefault(p => p.Id == productId);

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

    private static void orderMenu(OrderController orderController, ClientController clientController, ProductController productController)
    {
        while (true)
        {
            List<Product>? productList;
            Product old_product;
            int clientId;
            int orderToUpdateId;
            int orderToDeleteId;
            Order? order;
            Order? old_order;
            int new_amount;
            char input;
            Order.OrderStatus status;

            Console.WriteLine("ORDER MENU");
            Console.WriteLine("1. View orders");
            Console.WriteLine("2. Create order");
            Console.WriteLine("3. Update order");
            Console.WriteLine("4. Delete order");
            Console.WriteLine("5. Exit");

            char key = Console.ReadKey().KeyChar;
            Console.Clear();

            switch (key)
            {
                case '1':
                    Console.WriteLine("List of orders:");
                    foreach (Order obj in orderController.GetAllOrders())
                    {
                        Console.WriteLine($"ID: {obj.ID}, CustomerID: {obj.CustomerID}, Status: {obj.Status}, Products:");
                        foreach (Product product in obj.Products)
                        {
                            Console.WriteLine($"-{product.Name}, Quantity: {product.StoredAmount}");
                        }
                    }
                    Console.WriteLine("\n\n");
                    break;

                case '2':
                    Console.WriteLine("Creating order");
                    productList = getProducts(productController.GetAllProducts());
                    clientId = getClient(clientController.GetAllClients());
                    orderController.CreateOrder(clientId, productList);
                    Console.WriteLine("Order created succesfully");
                    foreach (Product product in productList)
                    {
                        old_product = productController.GetProductById(product.Id);
                        new_amount = old_product.StoredAmount - product.StoredAmount;
                        productController.UpdateProduct(product.Id, product.Name, product.Price, new_amount);
                    }
                    break;

                case '3':
                    Console.WriteLine("Updating order");
                    orderToUpdateId = getOrder(orderController.GetAllOrders());
                    order = orderController.GetAllOrders().FirstOrDefault(o => o.ID == orderToUpdateId);
                    Console.WriteLine("Choosen order:");
                    Console.WriteLine($"ID: {order.ID}, CustomerID: {order.CustomerID}, Status: {order.Status}, Products:");
                    foreach (Product product in order.Products)
                    {
                        Console.WriteLine($"-{product.Name}, Quantity: {product.StoredAmount}");
                    }
                    Console.WriteLine("\n\n");
                    Console.WriteLine("Want to change client? (Y/N)");
                    input = Console.ReadKey().KeyChar;
                    if (input == 'Y' || input == 'y')
                    {
                        clientId = getClient(clientController.GetAllClients());
                    }
                    else
                    {
                        clientId = order.CustomerID;
                    }
                    Console.WriteLine("Want to change products? (Y/N)");
                    input = Console.ReadKey().KeyChar;
                    if (input == 'Y' || input == 'y')
                    {
                        productList = getProducts(productController.GetAllProducts());
                    }
                    else
                    {
                        productList = order.Products;
                    }
                    Console.WriteLine("Want to chagne status? (Y/N)");
                    input = Console.ReadKey().KeyChar;
                    if (input == 'Y' || input == 'y')
                    {
                        status = getStatus();
                    }
                    else
                    {
                        status = order.Status;
                    }
                    orderController.UpdateOrder(orderToUpdateId, clientId, productList, status);
                    foreach (Product product in productList)
                    {
                        old_product = productController.GetProductById(product.Id);
                        new_amount = old_product.StoredAmount - product.StoredAmount;
                        productController.UpdateProduct(product.Id, product.Name, product.Price, new_amount);
                    }
                    Console.WriteLine("Order updated succesfully");
                    break;

                case '4':
                    Console.WriteLine("Deleting order");
                    orderToDeleteId = getOrder(orderController.GetAllOrders());
                    old_order = orderController.GetOrderById(orderToDeleteId);
                    foreach (Product product in old_order.Products)
                    {
                        old_product = productController.GetProductById(product.Id);
                        new_amount = old_product.StoredAmount + product.StoredAmount;
                        productController.UpdateProduct(product.Id, product.Name, product.Price, new_amount);
                    }
                    orderController.CancelOrder(orderToDeleteId);
                    Console.WriteLine("Order deleted succesfully");
                    break;

                case '5':
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please choose a number from 1 to 5.");
                    continue;
            }
        }

    }

    private static void clientMenu(ClientController clientController)
    {
        string name;
        string second_name;
        string email;
        int id;
        while (true)
        {
            Console.WriteLine("CLIENT MENU");
            Console.WriteLine("1. View clients");
            Console.WriteLine("2. Add client");
            Console.WriteLine("3. Update client");
            Console.WriteLine("4. Delete client");
            Console.WriteLine("5. Exit");

            char key = Console.ReadKey().KeyChar;
            Console.Clear();

            switch (key)
            {
                case '1':
                    Console.WriteLine("List of clients:");
                    foreach (Client client in clientController.GetAllClients())
                    {
                        Console.WriteLine($"ID: {client.Id}  Name: {client.Name} SecondName: {client.SecondName} Email: {client.Email}");
                    }
                    break;
                case '2':
                    Console.WriteLine("Creating client");
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("Enter name: ");
                            name = Console.ReadLine();
                            Console.WriteLine("Enter second name");
                            second_name = Console.ReadLine();
                            Console.WriteLine("Enter email");
                            email = Console.ReadLine();
                            id = clientController.GetAllClients().OrderByDescending(client => client.Id).FirstOrDefault()?.Id + 1 ?? 0;
                            clientController.CreateClient(id, name, second_name, email);
                            Console.WriteLine("Client created succesfully!");
                            break;
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine("Something went wrong, try again");
                        }
                    }
                    break;
                case '3':
                    Console.WriteLine("Updating Client");
                    while (true)
                    {
                        try
                        {
                            id = getClient(clientController.GetAllClients());
                            Console.WriteLine("Enter new name: ");
                            name = Console.ReadLine();
                            Console.WriteLine("Enter new second name");
                            second_name = Console.ReadLine();
                            Console.WriteLine("Enter new email");
                            email = Console.ReadLine();
                            clientController.UpdateClient(id, name, second_name, email);
                            Console.WriteLine("Client updated succesfully!");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Something went wrong, try again");
                        }
                    }
                    break;
                case '4':
                    Console.WriteLine("Deleting Client");
                    id = getClient(clientController.GetAllClients());
                    clientController.DeleteClient(id);
                    Console.WriteLine("Client deleted succesfully");
                    break;
                case '5':
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please choose a number from 1 to 5.");
                    continue;
            }
        }
    }

    private static void productMenu(ProductController productController)
    {
        string name;
        int id;
        string price;
        string amount;

        while (true)
        {
            Console.WriteLine("CLIENT MENU");
            Console.WriteLine("1. View products");
            Console.WriteLine("2. Add product");
            Console.WriteLine("3. Update product");
            Console.WriteLine("4. Delete product");
            Console.WriteLine("5. Exit");

            char key = Console.ReadKey().KeyChar;
            Console.Clear();

            switch (key)
            {
                case '1':
                    Console.WriteLine("List of products:");
                    foreach (Product product in productController.GetAllProducts())
                    {
                        Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Stored Amount: {product.StoredAmount}");
                    }
                    break;
                case '2':
                    Console.WriteLine("Creating product:");
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("Enter name: ");
                            name = Console.ReadLine();
                            Console.WriteLine("Enter price");
                            price = Console.ReadLine();
                            Decimal.TryParse(price, out decimal price_decimal);
                            Console.WriteLine("Enter stored amount");
                            amount = Console.ReadLine();
                            int.TryParse(amount, out int amount_decimal);
                            id = productController.GetAllProducts().OrderByDescending(product => product.Id).FirstOrDefault()?.Id + 1 ?? 0;
                            productController.CreateProduct(id, name, price_decimal, amount_decimal);
                            Console.WriteLine("Product created succesfully!");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Something went wrong, try again");
                        }
                    }
                    break;
                case '3':
                    Console.WriteLine("Updating product: ");
                    while (true)
                    {
                        try
                        {
                            id = getProduct(productController.GetAllProducts());
                            Console.WriteLine("Enter new name: ");
                            name = Console.ReadLine();
                            Console.WriteLine("Enter new price");
                            price = Console.ReadLine();
                            decimal.TryParse(price, out decimal price_decimal);
                            Console.WriteLine("Enter new stored amount");
                            amount = Console.ReadLine();
                            int.TryParse(amount, out int amount_decimal);
                            productController.UpdateProduct(id, name, price_decimal, amount_decimal);
                            Console.WriteLine("Product updated succesfully!");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Something went wrong, try again");
                        }
                    }
                    break;
                case '4':
                    Console.WriteLine("Deleting product: ");
                    id = getProduct(productController.GetAllProducts());
                    productController.DeleteProduct(id);
                    Console.WriteLine("Product deleted succesfully!");
                    break;
                case '5':
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please choose a number from 1 to 5.");
                    continue;
            }
        }
    }

    private static void Main(string[] args)
    {
        IDatabaseService context = new DatabaseService();

        while (true)
        {
            ClientController clientController = new ClientController(context);
            ProductController productController = new ProductController(context);
            OrderController orderController = new OrderController(context);


            Console.WriteLine("MENU");
            Console.WriteLine("1. Orders");
            Console.WriteLine("2. Clients");
            Console.WriteLine("3. Products");
            Console.WriteLine("4. Exit");

            char key = Console.ReadKey().KeyChar;
            Console.Clear();
            switch (key)
            {
                case '1':
                    orderMenu(orderController, clientController, productController);
                    break;

                case '2':
                    clientMenu(clientController);
                    break;

                case '3':
                    productMenu(productController);
                    break;

                case '4':
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please choose a number from 1 to 4.");
                    continue;

            }
        }
    }
}
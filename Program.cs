// ITCS 3112 Lab 1 - NinerCS Equipment Checkout
// Sairam Veerasurla-801405104, Mariam Muhammad- 801415977
using ITCS_3112_Lab_1_Checkout.Domain;
using ITCS_3112_Lab_1_Checkout.Domain.Enums;
using ITCS_3112_Lab_1_Checkout.Repositories;
using ITCS_3112_Lab_1_Checkout.Services;

namespace ITCS_3112_Lab_1_Checkout;

internal class Program
{
    private static void Main()
    {
        var repository = new InMemoryRepository();
        var clock = new SystemClock();
        var policy = new DefaultPolicy(clock);
        var notifier = new ConsoleNotifier();
        var checkoutService = new CheckoutService(repository, policy, clock, notifier);

        RunMenu(checkoutService);
    }

    private static void RunMenu(CheckoutService checkoutService)
    {
        while (true)
        {
            Console.WriteLine("Welcome to the Equipment Checkout System!");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Add items to inventory");
            Console.WriteLine("2) List available items");
            Console.WriteLine("3) List unavailable items");
            Console.WriteLine("4) Check out item");
            Console.WriteLine("5) Return item");
            Console.WriteLine("6) Show due soon (next 24h)");
            Console.WriteLine("7) Show overdue items");
            Console.WriteLine("8) Search items (optional)");
            Console.WriteLine("9) Mark item LOST");
            Console.WriteLine("0) Exit");
            Console.Write("Enter choice: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    AddItem(checkoutService);
                    break;
                case "2":
                    ListAvailable(checkoutService);
                    break;
                case "3":
                    ListUnavailable(checkoutService);
                    break;
                case "4":
                    CheckoutItem(checkoutService);
                    break;
                case "5":
                    ReturnItem(checkoutService);
                    break;
                case "6":
                    ShowDueSoon(checkoutService);
                    break;
                case "7":
                    ShowOverdue(checkoutService);
                    break;
                case "8":
                    SearchItems(checkoutService);
                    break;
                case "9":
                    MarkLost(checkoutService);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number from 0 to 9.");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void AddItem(CheckoutService checkoutService)
    {
        Console.Write("Enter item ID: ");
        var id = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter item name: ");
        var name = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter category: ");
        var category = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter condition: ");
        var condition = Console.ReadLine() ?? string.Empty;

        if (!Enum.TryParse<EquipmentCategory>(category, true, out var cat))
        {
            Console.WriteLine("Error: Invalid category. Use: Laptop, VrHeadset, Sensor, Other");
            return;
        }
        if (!Enum.TryParse<EquipmentCondition>(condition, true, out var cond))
        {
            Console.WriteLine("Error: Invalid condition. Use: New, Good, Fair, Poor");
            return;
        }

        try
        {
            var item = new Item(id, name, cat, cond);
            checkoutService.AddItem(item);
            Console.WriteLine("Item added successfully.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ListAvailable(CheckoutService checkoutService)
    {
        var available = checkoutService.Catalog.ListAvailable();

        Console.WriteLine("Available Items:");
        if (!available.Any())
        {
            Console.WriteLine("(none)");
            return;
        }

        foreach (var item in available)
        {
            Console.WriteLine($"{item.Id} | {item.Name} | {item.Category}");
        }
    }

    private static void ListUnavailable(CheckoutService checkoutService)
    {
        var unavailable = checkoutService.Catalog.ListUnavailable();

        Console.WriteLine("Unavailable Items:");
        if (!unavailable.Any())
        {
            Console.WriteLine("(none)");
            return;
        }

        foreach (var item in unavailable)
        {
            Console.WriteLine($"{item.Id} | {item.Name} | {item.Category} | {item.Status}");
        }
    }

    private static void CheckoutItem(CheckoutService checkoutService)
    {
        Console.Write("Enter item ID: ");
        var itemId = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter borrower name: ");
        var borrowerName = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter borrower email: ");
        var borrowerEmail = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter due date (YYYY-MM-DD): ");
        var dueDateInput = Console.ReadLine() ?? string.Empty;

        if (!DateTime.TryParse(dueDateInput, out var dueDate))
        {
            Console.WriteLine("Invalid date format.");
            return;
        }

        try
        {
            var borrower = new Borrower(borrowerEmail.Trim(), borrowerName.Trim(), borrowerEmail.Trim());
            var receipt = checkoutService.Checkout(itemId, borrower, dueDate);
            Console.WriteLine("Checkout successful.");
            Console.WriteLine(receipt.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Checkout failed: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ReturnItem(CheckoutService checkoutService)
    {
        Console.Write("Enter item ID: ");
        var itemId = Console.ReadLine() ?? string.Empty;

        try
        {
            var receipt = checkoutService.ReturnItem(itemId);
            Console.WriteLine("Return processed.");
            Console.WriteLine(receipt.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Return failed: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ShowDueSoon(CheckoutService checkoutService)
    {
        var dueSoon = checkoutService.FindDueSoon(TimeSpan.FromHours(24));

        Console.WriteLine("Items Due Soon:");
        if (!dueSoon.Any())
        {
            Console.WriteLine("(none)");
            return;
        }

        foreach (var record in dueSoon)
        {
            var borrower = checkoutService.GetBorrowerById(record.BorrowerId);
            var name = borrower?.Name ?? record.BorrowerId;
            var email = borrower?.Email ?? "";
            Console.WriteLine($"{record.ItemId} | {name} | {email} | Due: {record.DueDate:yyyy-MM-dd}");
        }
    }

    private static void ShowOverdue(CheckoutService checkoutService)
    {
        var overdue = checkoutService.FindOverdue();

        Console.WriteLine("Overdue Items:");
        if (!overdue.Any())
        {
            Console.WriteLine("(none)");
            return;
        }

        foreach (var record in overdue)
        {
            var borrower = checkoutService.GetBorrowerById(record.BorrowerId);
            var name = borrower?.Name ?? record.BorrowerId;
            var email = borrower?.Email ?? "";
            Console.WriteLine($"{record.ItemId} | {name} | {email} | Due: {record.DueDate:yyyy-MM-dd}");
        }
    }

    private static void SearchItems(CheckoutService checkoutService)
    {
        Console.WriteLine("Choose search type:");
        Console.WriteLine("1) Item ID");
        Console.WriteLine("2) Name");
        Console.WriteLine("3) Category");
        Console.Write("Enter your choice (1-3): ");
        var choice = Console.ReadLine();

        Console.Write("Enter search term: ");
        var term = Console.ReadLine() ?? string.Empty;

        IReadOnlyList<Item> results = choice switch
        {
            "1" => checkoutService.Catalog.FindById(term) is { } item ? new List<Item> { item } : new List<Item>(),
            "2" => checkoutService.Catalog.SearchByName(term),
            "3" => Enum.TryParse<EquipmentCategory>(term, true, out var c)
                ? checkoutService.Catalog.SearchByCategory(c)
                : new List<Item>(),
            _ => new List<Item>()
        };

        Console.WriteLine("Search Results:");
        if (!results.Any())
        {
            Console.WriteLine("(none)");
            return;
        }

        foreach (var item in results)
        {
            Console.WriteLine($"{item.Id} | {item.Name} | {item.Category} | {item.Status}");
        }
    }

    private static void MarkLost(CheckoutService checkoutService)
    {
        Console.Write("Enter item ID: ");
        var itemId = Console.ReadLine() ?? string.Empty;

        try
        {
            checkoutService.MarkLost(itemId);
            Console.WriteLine($"Item {itemId} marked LOST!");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}


using ITCS_3112_Lab_1_Checkout.Contracts;
using ITCS_3112_Lab_1_Checkout.Domain;

namespace ITCS_3112_Lab_1_Checkout.Services;

public class ConsoleNotifier : INotifier
{
    public void NotifyDueSoon(CheckoutRecord record, Borrower borrower)
    {
        Console.WriteLine($"[Due Soon] Item {record.ItemId} due {record.DueDate:yyyy-MM-dd} — {borrower}");
    }

    public void NotifyOverdue(CheckoutRecord record, Borrower borrower)
    {
        Console.WriteLine($"[Overdue] Item {record.ItemId} was due {record.DueDate:yyyy-MM-dd} — {borrower}");
    }
}

using ITCS_3112_Lab_1_Checkout.Domain;

namespace ITCS_3112_Lab_1_Checkout.Contracts;

/// <summary>
/// Sends notifications for due-soon or overdue items.
/// Preconditions: record != null, borrower != null.
/// Postconditions: notification attempt is made (implementation-defined).
/// </summary>
public interface INotifier
{
    /// <summary>
    /// Notify borrower that an item is due soon.
    /// Preconditions: record != null, borrower != null.
    /// Postconditions: a due-soon alert is sent/logged.
    /// </summary>
    void NotifyDueSoon(CheckoutRecord record, Borrower borrower);

    /// <summary>
    /// Notify borrower that an item is overdue.
    /// Preconditions: record != null, borrower != null.
    /// Postconditions: an overdue alert is sent/logged.
    /// </summary>
    void NotifyOverdue(CheckoutRecord record, Borrower borrower);
}
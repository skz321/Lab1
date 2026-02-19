using ITCS_3112_Lab_1_Checkout.Domain;

namespace ITCS_3112_Lab_1_Checkout.Contracts;

/// <summary>
/// Coordinates checkouts/returns/lost handling and due/overdue queries.
/// Preconditions: ids non-empty; borrower != null when checking out.
/// Postconditions: updates item status and creates/updates records when operations succeed.
/// </summary>
public interface ICheckoutService
{
    /// <summary>
    /// Provides the catalog query interface.
    /// Preconditions: none.
    /// Postconditions: returns a valid ICatalog instance.
    /// </summary>
    ICatalog Catalog { get; }

    /// <summary>
    /// Adds an item into inventory storage.
    /// Preconditions: item != null; item.Id unique.
    /// Postconditions: item stored and visible in catalog lists.
    /// </summary>
    void AddItem(Item item);

    /// <summary>
    /// Checks out an available item to a borrower and returns a receipt.
    /// Preconditions: itemId non-empty; borrower != null; dueDate valid by policy.
    /// Postconditions: item marked CHECKED_OUT; new active CheckoutRecord created; receipt returned.
    /// </summary>
    Receipt Checkout(string itemId, Borrower borrower, DateTime dueDate);

    /// <summary>
    /// Returns a checked-out item and returns a receipt.
    /// Preconditions: itemId non-empty; item must be CHECKED_OUT with an active record.
    /// Postconditions: item marked AVAILABLE; record updated with return date; receipt returned.
    /// </summary>
    Receipt ReturnItem(string itemId);

    /// <summary>
    /// Marks an item as lost.
    /// Preconditions: itemId non-empty; item must exist.
    /// Postconditions: item marked LOST.
    /// </summary>
    void MarkLost(string itemId);

    /// <summary>
    /// Finds active loans due within the next window (e.g., 24 hours).
    /// Preconditions: window > TimeSpan.Zero.
    /// Postconditions: returns matching active records.
    /// </summary>
    IReadOnlyList<CheckoutRecord> FindDueSoon(TimeSpan window);

    /// <summary>
    /// Finds active loans that are overdue (now > due date).
    /// Preconditions: none.
    /// Postconditions: returns overdue active records.
    /// </summary>
    IReadOnlyList<CheckoutRecord> FindOverdue();
}

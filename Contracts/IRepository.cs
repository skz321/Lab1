using ITCS_3112_Lab_1_Checkout.Domain;

namespace ITCS_3112_Lab_1_Checkout.Contracts;

/// <summary>
/// Low-level storage access for items, borrowers, and checkout records.
/// Preconditions: arguments must be non-null; ids must be non-empty.
/// Postconditions: persists/retrieves data without applying business rules.
/// </summary>
public interface IRepository
{
    // ----- Items -----

    /// <summary>
    /// Adds a new item.
    /// Preconditions: item != null; item.Id non-empty; no duplicate Id exists.
    /// Postconditions: item is stored and retrievable by its Id.
    /// </summary>
    void AddItem(Item item);

    /// <summary>
    /// Gets an item by id.
    /// Preconditions: itemId non-empty.
    /// Postconditions: returns the item or null if not found.
    /// </summary>
    Item? GetItemById(string itemId);

    /// <summary>
    /// Returns all items.
    /// Preconditions: none.
    /// Postconditions: returns a snapshot list of items.
    /// </summary>
    IReadOnlyList<Item> GetAllItems();

    // ----- Borrowers -----

    /// <summary>
    /// Upserts a borrower (add if new, replace if same Id exists).
    /// Preconditions: borrower != null; borrower.Id non-empty.
    /// Postconditions: borrower stored and retrievable by Id.
    /// </summary>
    void UpsertBorrower(Borrower borrower);

    /// <summary>
    /// Gets a borrower by id.
    /// Preconditions: borrowerId non-empty.
    /// Postconditions: returns borrower or null if not found.
    /// </summary>
    Borrower? GetBorrowerById(string borrowerId);

    // ----- Records -----

    /// <summary>
    /// Adds a checkout record.
    /// Preconditions: record != null; record.RecordId non-empty; no duplicate record id exists.
    /// Postconditions: record stored and included in queries.
    /// </summary>
    void AddRecord(CheckoutRecord record);

    /// <summary>
    /// Returns all checkout records.
    /// Preconditions: none.
    /// Postconditions: returns a snapshot list of records.
    /// </summary>
    IReadOnlyList<CheckoutRecord> GetAllRecords();

    /// <summary>
    /// Returns the active (not returned) record for an item, if any.
    /// Preconditions: itemId non-empty.
    /// Postconditions: returns active record or null.
    /// </summary>
    CheckoutRecord? GetActiveRecordForItem(string itemId);

    /// <summary>
    /// Returns all records for a borrower.
    /// Preconditions: borrowerId non-empty.
    /// Postconditions: returns list (may be empty).
    /// </summary>
    IReadOnlyList<CheckoutRecord> GetRecordsByBorrower(string borrowerId);
}

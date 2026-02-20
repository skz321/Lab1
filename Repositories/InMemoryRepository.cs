using System.Collections.Concurrent;
using ITCS_3112_Lab_1_Checkout.Domain;
using ITCS_3112_Lab_1_Checkout.Domain.Enums;

namespace ITCS_3112_Lab_1_Checkout.Repositories;

public class InMemoryRepository : Contracts.IRepository
{
    private readonly ConcurrentDictionary<string, Item> _items = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, Borrower> _borrowers = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<CheckoutRecord> _records = new();

    public void AddItem(Item item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (!_items.TryAdd(item.Id, item))
            throw new ArgumentException($"An item with ID '{item.Id}' already exists.");
    }

    public Item? GetItemById(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId)) return null;
        _items.TryGetValue(itemId, out var item);
        return item;
    }

    public IReadOnlyList<Item> GetAllItems()
    {
        return _items.Values.ToList();
    }

    public void UpsertBorrower(Borrower borrower)
    {
        if (borrower == null) throw new ArgumentNullException(nameof(borrower));
        _borrowers[borrower.Id] = borrower;
    }

    public Borrower? GetBorrowerById(string borrowerId)
    {
        if (string.IsNullOrWhiteSpace(borrowerId)) return null;
        _borrowers.TryGetValue(borrowerId, out var borrower);
        return borrower;
    }

    public void AddRecord(CheckoutRecord record)
    {
        if (record == null) throw new ArgumentNullException(nameof(record));
        _records.Add(record);
    }

    public IReadOnlyList<CheckoutRecord> GetAllRecords()
    {
        return _records.ToList();
    }

    public CheckoutRecord? GetActiveRecordForItem(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId)) return null;
        return _records
            .Where(r => r.ItemId.Equals(itemId, StringComparison.OrdinalIgnoreCase) && !r.IsReturned)
            .OrderBy(r => r.CheckoutDate)
            .LastOrDefault();
    }

    public IReadOnlyList<CheckoutRecord> GetRecordsByBorrower(string borrowerId)
    {
        if (string.IsNullOrWhiteSpace(borrowerId)) return Array.Empty<CheckoutRecord>();
        return _records
            .Where(r => r.BorrowerId.Equals(borrowerId, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

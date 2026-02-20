using ITCS_3112_Lab_1_Checkout.Contracts;
using ITCS_3112_Lab_1_Checkout.Domain;

namespace ITCS_3112_Lab_1_Checkout.Services;

public sealed class CheckoutService : ICheckoutService
{
    private readonly IRepository _repository;
    private readonly IPolicy _policy;
    private readonly IClock _clock;
    private readonly INotifier _notifier;
    private readonly Catalog _catalog;

    public CheckoutService(IRepository repository, IPolicy policy, IClock clock, INotifier notifier)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
        _catalog = new Catalog(repository);
    }

    public ICatalog Catalog => _catalog;

    public void AddItem(Item item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _repository.AddItem(item);
    }

    public Receipt Checkout(string itemId, Borrower borrower, DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(itemId))
            throw new ArgumentException("Item ID is required.", nameof(itemId));
        if (borrower == null) throw new ArgumentNullException(nameof(borrower));

        var item = _repository.GetItemById(itemId);
        if (item == null)
            throw new InvalidOperationException($"Item '{itemId}' not found.");

        if (!_policy.CanCheckout(item))
            throw new InvalidOperationException($"Item '{itemId}' cannot be checked out (status: {item.Status}).");

        var now = _clock.Now;
        if (!_policy.IsValidDueDate(now, dueDate))
            throw new ArgumentException("Due date must be in the future and within the allowed loan period.");

        _repository.UpsertBorrower(borrower);

        var recordId = Guid.NewGuid().ToString("N");
        var record = new CheckoutRecord(recordId, item.Id, borrower.Id, now, dueDate);
        _repository.AddRecord(record);

        item.MarkCheckedOut();

        return new Receipt(
            "Checkout",
            $"Item {item.Id} checked out to {borrower.Name}. Due: {dueDate:yyyy-MM-dd}.",
            now);
    }

    public Receipt ReturnItem(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId))
            throw new ArgumentException("Item ID is required.", nameof(itemId));

        var item = _repository.GetItemById(itemId);
        if (item == null)
            throw new InvalidOperationException($"Item '{itemId}' not found.");

        var record = _repository.GetActiveRecordForItem(itemId);
        if (record == null)
            throw new InvalidOperationException($"No active checkout found for item '{itemId}'.");

        var now = _clock.Now;
        item.MarkReturned();
        record.MarkReturned(now);

        return new Receipt(
            "Return",
            $"Item {item.Id} returned on {now:yyyy-MM-dd}.",
            now);
    }

    public void MarkLost(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId))
            throw new ArgumentException("Item ID is required.", nameof(itemId));

        var item = _repository.GetItemById(itemId);
        if (item == null)
            throw new ArgumentException($"Item '{itemId}' not found.");

        item.MarkLost();
    }

    public IReadOnlyList<CheckoutRecord> FindDueSoon(TimeSpan window)
    {
        if (window <= TimeSpan.Zero)
            throw new ArgumentException("Window must be positive.", nameof(window));

        var now = _clock.Now;
        var end = now + window;
        return _repository.GetAllRecords()
            .Where(r => !r.IsReturned && r.DueDate >= now && r.DueDate <= end)
            .ToList();
    }

    public IReadOnlyList<CheckoutRecord> FindOverdue()
    {
        var now = _clock.Now;
        return _repository.GetAllRecords()
            .Where(r => !r.IsReturned && r.DueDate < now)
            .ToList();
    }

    public Borrower? GetBorrowerById(string borrowerId)
    {
        return _repository.GetBorrowerById(borrowerId);
    }
}

namespace ITCS_3112_Lab_1_Checkout.Domain;

public sealed class CheckoutRecord
{
    public string RecordId { get; }
    public string ItemId { get; }
    public string BorrowerId { get; }
    public DateTime CheckoutDate { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnDate { get; private set; }

    public bool IsReturned => ReturnDate.HasValue;

    public CheckoutRecord(
        string recordId,
        string itemId,
        string borrowerId,
        DateTime checkoutDate,
        DateTime dueDate)
    {
        RecordId = recordId?.Trim() ?? throw new ArgumentNullException(nameof(recordId));
        ItemId = itemId?.Trim() ?? throw new ArgumentNullException(nameof(itemId));
        BorrowerId = borrowerId?.Trim() ?? throw new ArgumentNullException(nameof(borrowerId));

        if (RecordId.Length == 0) throw new ArgumentException("RecordId cannot be empty.", nameof(recordId));
        if (ItemId.Length == 0) throw new ArgumentException("ItemId cannot be empty.", nameof(itemId));
        if (BorrowerId.Length == 0) throw new ArgumentException("BorrowerId cannot be empty.", nameof(borrowerId));

        CheckoutDate = checkoutDate;
        DueDate = dueDate;
        if (dueDate <= checkoutDate)
            throw new ArgumentException("Due date must be after checkout date.", nameof(dueDate));
    }

    public void MarkReturned(DateTime returnDate)
    {
        if (IsReturned) throw new InvalidOperationException("Record already returned.");
        if (returnDate < CheckoutDate) throw new ArgumentException("Return date cannot be before checkout date.");
        ReturnDate = returnDate;
    }
}
namespace ITCS_3112_Lab_1_Checkout.Domain;

public sealed class Receipt
{
    public string Title { get; }
    public string Message { get; }
    public DateTime Timestamp { get; }

    public Receipt(string title, string message, DateTime timestamp)
    {
        Title = title?.Trim() ?? throw new ArgumentNullException(nameof(title));
        Message = message?.Trim() ?? throw new ArgumentNullException(nameof(message));
        Timestamp = timestamp;

        if (Title.Length == 0) throw new ArgumentException("Title cannot be empty.", nameof(title));
        if (Message.Length == 0) throw new ArgumentException("Message cannot be empty.", nameof(message));
    }

    public override string ToString()
        => $"[{Timestamp:yyyy-MM-dd HH:mm}] {Title}\n{Message}";
}
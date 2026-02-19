namespace ITCS_3112_Lab_1_Checkout.Domain;

public sealed class Borrower
{
    public string Id { get; }
    public string Name { get; }
    public string Email { get; }

    public Borrower(string id, string name, string email)
    {
        Id = id?.Trim() ?? throw new ArgumentNullException(nameof(id));
        Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
        Email = email?.Trim() ?? throw new ArgumentNullException(nameof(email));

        if (Id.Length == 0) throw new ArgumentException("Borrower id cannot be empty.", nameof(id));
        if (Name.Length == 0) throw new ArgumentException("Borrower name cannot be empty.", nameof(name));
        if (Email.Length == 0) throw new ArgumentException("Borrower email cannot be empty.", nameof(email));
    }

    public override string ToString() => $"{Name} ({Email})";
}
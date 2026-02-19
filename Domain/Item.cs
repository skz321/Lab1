using ITCS_3112_Lab_1_Checkout.Domain.Enums;

namespace ITCS_3112_Lab_1_Checkout.Domain;

public sealed class Item
{
    public string Id { get; }
    public string Name { get; private set; }
    public EquipmentCategory Category { get; private set; }
    public EquipmentCondition Condition { get; private set; }
    public ItemStatus Status { get; private set; }

    public Item(string id, string name, EquipmentCategory category, EquipmentCondition condition)
    {
        Id = id?.Trim() ?? throw new ArgumentNullException(nameof(id));
        if (Id.Length == 0) throw new ArgumentException("Item id cannot be empty.", nameof(id));

        Name = (name?.Trim() ?? throw new ArgumentNullException(nameof(name)));
        if (Name.Length == 0) throw new ArgumentException("Item name cannot be empty.", nameof(name));

        Category = category;
        Condition = condition;
        Status = ItemStatus.AVAILABLE;
    }

    public void UpdateDetails(string name, EquipmentCategory category, EquipmentCondition condition)
    {
        name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("Item name cannot be empty.", nameof(name));

        Name = name;
        Category = category;
        Condition = condition;
    }

    public void MarkCheckedOut()
    {
        if (Status != ItemStatus.AVAILABLE)
            throw new InvalidOperationException($"Cannot check out item {Id} because status is {Status}.");
        Status = ItemStatus.CHECKED_OUT;
    }

    public void MarkReturned()
    {
        if (Status != ItemStatus.CHECKED_OUT)
            throw new InvalidOperationException($"Cannot return item {Id} because status is {Status}.");
        Status = ItemStatus.AVAILABLE;
    }

    public void MarkLost()
    {
        if (Status == ItemStatus.LOST) return;
        Status = ItemStatus.LOST;
    }

    public override string ToString() => $"{Id} | {Name} | {Category} | {Status}";
}
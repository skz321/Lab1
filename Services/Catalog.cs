using ITCS_3112_Lab_1_Checkout.Contracts;
using ITCS_3112_Lab_1_Checkout.Domain;
using ITCS_3112_Lab_1_Checkout.Domain.Enums;

namespace ITCS_3112_Lab_1_Checkout.Services;

internal sealed class Catalog : ICatalog
{
    private readonly IRepository _repository;

    public Catalog(IRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IReadOnlyList<Item> ListAvailable()
    {
        return _repository.GetAllItems()
            .Where(i => i.Status == ItemStatus.AVAILABLE)
            .ToList();
    }

    public IReadOnlyList<Item> ListUnavailable()
    {
        return _repository.GetAllItems()
            .Where(i => i.Status == ItemStatus.CHECKED_OUT || i.Status == ItemStatus.LOST)
            .ToList();
    }

    public Item? FindById(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId)) return null;
        return _repository.GetItemById(itemId);
    }

    public IReadOnlyList<Item> SearchByName(string namePart)
    {
        if (string.IsNullOrWhiteSpace(namePart)) return Array.Empty<Item>();
        return _repository.GetAllItems()
            .Where(i => i.Name.Contains(namePart, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public IReadOnlyList<Item> SearchByCategory(EquipmentCategory category)
    {
        return _repository.GetAllItems()
            .Where(i => i.Category == category)
            .ToList();
    }
}

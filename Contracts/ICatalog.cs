using ITCS_3112_Lab_1_Checkout.Domain.Enums;
using ITCS_3112_Lab_1_Checkout.Domain;

namespace ITCS_3112_Lab_1_Checkout.Contracts;

/// <summary>
/// High-level read-only queries for inventory (filtered views).
/// Preconditions: none.
/// Postconditions: returns filtered results; does not mutate items.
/// </summary>
public interface ICatalog
{
    /// <summary>
    /// Lists all available items (Status == AVAILABLE).
    /// </summary>
    IReadOnlyList<Item> ListAvailable();

    /// <summary>
    /// Lists all unavailable items (Status == CHECKED_OUT or LOST).
    /// </summary>
    IReadOnlyList<Item> ListUnavailable();

    /// <summary>
    /// Searches items by id (exact match).
    /// Preconditions: itemId non-empty.
    /// Postconditions: returns item or null.
    /// </summary>
    Item? FindById(string itemId);

    /// <summary>
    /// Searches items by name (case-insensitive contains).
    /// Preconditions: namePart non-empty.
    /// Postconditions: returns matches (may be empty).
    /// </summary>
    IReadOnlyList<Item> SearchByName(string namePart);

    /// <summary>
    /// Searches items by category.
    /// Postconditions: returns matches (may be empty).
    /// </summary>
    IReadOnlyList<Item> SearchByCategory(EquipmentCategory category);
}

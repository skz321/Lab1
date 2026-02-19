using ITCS_3112_Lab_1_Checkout.Domain;

namespace ITCS_3112_Lab_1_Checkout.Contracts;

/// <summary>
/// Defines business rules for whether an item can be checked out and how long.
/// Preconditions: item != null.
/// Postconditions: method results reflect policy rules without changing system state.
/// </summary>
public interface IPolicy
{
    /// <summary>
    /// Determines if an item can be checked out at this moment.
    /// Preconditions: item != null.
    /// Postconditions: returns true only if checkout is allowed by policy.
    /// </summary>
    bool CanCheckout(Item item);

    /// <summary>
    /// Validates a due date for a checkout.
    /// Preconditions: now and dueDate are valid DateTime values.
    /// Postconditions: returns true only if the dueDate is acceptable.
    /// </summary>
    bool IsValidDueDate(DateTime now, DateTime dueDate);
}
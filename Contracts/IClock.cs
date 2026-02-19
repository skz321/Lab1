namespace ITCS_3112_Lab_1_Checkout.Contracts;

/// <summary>
/// Provides the current time.
/// Preconditions: none.
/// Postconditions: returns a DateTime representing "now" for the system.
/// </summary>
public interface IClock
{
    /// <returns>The current DateTime (system time or injected test time).</returns>
    DateTime Now { get; }
}
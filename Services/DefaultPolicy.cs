using ITCS_3112_Lab_1_Checkout.Contracts;
using ITCS_3112_Lab_1_Checkout.Domain;
using ITCS_3112_Lab_1_Checkout.Domain.Enums;

namespace ITCS_3112_Lab_1_Checkout.Services;

public class DefaultPolicy : IPolicy
{
    private readonly IClock _clock;
    private static readonly TimeSpan MaxLoanPeriod = TimeSpan.FromDays(14);

    public DefaultPolicy(IClock clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public bool CanCheckout(Item item)
    {
        return item != null && item.Status == ItemStatus.AVAILABLE;
    }

    public bool IsValidDueDate(DateTime now, DateTime dueDate)
    {
        return dueDate > now && (dueDate - now) <= MaxLoanPeriod;
    }
}

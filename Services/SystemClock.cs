namespace ITCS_3112_Lab_1_Checkout.Services;

public class SystemClock : Contracts.IClock
{
    public DateTime Now => DateTime.Now;
}

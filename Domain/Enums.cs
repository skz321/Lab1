namespace ITCS_3112_Lab_1_Checkout.Domain.Enums;

public enum ItemStatus
{
    AVAILABLE,
    CHECKED_OUT,
    LOST
}

public enum EquipmentCategory
{
    Laptop,
    VrHeadset,
    Sensor,
    Other
}

public enum EquipmentCondition
{
    New,
    Good,
    Fair,
    Poor
}
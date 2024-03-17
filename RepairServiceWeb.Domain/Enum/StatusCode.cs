namespace RepairServiceWeb.Domain.Enum
{
    public enum StatusCode
    {
        AccessoriesNotFound = 0,
        SuppliersNotFound,
        OrderAccessoriesNotFound,
        ClientsNotFound,
        RoleNotFound,
        StaffNotFound,
        DevicesNotFound,
        RepairsNotFound,

        OK = 200,
        InternalServerError = 500
    }
}

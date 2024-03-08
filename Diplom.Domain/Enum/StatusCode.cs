namespace Diplom.Domain.Enum
{
    public enum StatusCode
    {
        AccessoriesNotFound = 0,
        SuppliersNotFound,
        OrderAccessoriesNotFound ,
        ClientsNotFound,
        RoleNotFound,
        StaffNotFound,
        DevicesNotFound,

        OK = 200,
        InternalServerError = 500
    }
}

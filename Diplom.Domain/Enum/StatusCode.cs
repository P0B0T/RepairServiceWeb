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

        OK = 200,
        InternalServerError = 500
    }
}

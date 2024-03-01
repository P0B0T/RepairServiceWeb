﻿namespace Diplom.Domain.Enum
{
    public enum StatusCode
    {
        AccessoriesNotFound = 0,
        SuppliersNotFound = 1,
        OrderAccessoriesNotFound = 2,
        ClientsNotFound = 3,
        RoleNotFound = 4,

        OK = 200,
        InternalServerError = 500
    }
}

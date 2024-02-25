using System;
using System.Collections.Generic;

namespace Diplom.Domain.Entity;

public partial class OrderAccessory
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int AccessoriesId { get; set; }

    public string Count { get; set; } = null!;

    public decimal Cost { get; set; }

    public DateOnly DateOrder { get; set; }

    public string StatusOrder { get; set; } = null!;

    public virtual Accessory Accessories { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;
}

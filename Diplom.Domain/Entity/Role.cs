using System;
using System.Collections.Generic;

namespace Diplom.Domain.Entity;

public partial class Role
{
    public int Id { get; set; }

    public string Role1 { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}

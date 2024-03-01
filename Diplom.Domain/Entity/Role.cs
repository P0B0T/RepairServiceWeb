using System;

namespace Diplom.Domain.Entity;

public partial class Role
{
    public int Id { get; set; }

    public string Role1 { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}

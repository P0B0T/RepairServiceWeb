﻿using RepairServiceWeb.Domain.Interfaces;

namespace RepairServiceWeb.Domain.Entity;

public partial class Client : IUser
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string FullName => $"{Surname} {Name} {Patronymic}".Trim();

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public virtual ICollection<OrderAccessory> OrderAccessories { get; set; } = new List<OrderAccessory>();

    public virtual Role Role { get; set; } = null!;
}

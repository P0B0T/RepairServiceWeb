using System;
using System.Collections.Generic;

namespace Diplom.Domain.Entity;

public partial class Staff
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int? Experiance { get; set; }

    public string Post { get; set; } = null!;

    public decimal Salary { get; set; }

    public DateOnly DateOfEmployment { get; set; }

    public string? Photo { get; set; }

    public int RoleId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Repair> Repairs { get; set; } = new List<Repair>();

    public virtual Role Role { get; set; } = null!;
}

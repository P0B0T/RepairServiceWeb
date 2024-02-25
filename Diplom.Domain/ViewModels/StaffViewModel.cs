using Diplom.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Domain.ViewModels
{
    public class StaffViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string? Patronymic { get; set; }

        public string Fullname => $"{Surname} {Name} {Patronymic}".Trim();

        public int? Experiance { get; set; }

        public string Post { get; set; } = null!;

        public decimal Salary { get; set; }

        public DateOnly Date_of_employment { get; set; }

        public string? Photo { get; set; }

        public int RoleId { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public virtual Role Role { get; set; } = null!;
    }
}

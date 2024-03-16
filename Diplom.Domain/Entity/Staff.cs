using Diplom.Domain.Interfaces;

namespace Diplom.Domain.Entity;

public partial class Staff : IUser
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string FullName => $"{Surname} {Name} {Patronymic}".Trim();

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

    public string ExperianceWithWord
    {
        get
        {
            if (Experiance.HasValue)
            {
                int lastDigit = Experiance.Value % 10;

                if (Experiance.Value > 10 && Experiance.Value < 20)
                    return $"{Experiance} лет";
                else if (lastDigit == 1)
                    return $"{Experiance} год";
                else if (lastDigit > 1 && lastDigit < 5)
                    return $"{Experiance} года";
                else
                    return $"{Experiance} лет";
            }
            else
                return "";
        }
    }
}

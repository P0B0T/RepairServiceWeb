using Diplom.Domain.Entity;

namespace Diplom.Domain.ViewModels
{
    public class ClientsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string? Patronymic { get; set; }

        public string FullName => $"{Surname} {Name} {Patronymic}".Trim();

        public string Address { get; set; } = null!;

        public string Phone_number { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int RoleId { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public virtual Role Role { get; set; } = null!;
    }
}

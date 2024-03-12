using Diplom.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Diplom.Domain.ViewModels
{
    public class ClientsViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Имя:")]
        public string Name { get; set; } = null!;

        [Display(Name = "Фамилия:")]
        public string Surname { get; set; } = null!;

        [Display(Name = "Отчество:")]
        public string? Patronymic { get; set; }

        public string FullName => $"{Surname} {Name} {Patronymic}".Trim();

        [Display(Name = "Адрес:")]
        public string Address { get; set; } = null!;

        [Display(Name = "Номер телефона:")]
        [RegularExpression(@"^(\+7|8)\(\d{3}\)\d{3}-\d{2}-\d{2}$", ErrorMessage = "Неверный формат номера телефона")]
        public string Phone_number { get; set; } = null!;

        [Display(Name = "Email:")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Неверный формат электронной почты")]
        public string Email { get; set; } = null!;

        public int RoleId { get; set; }

        [Display(Name = "Логин:")]
        public string Login { get; set; } = null!;

        [Display(Name = "Пароль:")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,}$",
            ErrorMessage = "Пароль должен содержать как минимум 10 символов, включая хотя бы одну заглавную букву (en), одну строчную букву (en), одну цифру и один специальный символ.")]
        public string Password { get; set; } = null!;

        [Display(Name = "Повторите пароль:")]
        public string RepeatPassword { get; set; } = null!;

        public virtual Role? Role { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != RepeatPassword)
                yield return new ValidationResult("Пароли должны совпадать.", new[] { nameof(RepeatPassword) });
        }
    }
}

using RepairServiceWeb.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace RepairServiceWeb.Domain.ViewModels
{
    public class StaffViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Имя:")]
        public string Name { get; set; } = null!;

        [Display(Name = "Фамилия:")]
        public string Surname { get; set; } = null!;

        [Display(Name = "Отчество:")]
        public string? Patronymic { get; set; }

        public string FullName => $"{Surname} {Name} {Patronymic}".Trim();

        [Display(Name = "Стаж:")]
        [Range(1, 80, ErrorMessage = "Стаж сотрудника должен быть больше 0 и меньше 80.")]
        public int? Experiance { get; set; }

        [Display(Name = "Должность:")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Должность сотрудника должна быть от 6 до 50 символов.")]
        public string Post { get; set; } = null!;

        [Display(Name = "Зар. плата:")]
        [Range(1, 1000000, ErrorMessage = "Зар. плата сотрудника должна быть больше 0 и меньше 1 000 000.")]
        public decimal Salary { get; set; }

        [Display(Name = "Дата приёма на работу:")]
        public DateOnly Date_of_employment { get; set; }

        [Display(Name = "Фото:")]
        public string? Photo { get; set; }

        [Display(Name = "Роль:")]
        public int RoleId { get; set; }

        [Display(Name = "Логин:")]
        public string Login { get; set; } = null!;

        [Display(Name = "Пароль:")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?])[A-Za-z\d@$!%*?]{10,}$",
            ErrorMessage = "Пароль должен содержать как минимум 10 символов, включая хотя бы одну заглавную букву (en), одну строчную букву (en), одну цифру и один специальный символ (& - не использовать).")]
        public string Password { get; set; } = null!;

        [Display(Name = "Повторите пароль:")]
        public string RepeatPassword { get; set; } = null!;

        public virtual Role? Role { get; set; }

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            DateOnly startOfYear = DateOnly.FromDateTime(DateTime.Today);
            startOfYear = new DateOnly(startOfYear.Year, 1, 1);

            if (Password != RepeatPassword)
                errors.Add(new ValidationResult("Пароли должны совпадать.", new[] { nameof(RepeatPassword) }));

            if (Date_of_employment < startOfYear || Date_of_employment > DateOnly.FromDateTime(DateTime.Today))
                errors.Add(new ValidationResult("Дата приёма на работу должна быть не позднее текущего года и не раньше сегодняшней даты.", new[] { nameof(Date_of_employment) }));

            if (Password.Contains('&'))
                errors.Add(new ValidationResult("Символ '&' не должен содержаться в пароле.", new[] { nameof(Password) }));

            return errors;
        }
    }
}

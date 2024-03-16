using Diplom.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Diplom.Domain.ViewModels
{
    public class DevicesViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Модель:")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Модель должна быть от 5 до 50 символов.")]
        public string ModelDevice { get; set; } = null!;

        [Display(Name = "Производитель:")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название компании производителя должно быть от 2 до 50 символов.")]
        public string Manufacturer { get; set; } = null!;

        [Display(Name = "Тип:")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Тип устройства должен быть от 5 до 50 символов.")]
        public string Type { get; set; } = null!;

        [Display(Name = "Год производства:")]
        public short Year_of_release { get; set; }

        [Display(Name = "Серийный номер:")]
        public int? Serial_number { get; set; }

        [Display(Name = "Клиент:")]
        public int ClientId { get; set; }

        [Display(Name = "Фото:")]
        public string? Photo { get; set; }

        public virtual Client? Client { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Year_of_release > DateOnly.FromDateTime(DateTime.Today).Year || Year_of_release < 1950)
                yield return (new ValidationResult("Год производства должен быть не раньше текущего года и не позднее 1950 года.", new[] { nameof(Year_of_release) }));
        }
    }
}

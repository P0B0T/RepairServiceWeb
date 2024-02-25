using Diplom.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Diplom.Domain.ViewModels
{
    public class AccessoriesViewModel /*: IValidatableObject*/
    {
        public int Id { get; set; }

        [Display(Name = "Название:")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Название комплектующего должно быть от 5 до 50 символов.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Описание:")]
        public string? Description { get; set; }

        [Display(Name = "Производитель:")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название компании производителя должно быть от 2 до 50 символов.")]
        public string Manufacturer { get; set; } = null!;

        [Display(Name = "Стоимость:")]
        [Range(1, 1000000, ErrorMessage = "Стоимость ремонта должна быть больше 0 и меньше 1 000 000.")]
        public decimal Cost { get; set; }

        [Display(Name = "Поставщик:")]
        public int SupplierId { get; set; }

        public virtual Supplier? Supplier { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    List<ValidationResult> errors = new List<ValidationResult>();

        //    //if (Name.Trim().Length < 5)
        //    //    errors.Add(new ValidationResult("В конце и в начале не должно быть пробелов.", new[] { nameof(Name) }));

        //    //if (Manufacturer.Trim().Length < 2)
        //    //    errors.Add(new ValidationResult("В конце и в начале не должно быть пробелов.", new[] { nameof(Manufacturer) }));

        //    //if (String.IsNullOrWhiteSpace(Name))
        //    //    errors.Add(new ValidationResult("Хуй", new[] { nameof(Name) }));

        //    //if (String.IsNullOrWhiteSpace(Description))
        //    //    errors.Add(new ValidationResult("Хуй", new[] { nameof(Description) }));

        //    //if (String.IsNullOrWhiteSpace(Manufacturer))
        //    //    errors.Add(new ValidationResult("Хуй", new[] { nameof(Manufacturer) }));

        //    return errors;
        //}
    }
}

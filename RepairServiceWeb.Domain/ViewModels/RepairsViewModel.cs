using RepairServiceWeb.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace RepairServiceWeb.Domain.ViewModels
{
    public class RepairsViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Устройство:")]
        public int DeviceId { get; set; }

        [Display(Name = "Сотрудник:")]
        public int StaffId { get; set; }

        [Display(Name = "Дата поступления:")]
        public DateOnly Date_of_admission { get; set; }

        [Display(Name = "Дата окончания:")]
        public DateOnly End_date { get; set; }

        [Display(Name = "Стоимость:")]
        [Range(1, 1000000, ErrorMessage = "Стоимость ремонта должна быть больше 0 и меньше 1 000 000.")]
        public decimal Cost { get; set; }

        [Display(Name = "Описание проблемы:")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Описание проблемы должно быть от 10 до 500 символов.")]
        public string Description_of_problem { get; set; } = null!;

        [Display(Name = "Описание проделанной работы:")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Описание проделанной работы должно быть от 10 до 500 символов.")]
        public string Descriprion_of_work_done { get; set; } = null!;

        public virtual Staff? Staff { get; set; }

        public virtual Device? Device { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (Date_of_admission > DateOnly.FromDateTime(DateTime.Today))
                errors.Add(new ValidationResult("Дата поступления должна быть не раньше сегодняшней даты.", new[] { nameof(Date_of_admission) }));

            if (End_date < Date_of_admission || End_date > DateOnly.FromDateTime(DateTime.Today))
                errors.Add(new ValidationResult("Дата окончания должна быть не раньше даты поступления и не позднее сегодняшней даты.", new[] { nameof(End_date) }));

            return errors;
        }
    }
}

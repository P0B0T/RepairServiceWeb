using System.ComponentModel.DataAnnotations;

namespace RepairServiceWeb.Domain.ViewModels
{
    public class SuppliersViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название компании:")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название компании должно быть от 2 до 50 символов.")]
        public string Company_name { get; set; } = null!;

        [Display(Name = "Контактное лицо:")]
        public string? Contact_person { get; set; }

        [Display(Name = "Номер телефона:")]
        [RegularExpression(@"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$", ErrorMessage = "Неверный формат номера телефона")]
        public string? Phone_number { get; set; }

        [Display(Name = "Адрес:")]
        public string Address { get; set; } = null!;

        [Display(Name = "Электронная почта:")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Неверный формат электронной почты")]
        public string Email { get; set; } = null!;
    }
}

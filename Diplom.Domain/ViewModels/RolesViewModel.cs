using System.ComponentModel.DataAnnotations;

namespace Diplom.Domain.ViewModels
{
    public class RolesViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название роли:")]
        public string Role1 { get; set; } = null!;

        [Display(Name = "Описание:")]
        public string? Description { get; set; }
    }
}

﻿using RepairServiceWeb.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace RepairServiceWeb.Domain.ViewModels
{
    public class OrderAccessoriesViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Клиент:")]
        public int ClientId { get; set; }

        [Display(Name = "Запчасть:")]
        public int AccessoriesId { get; set; }

        [Display(Name = "Количество:")]
        [RegularExpression(@"([1-9][0-9]*\s\w*|[1-9][0-9]*)", ErrorMessage = "Количество должно содержать положительное целое число.")]
        public string Count { get; set; } = null!;

        [Display(Name = "Стоимость:")]
        [Range(1, 1000000, ErrorMessage = "Стоимость заказа должна быть больше 0 и меньше 1 000 000.")]
        public decimal Cost { get; set; }

        [Display(Name = "Дата заказа:")]
        public DateOnly Date_order { get; set; }

        [Display(Name = "Статус:")]
        public string Status_order { get; set; } = null!;

        public virtual Client? Client { get; set; } = null!;

        public virtual Accessory? Accessories { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date_order > DateOnly.FromDateTime(DateTime.Today))
                yield return new ValidationResult("Дата заказа должна быть не раньше сегодняшней даты.", new[] { nameof(Date_order) });
        }
    }
}

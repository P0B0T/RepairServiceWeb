using Diplom.Domain.Entity;

namespace Diplom.Domain.ViewModels
{
    public class OrderAccessoriesViewModel
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int AccessoriesId { get; set; }

        public string Count { get; set; } = null!;

        public decimal Cost { get; set; }

        public DateOnly Date_order { get; set; }

        public string Status_order { get; set; } = null!;

        public virtual Client Client{ get; set; } = null!;

        public virtual Accessory Accessories{ get; set; } = null!;
    }
}

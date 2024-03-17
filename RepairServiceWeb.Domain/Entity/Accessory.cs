namespace RepairServiceWeb.Domain.Entity;

public partial class Accessory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Manufacturer { get; set; } = null!;

    public decimal Cost { get; set; }

    public int SupplierId { get; set; }

    public virtual ICollection<OrderAccessory> OrderAccessories { get; set; } = new List<OrderAccessory>();

    public virtual Supplier Supplier { get; set; } = null!;
}

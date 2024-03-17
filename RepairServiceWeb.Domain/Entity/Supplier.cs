namespace RepairServiceWeb.Domain.Entity;

public partial class Supplier
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? ContactPerson { get; set; }

    public string? PhoneNumber { get; set; }

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Accessory> Accessories { get; set; } = new List<Accessory>();
}

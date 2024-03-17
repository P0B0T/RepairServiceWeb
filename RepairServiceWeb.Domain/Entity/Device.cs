namespace RepairServiceWeb.Domain.Entity;

public partial class Device
{
    public int Id { get; set; }

    public string Model { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public string Type { get; set; } = null!;

    public short YearOfRelease { get; set; }

    public int? SerialNumber { get; set; }

    public int ClientId { get; set; }

    public string? Photo { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Repair> Repairs { get; set; } = new List<Repair>();
}

namespace RepairServiceWeb.Domain.Entity;

public partial class Repair
{
    public int Id { get; set; }

    public int DeviceId { get; set; }

    public int StaffId { get; set; }

    public DateOnly DateOfAdmission { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal Cost { get; set; }

    public string DescriptionOfProblem { get; set; } = null!;

    public string DescriprionOfWorkDone { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Device Device { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}

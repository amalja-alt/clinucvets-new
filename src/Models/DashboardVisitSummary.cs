namespace ClinicVets.Models;

public class DashboardVisitSummary
{
    public int VisitId { get; init; }
    public DateTime VisitDateTime { get; init; }
    public string PetName { get; init; } = string.Empty;
    public string OwnerName { get; init; } = string.Empty;
    public string VeterinarianName { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
    public string Diagnosis { get; init; } = string.Empty;
}

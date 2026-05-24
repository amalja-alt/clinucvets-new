namespace ClinicVets.Models;

public class Treatment
{
    public int Id { get; init; }
    public int VisitId { get; set; }
    public int MedicineId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal MedicinePrice { get; set; }
    public decimal TotalPrice => Quantity * MedicinePrice;
    public DateTime TreatmentDate { get; set; } = DateTime.Now;
}
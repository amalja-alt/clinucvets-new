namespace ClinicVets.Models;

public class Visit
{
    public int Id { get; init; }
    public int AnimalId { get; set; }
    public int VeterinarianId { get; set; }
    public DateTime VisitDateTime { get; set; } = DateTime.Now;
    public string Reason { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public List<Medicine> MedicinesGiven { get; } = [];
    public decimal BaseVisitPrice { get; set; } = 150m;

    public decimal TotalPrice => BaseVisitPrice + MedicinesGiven.Sum(medicine => medicine.Price);
}

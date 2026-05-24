using ClinicVets.Models;

namespace ClinicVets.Services;

public class VaccineAlertService
{
    public bool IsVaccineDue(Animal animal)
    {
        return animal.LastVaccinationDate.AddYears(1) <= DateOnly.FromDateTime(DateTime.Today);
    }

    public string GetAlertMessage(Animal animal)
    {
        return IsVaccineDue(animal)
            ? "Yearly vaccination is due."
            : "Yearly vaccination is up to date.";
    }
}

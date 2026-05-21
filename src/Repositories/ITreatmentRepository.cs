using ClinicVets.Models;

namespace ClinicVets.Repositories;

public interface ITreatmentRepository
{
    Treatment Add(Treatment treatment);
    IReadOnlyList<Treatment> GetByVisitId(int visitId);
}

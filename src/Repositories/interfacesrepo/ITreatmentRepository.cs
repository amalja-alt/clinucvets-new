using ClinicVets.Models;

namespace ClinicVets.Repositories.interfacesrepo;

public interface ITreatmentRepository
{
    Treatment Add(Treatment treatment);
    IReadOnlyList<Treatment> GetByVisitId(int visitId);
}

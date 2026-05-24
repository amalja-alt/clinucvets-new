using ClinicVets.Models;
using ClinicVets.Repositories.interfacesrepo;

namespace ClinicVets.Services;

public class ClinicLookupService(
    IClinicLookupRepository lookupRepository,
    IAnimalRepository animalRepository)
{
    public IReadOnlyList<Customer> GetAllCustomers() => lookupRepository.GetAllCustomers();

    public IReadOnlyList<Animal> GetAllAnimals() => animalRepository.SearchByNameOrChip(string.Empty);

    public int CountVisits() => lookupRepository.CountVisits();

    public int CountVisitsForDate(DateOnly date) => lookupRepository.CountVisitsForDate(date);

    public IReadOnlyList<DashboardVisitSummary> GetVisitsForDate(DateOnly date) => lookupRepository.GetVisitsForDate(date);

    public IReadOnlyList<DashboardVisitSummary> GetRecentVisits(int maxCount) => lookupRepository.GetRecentVisits(maxCount);
}

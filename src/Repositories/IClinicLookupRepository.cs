using ClinicVets.Models;

namespace ClinicVets.Repositories;

public interface IClinicLookupRepository
{
    IReadOnlyList<Customer> GetAllCustomers();
    int CountVisits();
    int CountVisitsForDate(DateOnly date);
    IReadOnlyList<DashboardVisitSummary> GetVisitsForDate(DateOnly date);
    IReadOnlyList<DashboardVisitSummary> GetRecentVisits(int maxCount);
}

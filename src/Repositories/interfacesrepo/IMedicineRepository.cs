using ClinicVets.Models;

namespace ClinicVets.Repositories.interfacesrepo;
public interface IMedicineRepository
{
    IReadOnlyList<Medicine> GetAll();
    IReadOnlyList<Medicine> FindByIds(IEnumerable<int> medicineIds);
    Medicine Add(Medicine medicine);
    bool Remove(int medicineId);
}

using ClinicVets.Models;

namespace ClinicVets.Repositories;

public interface IAnimalRepository
{
    bool ExistsById(int animalId);
    bool ExistsByChipNumber(string chipNumber);
    Animal Add(Animal animal);
    IReadOnlyList<Animal> FindByOwnerCustomerId(int customerId);
    IReadOnlyList<Animal> SearchByNameOrChip(string searchText);
}

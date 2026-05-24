using ClinicVets.Models;

namespace ClinicVets.Repositories.interfacesrepo;

public interface IAnimalCategoryRepository
{
    IReadOnlyList<AnimalCategory> GetAll();
    AnimalCategory? GetById(int categoryId);
    bool ExistsByName(string name, int? excludeCategoryId = null);
    AnimalCategory Add(AnimalCategory category);
    bool Remove(int categoryId);
    int CountAnimalsUsingCategory(int categoryId);
}

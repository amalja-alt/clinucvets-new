using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Validators;

namespace ClinicVets.Services;

public class AnimalCategoryService(
    IAnimalCategoryRepository categoryRepository,
    AnimalCategoryValidator categoryValidator)
{
    public IReadOnlyList<AnimalCategory> GetAllCategories() => categoryRepository.GetAll();

    public OperationResult<AnimalCategory> AddCategory(string name)
    {
        OperationResult<bool> validation = categoryValidator.ValidateCategoryName(name);
        if (!validation.IsSuccess)
        {
            return OperationResult<AnimalCategory>.Failure(validation.ErrorMessage);
        }

        if (categoryRepository.ExistsByName(name))
        {
            return OperationResult<AnimalCategory>.Failure(ValidationMessages.DuplicateCategoryName);
        }

        AnimalCategory saved = categoryRepository.Add(new AnimalCategory { Name = name.Trim() });
        return OperationResult<AnimalCategory>.Success(saved);
    }

    public OperationResult<bool> RemoveCategory(int categoryId)
    {
        if (categoryRepository.GetById(categoryId) is null)
        {
            return OperationResult<bool>.Failure(ValidationMessages.CategoryNotFound);
        }

        if (categoryRepository.CountAnimalsUsingCategory(categoryId) > 0)
        {
            return OperationResult<bool>.Failure(ValidationMessages.CategoryInUse);
        }

        if (!categoryRepository.Remove(categoryId))
        {
            return OperationResult<bool>.Failure(ValidationMessages.CategoryNotFound);
        }

        return OperationResult<bool>.Success(true);
    }
}

using ClinicVets.Services;

namespace ClinicVets.Validators;

/// <summary>
/// Validates animal category input.
/// </summary>
public class AnimalCategoryValidator
{
    public OperationResult<bool> ValidateCategoryName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return OperationResult<bool>.Failure(ValidationMessages.CategoryNameRequired);
        }

        if (!ValidationRules.IsEnglishOrHebrewName(name.Trim()))
        {
            return OperationResult<bool>.Failure(ValidationMessages.InvalidCategoryName);
        }

        return OperationResult<bool>.Success(true);
    }
}

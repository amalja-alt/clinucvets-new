using ClinicVets.Services;

namespace ClinicVets.Validators;

/// <summary>
/// Validates animal patient card input.
/// </summary>
public class AnimalValidator
{
    public OperationResult<bool> ValidateAnimal(string name, decimal weightKg, DateOnly birthDate)
    {
        if (!ValidateName(name))
        {
            return OperationResult<bool>.Failure(ValidationMessages.InvalidAnimalName);
        }

        if (!ValidateWeight(weightKg))
        {
            return OperationResult<bool>.Failure(ValidationMessages.InvalidAnimalWeight);
        }

        if (!ValidateBirthDate(birthDate))
        {
            return OperationResult<bool>.Failure(ValidationMessages.InvalidAnimalBirthDate);
        }

        return OperationResult<bool>.Success(true);
    }

    public bool ValidateName(string name) => ValidationRules.IsEnglishOrHebrewName(name);
    public bool ValidateWeight(decimal weightKg) => ValidationRules.IsAnimalWeightValid(weightKg);
    public bool ValidateBirthDate(DateOnly birthDate) => ValidationRules.IsAnimalBirthDateValid(birthDate);
}

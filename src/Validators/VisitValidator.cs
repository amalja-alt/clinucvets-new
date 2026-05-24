using ClinicVets.Services;

namespace ClinicVets.Validators;

/// <summary>
/// Validates visit and treatment input.
/// </summary>
public class VisitValidator
{
    public OperationResult<bool> ValidateVisit(int animalId, string reason)
    {
        if (animalId <= 0)
        {
            return OperationResult<bool>.Failure(ValidationMessages.AnimalNotFound);
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            return OperationResult<bool>.Failure(ValidationMessages.VisitReasonRequired);
        }

        return OperationResult<bool>.Success(true);
    }
}

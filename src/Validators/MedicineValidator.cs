using ClinicVets.Services;

namespace ClinicVets.Validators;

/// <summary>
/// Validates medicine inventory input.
/// </summary>
public class MedicineValidator
{
    public OperationResult<bool> ValidateMedicine(string name, decimal price, int quantityInStock)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return OperationResult<bool>.Failure(ValidationMessages.MedicineNameRequired);
        }

        if (price < 0 || quantityInStock < 0)
        {
            return OperationResult<bool>.Failure(ValidationMessages.InvalidMedicinePriceOrQuantity);
        }

        return OperationResult<bool>.Success(true);
    }
}

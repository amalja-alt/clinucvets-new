using ClinicVets.Services;

namespace ClinicVets.Validators;

/// Validates customer management input.
public class CustomerValidator
{
    public OperationResult<bool> ValidateCustomer(string fullName, string identityNumber, string phone, string email)
    {
        if (!ValidateFullName(fullName))
        {
            return OperationResult<bool>.Failure(ValidationMessages.InvalidFullName);
        }

        if (!ValidateIdentityNumber(identityNumber))
        {
            return OperationResult<bool>.Failure(ValidationMessages.InvalidIsraeliIdentityNumber);
        }

        if (!ValidatePhone(phone))
        {
            return OperationResult<bool>.Failure(ValidationMessages.InvalidPhone);
        }

        if (!ValidateEmail(email))
        {
            return OperationResult<bool>.Failure(ValidationMessages.InvalidEmail);
        }

        return OperationResult<bool>.Success(true);
    }

    public bool ValidateFullName(string fullName) => ValidationRules.IsEnglishOrHebrewName(fullName);
    public bool ValidateIdentityNumber(string identityNumber) => ValidationRules.IsIdentityNumberValid(identityNumber);
    public bool ValidatePhone(string phone) => ValidationRules.IsPhoneValid(phone);
    public bool ValidateEmail(string email) => ValidationRules.IsEmailValid(email);
}

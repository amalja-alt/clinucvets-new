using ClinicVets.Services;
namespace ClinicVets.Validators;

// class to check the details off the employee 
public class EmployeeValidator
{

    private bool ValidateUsername(string username) => ValidationRules.IsUsernameValid(username);
    private bool ValidatePassword(string password) => ValidationRules.IsPasswordValid(password);
    private bool ValidateEmployeeNumber(string employeeNumber) => ValidationRules.IsEmployeeNumberValid(employeeNumber);
    private bool ValidateIdentityNumber(string identityNumber) => ValidationRules.IsIdentityNumberValid(identityNumber);
    private bool ValidateEmail(string email) => ValidationRules.IsEmailValid(email);

    // check if the details of the user want to regist are correct acourding to the rules in the assignmnet 
    public OperationResult<bool> ValidateRegistration(string username, string password, string employeeNumber, string email, string identityNumber)
    {
        if (!ValidateUsername(username)) return OperationResult<bool>.Failure(ValidationMessages.InvalidUsernameFormat);
        if (!ValidatePassword(password)) return OperationResult<bool>.Failure(ValidationMessages.InvalidPasswordFormat);
        if (!ValidateEmployeeNumber(employeeNumber)) return OperationResult<bool>.Failure(ValidationMessages.InvalidEmployeeNumber);
        if (!ValidateEmail(email)) return OperationResult<bool>.Failure(ValidationMessages.InvalidEmail);
        if (!ValidateIdentityNumber(identityNumber)) return OperationResult<bool>.Failure(ValidationMessages.InvalidIsraeliIdentityNumber);
        return OperationResult<bool>.Success(true);
    }

    // check the details to log in the system
    public OperationResult<bool> ValidateLogin(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username)) return OperationResult<bool>.Failure(ValidationMessages.UsernameRequired);
        if (string.IsNullOrWhiteSpace(password)) return OperationResult<bool>.Failure(ValidationMessages.PasswordRequired);
        if (!ValidateUsername(username)) return OperationResult<bool>.Failure(ValidationMessages.InvalidUsernameFormat);
        return OperationResult<bool>.Success(true);
    }


}

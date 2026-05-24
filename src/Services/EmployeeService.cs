using ClinicVets.Models;
using ClinicVets.Validators;
using ClinicVets.Repositories.interfacesrepo;


namespace ClinicVets.Services;
// this class take a repo interface and a validator as dependencies
public class EmployeeService(IEmployeeRepository employeeRepository, EmployeeValidator employeeValidator)
{
    // what this class provide :
    // operation result is a object that countain the info about the function if its success or not and the error message if its not success



    // 1- Checks whether registration fields are valid.
    public OperationResult<bool> ValidateRegistrationInput(
        string username,
        string password,
        string employeeNumber,
        string email,
        string identityNumber)
    {
        return employeeValidator.ValidateRegistration(username, password, employeeNumber, email, identityNumber);
    }


    // the main regestration methos 
    public OperationResult<Employee> RegisterEmployee(
        string username,
        string password,
        string employeeNumber,
        string email,
        string identityNumber,
        StaffRole role)
    {
        OperationResult<bool> validationResult = ValidateRegistrationInput(
            username,
            password,
            employeeNumber,
            email,
            identityNumber);

        if (!validationResult.IsSuccess)
        {
            return OperationResult<Employee>.Failure(validationResult.ErrorMessage);
        }

        if (employeeRepository.ExistsByRegistrationFields(username, employeeNumber, email, identityNumber))
        {
            return OperationResult<Employee>.Failure(ValidationMessages.DuplicateEmployee);
        }

        Employee employee = new()
        {
            Username = username,
            PasswordHash = password,
            EmployeeNumber = employeeNumber,
            Email = email,
            IdentityNumber = identityNumber,
            Role = role
        };

        Employee savedEmployee = employeeRepository.Add(employee);
        return OperationResult<Employee>.Success(savedEmployee);
    }
}

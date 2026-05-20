using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Validators;

namespace ClinicVets.Services;

public class EmployeeService(IEmployeeRepository employeeRepository, EmployeeValidator employeeValidator)
{
    public OperationResult<bool> ValidateRegistrationInput(
        string username,
        string password,
        string employeeNumber,
        string email,
        string identityNumber)
    {
        return employeeValidator.ValidateRegistration(username, password, employeeNumber, email, identityNumber);
    }

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

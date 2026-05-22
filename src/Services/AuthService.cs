using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Validators;

namespace ClinicVets.Services;

public class AuthService(IEmployeeRepository employeeRepository, EmployeeValidator employeeValidator)
{
    public Employee? CurrentUser { get; private set; }

    public bool IsAuthenticated => CurrentUser is not null;

    public AuthenticationResult Login(string username, string password)
    {
        OperationResult<bool> validationResult = employeeValidator.ValidateLogin(username, password);

        if (!validationResult.IsSuccess)
        {
            return AuthenticationResult.Failure(validationResult.ErrorMessage);
        }

        Employee? employee = employeeRepository.FindByUsername(username);

        if (employee is null || !PasswordHasher.VerifyPassword(password, employee.PasswordHash))
        {
            return AuthenticationResult.Failure(ValidationMessages.WrongCredentials);
        }

        CurrentUser = employee;
        return AuthenticationResult.Success(employee);
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}

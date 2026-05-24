using ClinicVets.Models;
using ClinicVets.Repositories.interfacesrepo;
using ClinicVets.Validators;

namespace ClinicVets.Services;
// this is the class for the log in log out of the user 
public class AuthService(IEmployeeRepository employeeRepository, EmployeeValidator employeeValidator)
{
    // Stores the currently logged-in employee, or null if no one is logged in
    public Employee? CurrentUser { get; private set; }

    // Indicates whether an employee is currently authenticated (logged in)
    public bool IsAuthenticated => CurrentUser is not null;

    // Handles the login process by validating input, checking credentials, and setting the current user if successful
    public AuthenticationResult Login(string username, string password)
    {
        OperationResult<bool> validationResult = employeeValidator.ValidateLogin(username, password);

        // If validation fails, return an authentication failure result with the error message
        if (!validationResult.IsSuccess)
        {
            return AuthenticationResult.Failure(validationResult.ErrorMessage);
        }

        Employee? employee = employeeRepository.FindByUsername(username);
        // If no employee is found with the given username, or if the password does not match, return an authentication failure result
        if (employee is null || employee.PasswordHash != password)
        {
            return AuthenticationResult.Failure(ValidationMessages.WrongCredentials);
        }

        CurrentUser = employee;
        return AuthenticationResult.Success(employee);
    }
    // Handles the logout process by clearing the current user
    public void Logout()
    {
        CurrentUser = null;
    }
}

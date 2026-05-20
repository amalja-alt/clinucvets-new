using ClinicVets.Models;

namespace ClinicVets.Services;

public class AuthenticationResult
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }
    public Employee? LoggedInUser { get; }

    private AuthenticationResult(bool isSuccess, string errorMessage, Employee? loggedInUser)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        LoggedInUser = loggedInUser;
    }

    public static AuthenticationResult Success(Employee loggedInUser)
    {
        return new AuthenticationResult(true, string.Empty, loggedInUser);
    }

    public static AuthenticationResult Failure(string errorMessage)
    {
        return new AuthenticationResult(false, errorMessage, null);
    }
}

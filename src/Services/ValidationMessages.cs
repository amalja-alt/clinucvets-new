namespace ClinicVets.Services;

public static class ValidationMessages
{
    public const string UsernameRequired = "Username is required.";
    public const string PasswordRequired = "Password is required.";
    public const string InvalidUsernameFormat = "Username must be 6-8 English letters/digits with up to 2 digits.";
    public const string InvalidPasswordFormat = "Password must be 8-10 chars and include a letter, digit, and !/#/$.";
    public const string InvalidEmployeeNumber = "Employee number must contain exactly 4 digits.";
    public const string InvalidEmail = "Invalid email format.";
    public const string InvalidIsraeliIdentityNumber = "Israeli ID must contain exactly 9 digits.";
    public const string DuplicateEmployee = "An employee with these unique details already exists.";
    public const string WrongCredentials = "Username or password is incorrect.";
    public const string NotAuthenticated = "No user is currently logged in.";

    public const string SecretaryOnly = "Only a secretary can register customers.";
    public const string CustomerManagementSecretaryOnly = "Only a secretary can access customer management.";
    public const string InvalidFullName = "Full name must contain letters only.";
    public const string InvalidPhone = "Invalid phone number format.";
    public const string DuplicateCustomer = "Customer identity number already exists.";
}

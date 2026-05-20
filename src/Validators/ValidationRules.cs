using System.Text.RegularExpressions;
namespace ClinicVets.Validators;
// calss to the rules of the details of the employee to check if they are correct or not 
internal static partial class ValidationRules
{
    public static bool IsUsernameValid(string username)
    {
        if (!UsernameRegex().IsMatch(username))
        {
            return false;
        }

        return username.Count(char.IsDigit) <= 2;
    }

    public static bool IsPasswordValid(string password) => PasswordRegex().IsMatch(password);

    public static bool IsEmployeeNumberValid(string employeeNumber) => Regex.IsMatch(employeeNumber, @"^\d{4}$");

    public static bool IsIdentityNumberValid(string identityNumber) => Regex.IsMatch(identityNumber, @"^\d{9}$");

    public static bool IsEmailValid(string email) => EmailRegex().IsMatch(email);

    public static bool IsPhoneValid(string phone) => Regex.IsMatch(phone, @"^0\d{8,9}$");

    public static bool IsEnglishOrHebrewName(string name) => NameRegex().IsMatch(name.Trim());

    public static bool IsAnimalWeightValid(decimal weightKg) => weightKg >= 0.1m && weightKg <= 100m;

    public static bool IsAnimalBirthDateValid(DateOnly birthDate)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        return birthDate <= today && birthDate.Year >= 2000;
    }

    [GeneratedRegex(@"^[A-Za-z0-9]{6,8}$")]
    private static partial Regex UsernameRegex();

    [GeneratedRegex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!#$])[A-Za-z\d!#$]{8,10}$")]
    private static partial Regex PasswordRegex();

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"^[A-Za-z\u05D0-\u05EA]+(?: [A-Za-z\u05D0-\u05EA]+)*$")]
    private static partial Regex NameRegex();
}

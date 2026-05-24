namespace ClinicVets.Services;

// error messages for validation failures, to avoid hardcoding strings in multiple places and to ensure consistency across the app
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
    public const string DatabaseBusy = "The database is busy. Close other running copies of the app and try again.";

    public const string InvalidAnimalName = "Animal name must contain letters only.";
    public const string InvalidAnimalWeight = "Animal weight must be between 0.1 and 100 kg.";
    public const string InvalidAnimalBirthDate = "Animal birth date must be valid and cannot be in the future.";
    public const string AnimalOwnerRequired = "Animal owner customer was not found.";
    public const string DuplicateChipNumber = "Animal chip number already exists.";
    public const string AnimalNotFound = "Animal was not found.";

    public const string CategoryNameRequired = "Category name is required.";
    public const string InvalidCategoryName = "Category name must contain letters only.";
    public const string DuplicateCategoryName = "Animal category already exists.";
    public const string CategoryNotFound = "Animal category was not found.";
    public const string CategoryInUse = "Animal category cannot be removed while animals use it.";

    public const string MedicineNameRequired = "Medicine name is required.";
    public const string InvalidMedicinePriceOrQuantity = "Medicine price and quantity must be zero or greater.";

    public const string VeterinarianOnly = "Only a veterinarian can open visits.";
    public const string VisitReasonRequired = "Visit reason is required.";
}

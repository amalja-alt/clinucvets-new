namespace ClinicVets.Services;

// error messages for validation failures, to avoid hardcoding strings in multiple places and to ensure consistency across the app
public static class ValidationMessages
{
<<<<<<< HEAD
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
=======
    // General
    public const string InvalidInput = "Invalid input.";
    public const string NotAuthenticated = "User is not authenticated.";

    // Auth
    public const string WrongCredentials = "Wrong username or password.";

    // Employee
    public const string UsernameRequired = "Username is required.";
    public const string PasswordRequired = "Password is required.";
    public const string InvalidUsernameFormat = "Invalid username format.";
    public const string InvalidPasswordFormat = "Invalid password format.";
    public const string InvalidEmployeeNumber = "Invalid employee number.";
    public const string DuplicateEmployee = "Employee already exists.";

    // Customer
    public const string InvalidFullName = "Invalid full name.";
    public const string InvalidIsraeliIdentityNumber = "Invalid Israeli identity number.";
    public const string InvalidPhone = "Invalid phone number.";
    public const string InvalidEmail = "Invalid email address.";
    public const string DuplicateCustomer = "Customer already exists.";
    public const string SecretaryOnly = "Only secretary can perform this action.";
    public const string CustomerManagementSecretaryOnly = "Only secretary can manage customers.";

    // Animal
    public const string InvalidAnimalName = "Invalid animal name.";
    public const string InvalidAnimalWeight = "Invalid animal weight.";
    public const string InvalidAnimalBirthDate = "Invalid animal birth date.";
    public const string AnimalOwnerRequired = "Animal owner is required.";
    public const string DuplicateChipNumber = "Chip number already exists.";
    public const string AnimalNotFound = "Animal not found.";

    // Animal Category
    public const string CategoryNameRequired = "Category name is required.";
    public const string InvalidCategoryName = "Invalid category name.";
    public const string DuplicateCategoryName = "Category already exists.";
    public const string CategoryNotFound = "Category not found.";
    public const string CategoryInUse = "Category is currently in use.";

    // Visit
    public const string VisitReasonRequired = "Visit reason is required.";
    public const string DiagnosisRequired = "Diagnosis is required.";
    public const string VeterinarianOnly = "Only veterinarian can perform this action.";

    // Medicine
    public const string MedicineNameRequired = "Medicine name is required.";
    public const string MedicineStockInvalid = "Medicine stock is invalid.";
    public const string InvalidMedicinePriceOrQuantity = "Invalid medicine price or quantity.";
}
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9

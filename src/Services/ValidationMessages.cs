namespace ClinicVets.Services;

public static class ValidationMessages
{
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
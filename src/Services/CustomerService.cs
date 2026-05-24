using ClinicVets.Models;
using ClinicVets.Repositories.interfacesrepo;
using ClinicVets.Validators;
using Microsoft.Data.Sqlite;

namespace ClinicVets.Services;

public class CustomerService(
    ICustomerRepository customerRepository,
    IAnimalRepository animalRepository,
    CustomerValidator customerValidator)
{
    public OperationResult<Customer> RegisterCustomer(
        Employee? currentUser,
        string fullName,
        string identityNumber,
        string phone,
        string email)
    {
        // check the permission of the user to manage customers, only secretaries can manage customers
        if (!CanManageCustomers(currentUser))
        {
            return OperationResult<Customer>.Failure(ValidationMessages.SecretaryOnly);
        }

        // the user is secretary
        OperationResult<bool> validationResult = customerValidator.ValidateCustomer(
            fullName,
            identityNumber,
            phone,
            email);

        // check the input of the custemer, if its not valid return the error message
        if (!validationResult.IsSuccess)
        {
            return OperationResult<Customer>.Failure(validationResult.ErrorMessage);
        }

        try
        {
            // check if there is already a customer with the same identity number, if yes return an error message
            if (customerRepository.ExistsByIdentityNumber(identityNumber))
            {
                return OperationResult<Customer>.Failure(ValidationMessages.DuplicateCustomer);
            }

            Customer customer = new()
            {
                FullName = fullName,
                IdentityNumber = identityNumber,
                Phone = phone,
                Email = email
            };

            Customer savedCustomer = customerRepository.Add(customer);
            return OperationResult<Customer>.Success(savedCustomer);
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 5)
        {
            return OperationResult<Customer>.Failure(ValidationMessages.DatabaseBusy);
        }
    }

    // return the customer if we have a customer with the same identity number or phone number 
    public OperationResult<Customer?> SearchByIdentityOrPhone(Employee? currentUser, string searchText)
    {
        // check the permission of the user to manage customers, only secretaries can manage customers
        if (!CanManageCustomers(currentUser))
        {
            return OperationResult<Customer?>.Failure(ValidationMessages.CustomerManagementSecretaryOnly);
        }

        // the user is secretary, search for the customer by identity number or phone number
        try
        {
            string normalizedSearchText = NormalizeSearchText(searchText);
            return OperationResult<Customer?>.Success(customerRepository.FindByIdentityOrPhone(normalizedSearchText));
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 5)
        {
            return OperationResult<Customer?>.Failure(ValidationMessages.DatabaseBusy);
        }
    }

    // return the customer if we have a customer with the same id in the table

    public OperationResult<IReadOnlyList<Animal>> GetCustomerAnimals(Employee? currentUser, int customerId)
    {
        // check the permission of the user to manage customers, only secretaries can manage customers
        // this is for a specific use case where we want to show the animals of a customer 
        if (!CanManageCustomers(currentUser))
        {
            return OperationResult<IReadOnlyList<Animal>>.Failure(ValidationMessages.CustomerManagementSecretaryOnly);
        }

        return OperationResult<IReadOnlyList<Animal>>.Success(animalRepository.FindByOwnerCustomerId(customerId));
    }

    // private function to keep just the secretary can manage customers, this is for code readability 
    private static bool CanManageCustomers(Employee? currentUser) => currentUser?.Role == StaffRole.Secretary;

    // private function to normalize the search text by trimming it and removing non-digit characters
    private static string NormalizeSearchText(string searchText)
    {
        string trimmed = searchText.Trim();
        string digitsOnly = new(trimmed.Where(char.IsDigit).ToArray());
        return digitsOnly.Length > 0 ? digitsOnly : trimmed;
    }
}

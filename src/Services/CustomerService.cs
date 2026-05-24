using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Validators;
<<<<<<< HEAD
using Microsoft.Data.Sqlite;
=======
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9

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
<<<<<<< HEAD
=======
        // Assignment requirement: customer management is restricted to secretaries.
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
        if (!CanManageCustomers(currentUser))
        {
            return OperationResult<Customer>.Failure(ValidationMessages.SecretaryOnly);
        }

        OperationResult<bool> validationResult = customerValidator.ValidateCustomer(
            fullName,
            identityNumber,
            phone,
            email);

        if (!validationResult.IsSuccess)
        {
            return OperationResult<Customer>.Failure(validationResult.ErrorMessage);
        }

<<<<<<< HEAD
        try
        {
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
=======
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
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
    }

    public OperationResult<Customer?> SearchByIdentityOrPhone(Employee? currentUser, string searchText)
    {
<<<<<<< HEAD
=======
        // Defense in depth: searches are authorized in the service, not only in the UI.
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
        if (!CanManageCustomers(currentUser))
        {
            return OperationResult<Customer?>.Failure(ValidationMessages.CustomerManagementSecretaryOnly);
        }

<<<<<<< HEAD
        try
        {
            string normalizedSearchText = NormalizeSearchText(searchText);
            return OperationResult<Customer?>.Success(customerRepository.FindByIdentityOrPhone(normalizedSearchText));
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 5)
        {
            return OperationResult<Customer?>.Failure(ValidationMessages.DatabaseBusy);
        }
=======
        return OperationResult<Customer?>.Success(customerRepository.FindByIdentityOrPhone(searchText));
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
    }

    public OperationResult<IReadOnlyList<Animal>> GetCustomerAnimals(Employee? currentUser, int customerId)
    {
<<<<<<< HEAD
=======
        // Defense in depth: linked animal viewing is part of customer management.
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
        if (!CanManageCustomers(currentUser))
        {
            return OperationResult<IReadOnlyList<Animal>>.Failure(ValidationMessages.CustomerManagementSecretaryOnly);
        }

        return OperationResult<IReadOnlyList<Animal>>.Success(animalRepository.FindByOwnerCustomerId(customerId));
    }

    private static bool CanManageCustomers(Employee? currentUser) => currentUser?.Role == StaffRole.Secretary;
<<<<<<< HEAD

    private static string NormalizeSearchText(string searchText)
    {
        string trimmed = searchText.Trim();
        string digitsOnly = new(trimmed.Where(char.IsDigit).ToArray());
        return digitsOnly.Length > 0 ? digitsOnly : trimmed;
    }
=======
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
}

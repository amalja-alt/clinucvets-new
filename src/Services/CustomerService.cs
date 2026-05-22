using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Validators;

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
        // Assignment requirement: customer management is restricted to secretaries.
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

    public OperationResult<Customer?> SearchByIdentityOrPhone(Employee? currentUser, string searchText)
    {
        // Defense in depth: searches are authorized in the service, not only in the UI.
        if (!CanManageCustomers(currentUser))
        {
            return OperationResult<Customer?>.Failure(ValidationMessages.CustomerManagementSecretaryOnly);
        }

        string normalizedSearchText = NormalizeSearchText(searchText);
        return OperationResult<Customer?>.Success(customerRepository.FindByIdentityOrPhone(normalizedSearchText));
    }

    public OperationResult<IReadOnlyList<Animal>> GetCustomerAnimals(Employee? currentUser, int customerId)
    {
        // Defense in depth: linked animal viewing is part of customer management.
        if (!CanManageCustomers(currentUser))
        {
            return OperationResult<IReadOnlyList<Animal>>.Failure(ValidationMessages.CustomerManagementSecretaryOnly);
        }

        return OperationResult<IReadOnlyList<Animal>>.Success(animalRepository.FindByOwnerCustomerId(customerId));
    }

    private static bool CanManageCustomers(Employee? currentUser) => currentUser?.Role == StaffRole.Secretary;

    private static string NormalizeSearchText(string searchText)
    {
        string trimmed = searchText.Trim();
        string digitsOnly = new(trimmed.Where(char.IsDigit).ToArray());
        return digitsOnly.Length > 0 ? digitsOnly : trimmed;
    }
}

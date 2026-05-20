using ClinicVets.Models;

namespace ClinicVets.Repositories;

public interface ICustomerRepository
{
    bool ExistsByIdentityNumber(string identityNumber);
    Customer? FindByIdentityOrPhone(string searchText);
    Customer? FindById(int customerId);
    Customer Add(Customer customer);
}

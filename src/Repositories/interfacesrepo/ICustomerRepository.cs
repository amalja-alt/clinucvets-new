using ClinicVets.Models;

namespace ClinicVets.Repositories.interfacesrepo;

// alaa 
// this class between the ui and the data access layer ( in memory or database )
// interface because we can have multiple implementations 

public interface ICustomerRepository
{
    bool ExistsByIdentityNumber(string identityNumber);
    Customer? FindByIdentityOrPhone(string searchText);
    Customer? FindById(int customerId);
    Customer Add(Customer customer);
}

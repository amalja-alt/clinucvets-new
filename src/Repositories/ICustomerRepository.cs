using ClinicVets.Models;

namespace ClinicVets.Repositories;

<<<<<<< HEAD
<<<<<<< HEAD
// alaa 
// this class between the ui and the data access layer ( in memory or database )
// interface because we can have multiple implementations 
=======
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
// alaa 
// this class between the ui and the data access layer ( in memory or database )
// interface because we can have multiple implementations 
>>>>>>> main
public interface ICustomerRepository
{
    bool ExistsByIdentityNumber(string identityNumber);
    Customer? FindByIdentityOrPhone(string searchText);
    Customer? FindById(int customerId);
    Customer Add(Customer customer);
}

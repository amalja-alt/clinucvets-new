using ClinicVets.Models;

namespace ClinicVets.Repositories;

<<<<<<< HEAD
// alaa 
// this class between the ui and the data access layer ( in memory or database )
// interface because we can have multiple implementations 

=======
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
public interface IEmployeeRepository
{
    Employee? FindByUsername(string username);
    bool ExistsByRegistrationFields(string username, string employeeNumber, string email, string identityNumber);
    Employee Add(Employee employee);
}

using ClinicVets.Models;

namespace ClinicVets.Repositories;

// alaa 
// this class between the ui and the data access layer ( in memory or database )
// interface because we can have multiple implementations 

public interface IEmployeeRepository
{
    Employee? FindByUsername(string username);
    bool ExistsByRegistrationFields(string username, string employeeNumber, string email, string identityNumber);
    Employee Add(Employee employee);
}

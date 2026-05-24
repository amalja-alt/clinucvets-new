namespace ClinicVets.Models;

// alaa 
<<<<<<< HEAD
// this class represents an employee of the clinic
// employee == worker 
public class Employee
{

=======
// employee - worker in the clinic 
public class Employee
{
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
    public int Id { get; init; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty;
    public StaffRole Role { get; set; }
}

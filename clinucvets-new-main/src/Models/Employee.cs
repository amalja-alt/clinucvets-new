namespace ClinicVets.Models;

// alaa 
// this class represents an employee of the clinic
// employee == worker 
public class Employee
{

    public int Id { get; init; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty;
    public StaffRole Role { get; set; }
}

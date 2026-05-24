namespace ClinicVets.Models;

// alaa - id is the id of the customer( from thr system ) 
public class Customer
{
    public int Id { get; init; }
    public string FullName { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

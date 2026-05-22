using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.FunctionalTests;

public class EmployeeServiceFunctionalTests
{
    [Fact]
    public void RegisterEmployee_WithValidData_SucceedsAndStoresHashedPassword()
    {
        FakeEmployeeRepository employees = new();
        EmployeeService service = new(employees, new EmployeeValidator());

        OperationResult<Employee> result = service.RegisterEmployee("worker1", "Worker#1", "1234", "worker@clinicvets.com", "123456782", StaffRole.Secretary);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEqual("Worker#1", result.Value!.PasswordHash);
        Assert.True(PasswordHasher.VerifyPassword("Worker#1", result.Value.PasswordHash));
    }

    [Fact]
    public void RegisterEmployee_WithDuplicateUniqueField_Fails()
    {
        FakeEmployeeRepository employees = new();
        employees.Seed(new Employee
        {
            Id = 1,
            Username = "worker1",
            PasswordHash = "Worker#1",
            EmployeeNumber = "1234",
            Email = "worker@clinicvets.com",
            IdentityNumber = "123456782",
            Role = StaffRole.Secretary
        });
        EmployeeService service = new(employees, new EmployeeValidator());

        OperationResult<Employee> result = service.RegisterEmployee("worker1", "Other#12", "5678", "other@clinicvets.com", "100000009", StaffRole.Veterinarian);

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.DuplicateEmployee, result.ErrorMessage);
    }
}

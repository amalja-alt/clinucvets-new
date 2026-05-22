using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.DecisionTableTests;

public class RegisterEmployeeDecisionTableTests
{
    [Fact]
    public void AllFieldsValid_AndNoDuplicate_ReturnsSuccess()
    {
        EmployeeService service = new(new FakeEmployeeRepository(), new EmployeeValidator());

        OperationResult<Employee> result = service.RegisterEmployee("worker1", "Worker#1", "1234", "worker@clinicvets.com", "123456782", StaffRole.Secretary);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("abc12", "Worker#1", "1234", "worker@clinicvets.com", "123456782", ValidationMessages.InvalidUsernameFormat)]
    [InlineData("worker1", "Worker12", "1234", "worker@clinicvets.com", "123456782", ValidationMessages.InvalidPasswordFormat)]
    [InlineData("worker1", "Worker#1", "123", "worker@clinicvets.com", "123456782", ValidationMessages.InvalidEmployeeNumber)]
    [InlineData("worker1", "Worker#1", "1234", "worker-at-clinicvets.com", "123456782", ValidationMessages.InvalidEmail)]
    [InlineData("worker1", "Worker#1", "1234", "worker@clinicvets.com", "12345678A", ValidationMessages.InvalidIsraeliIdentityNumber)]
    public void InvalidInputDecisionRows_ReturnExpectedFailure(
        string username,
        string password,
        string employeeNumber,
        string email,
        string identityNumber,
        string expectedMessage)
    {
        EmployeeService service = new(new FakeEmployeeRepository(), new EmployeeValidator());

        OperationResult<Employee> result = service.RegisterEmployee(username, password, employeeNumber, email, identityNumber, StaffRole.Secretary);

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedMessage, result.ErrorMessage);
    }

    [Fact]
    public void DuplicateEmployeeDecisionRow_ReturnsFailure()
    {
        FakeEmployeeRepository employees = new();
        employees.Seed(new Employee { Id = 1, Username = "worker1", PasswordHash = "Worker#1", EmployeeNumber = "1234", Email = "worker@clinicvets.com", IdentityNumber = "123456782", Role = StaffRole.Secretary });
        EmployeeService service = new(employees, new EmployeeValidator());

        OperationResult<Employee> result = service.RegisterEmployee("worker1", "Other#12", "5678", "other@clinicvets.com", "100000009", StaffRole.Veterinarian);

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.DuplicateEmployee, result.ErrorMessage);
    }
}

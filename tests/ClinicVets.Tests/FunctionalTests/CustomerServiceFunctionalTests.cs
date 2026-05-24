using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.FunctionalTests;

public class CustomerServiceFunctionalTests
{
    [Fact]
    public void RegisterCustomer_ForSecretary_Succeeds()
    {
        FakeCustomerRepository customers = new();
        CustomerService service = new(customers, new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Secretary(), "Dana Levi", "123456782", "0501234567", "dana.levi@gmail.com");

        Assert.True(result.IsSuccess);
        Assert.Equal("Dana Levi", result.Value?.FullName);
    }

    [Fact]
    public void RegisterCustomer_WithDuplicateIdentity_Fails()
    {
        FakeCustomerRepository customers = new();
        customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana.levi@gmail.com" });
        CustomerService service = new(customers, new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Secretary(), "Dana Other", "123456782", "0527654321", "dana.other@gmail.com");

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.DuplicateCustomer, result.ErrorMessage);
    }

    [Fact]
    public void SearchCustomer_ByIdentity_Succeeds()
    {
        (CustomerService service, Customer saved) = ServiceWithCustomer();

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), "123456782");

        Assert.True(result.IsSuccess);
        Assert.Equal(saved.Id, result.Value?.Id);
    }

    [Fact]
    public void SearchCustomer_ByIdentityWithSpaces_Succeeds()
    {
        (CustomerService service, Customer saved) = ServiceWithCustomer();

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), " 123 456 782 ");

        Assert.True(result.IsSuccess);
        Assert.Equal(saved.Id, result.Value?.Id);
    }

    [Fact]
    public void SearchCustomer_ByPhone_Succeeds()
    {
        (CustomerService service, Customer saved) = ServiceWithCustomer();

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), "0501234567");

        Assert.True(result.IsSuccess);
        Assert.Equal(saved.Id, result.Value?.Id);
    }

    [Fact]
    public void SearchCustomer_ByFormattedPhone_Succeeds()
    {
        (CustomerService service, Customer saved) = ServiceWithCustomer();

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), "050-123-4567");

        Assert.True(result.IsSuccess);
        Assert.Equal(saved.Id, result.Value?.Id);
    }

    [Theory]
    [InlineData("050 123 4567")]
    [InlineData("(050) 123-4567")]
    [InlineData(" 050-123-4567 ")]
    public void SearchCustomer_ByPhoneWithCommonFormatting_Succeeds(string searchText)
    {
        (CustomerService service, Customer saved) = ServiceWithCustomer();

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), searchText);

        Assert.True(result.IsSuccess);
        Assert.Equal(saved.Id, result.Value?.Id);
    }

    [Fact]
    public void SearchCustomer_NotFound_ReturnsSuccessWithNullValue()
    {
        (CustomerService service, _) = ServiceWithCustomer();

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), "000000000");

        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal(string.Empty, result.ErrorMessage);
    }

    [Fact]
    public void SearchCustomer_ForVeterinarian_ReturnsAuthorizationFailure()
    {
        (CustomerService service, _) = ServiceWithCustomer();

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Veterinarian(), "123456782");

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.CustomerManagementSecretaryOnly, result.ErrorMessage);
    }

    [Fact]
    public void GetCustomerAnimals_ForSecretary_ReturnsLinkedAnimals()
    {
        FakeCustomerRepository customers = new();
        FakeAnimalRepository animals = new();
        Customer saved = customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana.levi@gmail.com" });
        Animal linkedAnimal = animals.Add(new Animal
        {
            Name = "Luna",
            ChipNumber = "CAT-2001",
            Type = AnimalType.Cat,
            WeightKg = 4.3m,
            BirthDate = new DateOnly(2022, 8, 5),
            LastVaccinationDate = new DateOnly(2025, 5, 1),
            OwnerCustomerId = saved.Id
        });
        animals.Add(new Animal
        {
            Name = "Buddy",
            ChipNumber = "DOG-1001",
            Type = AnimalType.Dog,
            WeightKg = 18.5m,
            BirthDate = new DateOnly(2021, 4, 12),
            LastVaccinationDate = new DateOnly(2025, 3, 20),
            OwnerCustomerId = saved.Id + 1
        });
        CustomerService service = new(customers, animals, new CustomerValidator());

        OperationResult<IReadOnlyList<Animal>> result = service.GetCustomerAnimals(TestEmployees.Secretary(), saved.Id);

        Assert.True(result.IsSuccess);
        Animal animal = Assert.Single(result.Value!);
        Assert.Equal(linkedAnimal.Id, animal.Id);
    }

    [Fact]
    public void GetCustomerAnimals_ForVeterinarian_ReturnsAuthorizationFailure()
    {
        (CustomerService service, Customer saved) = ServiceWithCustomer();

        OperationResult<IReadOnlyList<Animal>> result = service.GetCustomerAnimals(TestEmployees.Veterinarian(), saved.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.CustomerManagementSecretaryOnly, result.ErrorMessage);
    }

    private static (CustomerService Service, Customer Saved) ServiceWithCustomer()
    {
        FakeCustomerRepository customers = new();
        Customer saved = customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana.levi@gmail.com" });
        return (new CustomerService(customers, new FakeAnimalRepository(), new CustomerValidator()), saved);
    }
}

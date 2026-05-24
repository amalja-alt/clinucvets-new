using ClinicVets.Models;
using ClinicVets.Repositories;

namespace ClinicVets.Tests.TestSupport;

internal sealed class FakeEmployeeRepository : IEmployeeRepository
{
    private readonly List<Employee> _employees = [];
    private int _nextId = 1;

    public void Seed(Employee employee)
    {
        _employees.Add(employee);
        _nextId = Math.Max(_nextId, employee.Id + 1);
    }

    public Employee? FindByUsername(string username)
    {
        return _employees.SingleOrDefault(employee => employee.Username == username);
    }

    public bool ExistsByRegistrationFields(string username, string employeeNumber, string email, string identityNumber)
    {
        return _employees.Any(employee =>
            employee.Username == username ||
            employee.EmployeeNumber == employeeNumber ||
            employee.Email == email ||
            employee.IdentityNumber == identityNumber);
    }

    public Employee Add(Employee employee)
    {
        Employee saved = new()
        {
            Id = _nextId++,
            Username = employee.Username,
            PasswordHash = employee.PasswordHash,
            EmployeeNumber = employee.EmployeeNumber,
            Email = employee.Email,
            IdentityNumber = employee.IdentityNumber,
            Role = employee.Role
        };
        _employees.Add(saved);
        return saved;
    }
}

internal sealed class FakeCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = [];
    private int _nextId = 1;

    public bool ExistsByIdentityNumber(string identityNumber)
    {
        return _customers.Any(customer => customer.IdentityNumber == identityNumber);
    }

    public Customer? FindByIdentityOrPhone(string searchText)
    {
        return _customers.SingleOrDefault(customer => customer.IdentityNumber == searchText || customer.Phone == searchText);
    }

    public Customer? FindById(int customerId)
    {
        return _customers.SingleOrDefault(customer => customer.Id == customerId);
    }

    public Customer Add(Customer customer)
    {
        Customer saved = new()
        {
            Id = _nextId++,
            FullName = customer.FullName,
            IdentityNumber = customer.IdentityNumber,
            Phone = customer.Phone,
            Email = customer.Email
        };
        _customers.Add(saved);
        return saved;
    }
}

internal sealed class FakeAnimalRepository : IAnimalRepository
{
    private readonly List<Animal> _animals = [];
    private int _nextId = 1;

    public bool ExistsById(int animalId)
    {
        return _animals.Any(animal => animal.Id == animalId);
    }

    public bool ExistsByChipNumber(string chipNumber)
    {
        return _animals.Any(animal => animal.ChipNumber == chipNumber);
    }

    public Animal Add(Animal animal)
    {
        Animal saved = new()
        {
            Id = _nextId++,
            Name = animal.Name,
            ChipNumber = animal.ChipNumber,
            Type = animal.Type,
            WeightKg = animal.WeightKg,
            BirthDate = animal.BirthDate,
            LastVaccinationDate = animal.LastVaccinationDate,
            OwnerCustomerId = animal.OwnerCustomerId
        };
        _animals.Add(saved);
        return saved;
    }

    public IReadOnlyList<Animal> FindByOwnerCustomerId(int customerId)
    {
        return _animals.Where(animal => animal.OwnerCustomerId == customerId).ToList();
    }

    public IReadOnlyList<Animal> SearchByNameOrChip(string searchText)
    {
        return _animals
            .Where(animal =>
                string.IsNullOrWhiteSpace(searchText) ||
                animal.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                animal.ChipNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

internal static class TestEmployees
{
    public static Employee Secretary() => new()
    {
        Id = 10,
        Username = "secret1",
        Role = StaffRole.Secretary
    };

    public static Employee Veterinarian() => new()
    {
        Id = 20,
        Username = "vetuser",
        Role = StaffRole.Veterinarian
    };
}

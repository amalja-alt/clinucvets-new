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

    public void Seed(Animal animal)
    {
        _animals.Add(animal);
        _nextId = Math.Max(_nextId, animal.Id + 1);
    }

    public bool ExistsById(int animalId) => _animals.Any(animal => animal.Id == animalId);

    public bool ExistsByChipNumber(string chipNumber) => _animals.Any(animal => animal.ChipNumber == chipNumber);

    public Animal Add(Animal animal)
    {
        Animal saved = new()
        {
            Id = _nextId++,
            Name = animal.Name,
            ChipNumber = animal.ChipNumber,
            CategoryId = animal.CategoryId,
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
            .Where(animal => animal.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) || animal.ChipNumber == searchText)
            .ToList();
    }
}

internal sealed class FakeVisitRepository : IVisitRepository
{
    private int _nextId = 1;

    public Visit Add(Visit visit)
    {
        Visit saved = new()
        {
            Id = _nextId++,
            AnimalId = visit.AnimalId,
            VeterinarianId = visit.VeterinarianId,
            VisitDateTime = visit.VisitDateTime,
            Reason = visit.Reason,
            Diagnosis = visit.Diagnosis,
            BaseVisitPrice = visit.BaseVisitPrice
        };
        saved.MedicinesGiven.AddRange(visit.MedicinesGiven);
        return saved;
    }
}

internal sealed class FakeMedicineRepository : IMedicineRepository
{
    private readonly List<Medicine> _medicines = [];
    private int _nextId = 1;

    public void Seed(Medicine medicine)
    {
        _medicines.Add(medicine);
        _nextId = Math.Max(_nextId, medicine.Id + 1);
    }

    public IReadOnlyList<Medicine> GetAll() => _medicines.ToList();

    public IReadOnlyList<Medicine> FindByIds(IEnumerable<int> medicineIds)
    {
        HashSet<int> ids = medicineIds.ToHashSet();
        return _medicines.Where(medicine => ids.Contains(medicine.Id)).ToList();
    }

    public Medicine Add(Medicine medicine)
    {
        Medicine saved = new()
        {
            Id = _nextId++,
            Name = medicine.Name,
            Price = medicine.Price,
            QuantityInStock = medicine.QuantityInStock
        };
        _medicines.Add(saved);
        return saved;
    }

    public bool Remove(int medicineId)
    {
        Medicine? medicine = _medicines.SingleOrDefault(item => item.Id == medicineId);
        return medicine is not null && _medicines.Remove(medicine);
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

using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Repositories.interfacesrepo;
using ClinicVets.Services;
using ClinicVets.Validators;
using Xunit;

namespace ClinicVets.Tests.UnitTests;

public class MedicineServiceTests
{
    [Fact]
    public void AddMedicine_WithValidData_ShouldAddMedicineSuccessfully()
    {
        // Arrange
        IMedicineRepository repository = new FakeMedicineRepository();
        MedicineValidator validator = new();
        MedicineService service = new(repository, validator);

        // Act
        OperationResult<Medicine> result =
            service.AddMedicine("Antibiotic", 50, 10);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Antibiotic", result.Value!.Name);
    }

    [Fact]
    public void AddMedicine_WithEmptyName_ShouldFail()
    {
        // Arrange
        IMedicineRepository repository = new FakeMedicineRepository();
        MedicineValidator validator = new();
        MedicineService service = new(repository, validator);

        // Act
        OperationResult<Medicine> result =
            service.AddMedicine("", 50, 10);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void RemoveMedicine_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        IMedicineRepository repository = new FakeMedicineRepository();
        MedicineValidator validator = new();
        MedicineService service = new(repository, validator);

        Medicine addedMedicine =
            service.AddMedicine("Painkiller", 30, 5).Value!;

        // Act
        bool result = service.RemoveMedicine(addedMedicine.Id);

        // Assert
        Assert.True(result);
    }
}

public class FakeMedicineRepository : IMedicineRepository
{
    private readonly List<Medicine> _medicines = [];
    private int _nextId = 1;

    public Medicine Add(Medicine medicine)
    {
        Medicine savedMedicine = new()
        {
            Id = _nextId++,
            Name = medicine.Name,
            Price = medicine.Price,
            QuantityInStock = medicine.QuantityInStock
        };

        _medicines.Add(savedMedicine);
        return savedMedicine;
    }

    public IReadOnlyList<Medicine> GetAll()
    {
        return _medicines;
    }

    public IReadOnlyList<Medicine> FindByIds(IEnumerable<int> medicineIds)
    {
        return _medicines
            .Where(m => medicineIds.Contains(m.Id))
            .ToList();
    }

    public bool Remove(int medicineId)
    {
        Medicine? medicine =
            _medicines.FirstOrDefault(m => m.Id == medicineId);

        if (medicine == null)
        {
            return false;
        }

        _medicines.Remove(medicine);
        return true;
    }
}

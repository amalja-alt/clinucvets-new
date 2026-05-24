using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Services;
using ClinicVets.Validators;
using Xunit;

namespace ClinicVets.Tests.UnitTests;

public class VisitServiceTests
{
    [Fact]
    public void OpenVisit_WithVeterinarian_ShouldSucceed()
    {
        // Arrange
        IVisitRepository visitRepository = new FakeVisitRepository();
        IAnimalRepository animalRepository = new FakeAnimalRepository();
        IMedicineRepository medicineRepository = new FakeMedicineRepository();

        VisitValidator validator = new();

        VisitService service = new(
            visitRepository,
            animalRepository,
            medicineRepository,
            validator);

        Employee veterinarian = new()
        {
            Id = 1,
            Role = StaffRole.Veterinarian
        };

        // Act
        OperationResult<Visit> result = service.OpenVisit(
            veterinarian,
            1,
            "Fever",
            "Healthy",
            []);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void OpenVisit_WithSecretary_ShouldFail()
    {
        // Arrange
        IVisitRepository visitRepository = new FakeVisitRepository();
        IAnimalRepository animalRepository = new FakeAnimalRepository();
        IMedicineRepository medicineRepository = new FakeMedicineRepository();

        VisitValidator validator = new();

        VisitService service = new(
            visitRepository,
            animalRepository,
            medicineRepository,
            validator);

        Employee secretary = new()
        {
            Id = 2,
            Role = StaffRole.Secretary
        };

        // Act
        OperationResult<Visit> result = service.OpenVisit(
            secretary,
            1,
            "Fever",
            "Healthy",
            []);

        // Assert
        Assert.False(result.IsSuccess);
    }
}

public class FakeVisitRepository : IVisitRepository
{
    private readonly List<Visit> _visits = [];
    private int _nextId = 1;

    public Visit Add(Visit visit)
    {
        Visit savedVisit = new()
        {
            Id = _nextId++,
            AnimalId = visit.AnimalId,
            VeterinarianId = visit.VeterinarianId,
            Reason = visit.Reason,
            Diagnosis = visit.Diagnosis,
            VisitDateTime = visit.VisitDateTime
        };

        _visits.Add(savedVisit);
        return savedVisit;
    }

    public IReadOnlyList<Visit> GetAll()
    {
        return _visits;
    }
}

public class FakeAnimalRepository : IAnimalRepository
{
    public bool ExistsById(int animalId)
    {
        return animalId == 1;
    }

    public bool ExistsByChipNumber(string chipNumber)
    {
        return false;
    }

    public Animal Add(Animal animal)
    {
        return animal;
    }

    public IReadOnlyList<Animal> FindByOwnerCustomerId(int customerId)
    {
        return [];
    }

    public IReadOnlyList<Animal> SearchByNameOrChip(string searchTerm)
    {
        return [];
    }
}
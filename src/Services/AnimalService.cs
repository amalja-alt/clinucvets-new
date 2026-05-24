using ClinicVets.Models;
using ClinicVets.Repositories.interfacesrepo;
using ClinicVets.Validators;

namespace ClinicVets.Services;

public class AnimalService(
    IAnimalRepository animalRepository,
    ICustomerRepository customerRepository,
    AnimalValidator animalValidator)
{
    public OperationResult<Animal> AddAnimal(
        string name,
        string chipNumber,
        AnimalType type,
        decimal weightKg,
        DateOnly birthDate,
        DateOnly lastVaccinationDate,
        int ownerCustomerId)
    {
        OperationResult<bool> validationResult = animalValidator.ValidateAnimal(name, weightKg, birthDate);

        if (!validationResult.IsSuccess)
        {
            return OperationResult<Animal>.Failure(validationResult.ErrorMessage);
        }

        if (customerRepository.FindById(ownerCustomerId) is null)
        {
            return OperationResult<Animal>.Failure(ValidationMessages.AnimalOwnerRequired);
        }

        if (animalRepository.ExistsByChipNumber(chipNumber))
        {
            return OperationResult<Animal>.Failure(ValidationMessages.DuplicateChipNumber);
        }

        Animal animal = new()
        {
            Name = name,
            ChipNumber = chipNumber,
            Type = type,
            WeightKg = weightKg,
            BirthDate = birthDate,
            LastVaccinationDate = lastVaccinationDate,
            OwnerCustomerId = ownerCustomerId
        };

        Animal savedAnimal = animalRepository.Add(animal);
        return OperationResult<Animal>.Success(savedAnimal);
    }

    public IReadOnlyList<Animal> SearchByNameOrChip(string searchText)
    {
        return animalRepository.SearchByNameOrChip(searchText);
    }

    public bool NeedsAnnualVaccination(Animal animal)
    {
        return animal.LastVaccinationDate.AddYears(1) <= DateOnly.FromDateTime(DateTime.Today);
    }
}

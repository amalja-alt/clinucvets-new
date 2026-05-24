using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Validators;

namespace ClinicVets.Services;

public class VisitService(
    IVisitRepository visitRepository,
    IAnimalRepository animalRepository,
    IMedicineRepository medicineRepository,
    VisitValidator visitValidator)
{
    public OperationResult<Visit> OpenVisit(
        Employee currentUser,
        int animalId,
        string reason,
        string diagnosis,
        IEnumerable<int> medicineIds)
    {
        if (currentUser.Role != StaffRole.Veterinarian)
        {
            return OperationResult<Visit>.Failure(ValidationMessages.VeterinarianOnly);
        }

        OperationResult<bool> validationResult = visitValidator.ValidateVisit(animalId, reason);

        if (!validationResult.IsSuccess)
        {
            return OperationResult<Visit>.Failure(validationResult.ErrorMessage);
        }

        if (!animalRepository.ExistsById(animalId))
        {
            return OperationResult<Visit>.Failure(ValidationMessages.AnimalNotFound);
        }

        IReadOnlyList<Medicine> medicines = medicineRepository.FindByIds(medicineIds);

        Visit visit = new()
        {
            AnimalId = animalId,
            VeterinarianId = currentUser.Id,
            Reason = reason.Trim(),
            Diagnosis = diagnosis.Trim(),
            VisitDateTime = DateTime.Now
        };

        visit.MedicinesGiven.AddRange(medicines);
        Visit savedVisit = visitRepository.Add(visit);
        return OperationResult<Visit>.Success(savedVisit);
    }
}

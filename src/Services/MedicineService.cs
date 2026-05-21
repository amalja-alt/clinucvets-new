using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Validators;

namespace ClinicVets.Services;

public class MedicineService(IMedicineRepository medicineRepository, MedicineValidator medicineValidator)
{
    public OperationResult<Medicine> AddMedicine(string name, decimal price, int quantityInStock)
    {
        OperationResult<bool> validationResult = medicineValidator.ValidateMedicine(name, price, quantityInStock);
        if (!validationResult.IsSuccess)
        {
            return OperationResult<Medicine>.Failure(validationResult.ErrorMessage);
        }

        Medicine medicine = new()
        {
            Name = name.Trim(),
            Price = price,
            QuantityInStock = quantityInStock
        };

        Medicine savedMedicine = medicineRepository.Add(medicine);
        return OperationResult<Medicine>.Success(savedMedicine);
    }

    public bool RemoveMedicine(int medicineId)
    {
        return medicineRepository.Remove(medicineId);
    }

    public IReadOnlyList<Medicine> GetAllMedicines() => medicineRepository.GetAll();
}

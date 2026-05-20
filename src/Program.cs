using ClinicVets.Data;
using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Services;
using ClinicVets.UI;
using ClinicVets.Validators;

namespace ClinicVets;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        ClinicDatabaseInitializer databaseInitializer = new(DatabaseSettings.ConnectionString);
        databaseInitializer.Initialize();

        EmployeeValidator employeeValidator = new();
        CustomerValidator customerValidator = new();
        AnimalValidator animalValidator = new();
        AnimalCategoryValidator animalCategoryValidator = new();
        MedicineValidator medicineValidator = new();
        VisitValidator visitValidator = new();

        IEmployeeRepository employeeRepository = new SqliteEmployeeRepository(DatabaseSettings.ConnectionString);
        ICustomerRepository customerRepository = new CustomerRepository(DatabaseSettings.ConnectionString);
        IAnimalRepository animalRepository = new AnimalRepository(DatabaseSettings.ConnectionString);
        IAnimalCategoryRepository animalCategoryRepository = new AnimalCategoryRepository(DatabaseSettings.ConnectionString);
        IMedicineRepository medicineRepository = new MedicineRepository(DatabaseSettings.ConnectionString);
        IVisitRepository visitRepository = new VisitRepository(DatabaseSettings.ConnectionString);
        IClinicLookupRepository lookupRepository = new ClinicLookupRepository(DatabaseSettings.ConnectionString);

        EmployeeService employeeService = new(employeeRepository, employeeValidator);
        AuthService authService = new(employeeRepository, employeeValidator);
        CustomerService customerService = new(customerRepository, animalRepository, customerValidator);
        AnimalService animalService = new(animalRepository, customerRepository, animalValidator);
        AnimalCategoryService animalCategoryService = new(animalCategoryRepository, animalCategoryValidator);
        MedicineService medicineService = new(medicineRepository, medicineValidator);
        VisitService visitService = new(visitRepository, animalRepository, medicineRepository, visitValidator);
        ClinicLookupService lookupService = new(lookupRepository, animalRepository);
        VaccineAlertService vaccineAlertService = new();
        ClinicAppServices appServices = new(
            authService,
            employeeService,
            customerService,
            animalService,
            animalCategoryService,
            medicineService,
            visitService,
            lookupService,
            vaccineAlertService);

        Application.Run(new LoginForm(appServices));
    }
}

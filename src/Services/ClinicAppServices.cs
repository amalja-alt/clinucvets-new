namespace ClinicVets.Services;

public class ClinicAppServices(
    AuthService authService,
    EmployeeService employeeService,
    CustomerService customerService,
    AnimalService animalService,
    AnimalCategoryService animalCategoryService,
    MedicineService medicineService,
    VisitService visitService,
    ClinicLookupService lookupService,
    VaccineAlertService vaccineAlertService)
{
    public AuthService AuthService { get; } = authService;
    public EmployeeService EmployeeService { get; } = employeeService;
    public CustomerService CustomerService { get; } = customerService;
    public AnimalService AnimalService { get; } = animalService;
    public AnimalCategoryService AnimalCategoryService { get; } = animalCategoryService;
    public MedicineService MedicineService { get; } = medicineService;
    public VisitService VisitService { get; } = visitService;
    public ClinicLookupService LookupService { get; } = lookupService;
    public VaccineAlertService VaccineAlertService { get; } = vaccineAlertService;
}

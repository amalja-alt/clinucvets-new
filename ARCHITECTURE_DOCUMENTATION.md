# ClinicVets Architecture and Testing Guide

This document describes the current ClinicVets implementation for the Software Testing course project. It is intentionally aligned with the code that exists now and keeps the assignment focus separate from broader project modules.

ClinicVets is a C# WinForms veterinary clinic management system with two staff roles only:

- `Secretary`
- `Veterinarian`

There is no third staff role in the current system.

## My Assignment Scope

The main assignment scope is:

1. Employee login and employee registration.
2. Customer management for animal owners.

Employee login and registration includes SQLite-backed login, authentication flow, employee registration, username validation, password validation, employee ID validation, email validation, Israeli ID validation, and role selection for `Secretary` or `Veterinarian`. Passwords are stored as entered for the current course implementation.

Customer management is secretary-only. A `Secretary` can register customers, search customers by Israeli ID or phone number, and display animals linked to a customer. A `Veterinarian` must not register, search, or manage customer information.

## Other Existing Modules

Animal, animal-category, medicine, visit, dashboard, and lookup modules exist in the codebase for the broader ClinicVets application. They are not the main assignment responsibility, except where they support displaying animals linked to a customer.

## Layered Architecture

The implementation follows this dependency direction:

```text
UI Forms -> Services -> Validators and Repositories -> SQLite
```

### UI Layer

Location: `src/UI`

Main assignment forms:

- `LoginForm`
- `RegisterEmployeeForm`
- `SecretaryDashboardForm`
- `CustomerForm`

Other existing forms:

- `VeterinarianDashboardForm`
- `AnimalForm`
- `AnimalCategoryForm`
- `MedicineForm`
- `VisitForm`
- `VisitsOverviewForm`
- `UiTheme`

UI responsibilities:

- collect input from WinForms controls,
- display validation and workflow messages,
- open the correct dashboard after login,
- call services for every business action.

UI forms must not open SQLite connections, write SQL, compare passwords, or duplicate validation rules.

### Service Layer

Location: `src/Services`

Main assignment services:

- `AuthService`
- `EmployeeService`
- `CustomerService`

Other existing services:

- `AnimalService`
- `AnimalCategoryService`
- `MedicineService`
- `VisitService`
- `ClinicLookupService`
- `VaccineAlertService`

Service responsibilities:

- coordinate workflows,
- enforce role permissions,
- call validators,
- call repositories,
- return `OperationResult<T>` or `AuthenticationResult` as testable oracles.

Examples:

- `AuthService.Login` validates login input, loads an employee by username, compares the saved password, and stores `CurrentUser`.
- `EmployeeService.RegisterEmployee` validates registration input, checks duplicate employee fields, and saves the employee.
- `CustomerService.RegisterCustomer` allows only a `Secretary` to add customers.
- `CustomerService.SearchByIdentityOrPhone` allows only a `Secretary` to search customer records.
- `VisitService.OpenVisit` allows only a `Veterinarian` to open visits.

### Validator Layer

Location: `src/Validators`

Main assignment validators:

- `ValidationRules`
- `EmployeeValidator`
- `CustomerValidator`

Other existing validators:

- `AnimalValidator`
- `AnimalCategoryValidator`
- `MedicineValidator`
- `VisitValidator`

Validation rules are centralized so they can be tested without opening GUI screens. This supports unit testing, equivalence-class testing, boundary-value analysis, and regression testing.

Current important rules:

- Username: 6-8 English letters/digits, at most 2 digits.
- Password: 8-10 characters, at least one letter, one digit, and one of `!`, `#`, `$`.
- Employee number: exactly 4 digits.
- Israeli ID: exactly 9 numeric digits.
- Email: basic email format.
- Phone: starts with `0` and contains 9 or 10 digits total.
- Customer full name: letters only, English or Hebrew, with optional spaces.

### Repository Layer

Location: `src/Repositories`

Repositories hide SQLite access behind interfaces:

- `IEmployeeRepository` / `SqliteEmployeeRepository`
- `ICustomerRepository` / `CustomerRepository`
- `IAnimalRepository` / `AnimalRepository`
- `IAnimalCategoryRepository` / `AnimalCategoryRepository`
- `IMedicineRepository` / `MedicineRepository`
- `IVisitRepository` / `VisitRepository`
- `IClinicLookupRepository` / `ClinicLookupRepository`

Repositories are responsible for:

- opening SQLite connections,
- enabling foreign keys,
- using parameterized SQL,
- mapping rows to model objects,
- keeping SQL out of services and UI.

### Data Layer

Location: `src/Data`

`ClinicDatabaseInitializer` creates the SQLite schema and seed data. The database is created as `clinicvets.db` in the application output folder.

Created tables:

- `Roles`
- `Employees`
- `Customers`
- `AnimalCategories`
- `Animals`
- `Medicines`
- `Visits`
- `VisitMedicines`

Seeded roles:

- `Veterinarian`
- `Secretary`

## Startup Flow

Startup happens in `Program.Main`:

1. Initialize WinForms.
2. Create and initialize the SQLite database.
3. Create validators.
4. Create SQLite repositories.
5. Create services.
6. Group services inside `ClinicAppServices`.
7. Open `LoginForm`.

## Authentication Flow

1. `LoginForm` collects username and password.
2. `LoginForm` calls `AuthService.Login`.
3. `AuthService` validates the login input.
4. `AuthService` loads the employee through `IEmployeeRepository`.
5. The entered password is compared with the saved database password value.
6. On success, `CurrentUser` is set.
7. `LoginForm` opens:
   - `SecretaryDashboardForm` for a secretary,
   - `VeterinarianDashboardForm` for a veterinarian.
8. When the dashboard closes, `LoginForm` logs the user out and clears the password field.

## Employee Registration Flow

`RegisterEmployeeForm` collects:

- username,
- password,
- employee number,
- email,
- Israeli ID,
- role: `Secretary` or `Veterinarian`.

The form calls `EmployeeService.RegisterEmployee`.

`EmployeeService.RegisterEmployee`:

1. validates all registration fields through `EmployeeValidator`,
2. checks duplicates through `IEmployeeRepository`,
3. stores the entered password value on the employee model,
4. saves the employee through the repository.

## Customer Management Flow

Customer management belongs to the secretary workflow.

`CustomerForm` supports:

- registering a customer,
- searching by Israeli ID or phone,
- displaying customer details,
- displaying all animals linked to the customer.

`CustomerService` enforces the permission rule:

- `Secretary` can register and search customers.
- `Veterinarian` cannot access customer management actions through the service.

This rule is enforced in the service layer, not only in the UI.

## Role-Based Navigation

### SecretaryDashboardForm

For the assignment scope, the secretary dashboard is important because it is the entry point to customer management:

- dashboard summary,
- customers.

Other existing navigation items such as appointments, visits, and pets/animals support the broader application.

### VeterinarianDashboardForm

The veterinarian dashboard is an existing broader-module screen. For this assignment, the important point is that veterinarians must not manage customer records through `CustomerService`.

- today's patients,
- appointments,
- patients/animals,
- medical records,
- treatments,
- prescriptions/medicine.

## Validation and Testability

The project is designed so testing can be split by level:

- Unit tests: `ValidationRules`, module validators, and small service decisions.
- Functional/service tests: `AuthService`, `EmployeeService`, `CustomerService`, using fake repositories.
- Integration tests: SQLite repositories and database initializer.
- GUI tests: WinForms behavior such as error messages, role navigation, and form-level flows.

`OperationResult<T>` and `AuthenticationResult` act as test oracles because they expose:

- success/failure,
- returned value,
- clear error message.

## Software Testing Course Mapping

### Black Box Testing

Use documented input rules to derive equivalence classes and boundary values without looking at implementation.

Examples:

- username length 5, 6, 8, 9,
- password length 7, 8, 10, 11,
- employee number length 3, 4, 5,
- Israeli ID exactly 9 numeric digits vs too short, too long, or non-numeric,
- phone starts with `0` vs does not start with `0`.

### White Box Testing

Use service and validator control flow to cover branches.

Good CFG candidates:

- `AuthService.Login`
- `EmployeeValidator.ValidateRegistration`
- `CustomerValidator.ValidateCustomer`
- `CustomerService.RegisterCustomer`

### Decision Tables

Good decision-table candidates:

- Login: valid format, username exists, password matches.
- Employee registration: each field valid, duplicate exists, selected role.
- Customer registration: role is secretary, fields valid, duplicate customer exists.

### Risk-Based Testing

Highest-risk areas:

- authentication,
- password comparison during login,
- Israeli ID format validation,
- role-based access control,
- customer search correctness,
- database duplicate constraints.

## Current Testing Structure

The automated tests live in:

```text
tests/ClinicVets.Tests
```

Current test groups:

- validation unit tests,
- validator oracle tests,
- functional service tests with fake repositories.

Run tests:

```powershell
dotnet test tests\ClinicVets.Tests\ClinicVets.Tests.csproj
```

## Architecture Rules

1. Keep business logic out of WinForms.
2. Keep SQL inside repositories.
3. Keep validation rules in validators.
4. Keep password comparison inside `AuthService`, not in UI forms.
5. Enforce permissions in services.
6. Keep roles limited to `Secretary` and `Veterinarian`.
7. Use repository interfaces for testability and future mocking.
8. Prefer small, deterministic functions for validation and service rules.
9. Add tests before changing behavior in risky workflows.

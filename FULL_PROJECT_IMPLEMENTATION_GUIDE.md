# ClinicVets Full Project Implementation Guide

This document explains the current ClinicVets implementation for developers, testers, and reviewers. It clearly separates the course assignment responsibility from broader existing project modules.

## My Assignment Scope

The implementation/testing responsibility for this course assignment is mainly:

1. employee login and employee registration,
2. secretary-only customer management for animal owners.

Employee login and registration includes SQLite-backed login, authentication flow, employee registration, username validation, password validation, employee ID validation, email validation, Israeli ID validation, and role selection for `Secretary` or `Veterinarian`. Passwords are stored as entered for the current course implementation.

Customer management includes registering customers, searching by Israeli ID or phone number, and displaying animals linked to a customer. Customer management is restricted to `Secretary` users. `Veterinarian` users must not register, search, or manage customer information.

## Other Existing Modules

The codebase also contains animal, animal-category, medicine, visit, dashboard, and lookup modules. These are part of the broader ClinicVets application. They should not be treated as the main assignment scope, except where linked animals are displayed from the customer-management flow.

## 1. Project Overview

ClinicVets is a veterinary clinic management system written in C# with a WinForms GUI. It uses SQLite for local persistent storage and xUnit for automated tests.

The project was developed in the context of a Software Testing / Software Quality Assurance course. Because of that, the implementation emphasizes:

- clear separation between UI, logic, validation, and persistence,
- reusable validation rules,
- service-level authorization,
- deterministic automated tests,
- support for boundary testing, equivalence-class testing, functional testing, regression testing, and decision-table testing.

Main technologies:

- C#
- .NET Windows target
- WinForms
- SQLite through `Microsoft.Data.Sqlite`
- xUnit test project
- layered architecture

The application entry point is `src/Program.cs`.

## 2. Current Implemented Roles

The system currently contains two roles only:

- `Secretary`
- `Veterinarian`

The roles are defined in `src/Models/StaffRole.cs`.

### Secretary

Main assignment capabilities for secretary:

- log in through `LoginForm`,
- open `SecretaryDashboardForm`,
- register customers through `CustomerForm`,
- search customers by Israeli ID or phone,
- view customer details,
- view animals linked to a customer.

Customer-management write and search operations are enforced in `CustomerService`, not only in the UI.

### Veterinarian

Assignment-relevant veterinarian behavior:

- log in through `LoginForm`,
- open `VeterinarianDashboardForm`,
- cannot register customers,
- cannot search customers,
- cannot manage customer information through `CustomerService`.

Other patient, visit, treatment, and medicine screens are broader existing modules.

## 3. High-Level Architecture

The project uses a layered architecture:

```text
WinForms UI
    |
    v
Services
    |
    +--> Validators
    |
    v
Repositories
    |
    v
SQLite database
```

More specifically:

```text
UI Forms -> Services -> Validators / Repositories -> SQLite
```

### UI Layer

Location: `src/UI`

Responsibilities:

- render WinForms screens,
- collect user input,
- display success/error messages,
- call service methods,
- open the correct dashboard and embedded views.

Important classes:

- `LoginForm`
- `RegisterEmployeeForm`
- `SecretaryDashboardForm`
- `VeterinarianDashboardForm`
- `CustomerForm`
- `AnimalForm`
- `VisitForm`
- `VisitsOverviewForm`
- `MedicineForm`
- `AnimalCategoryForm`
- `UiTheme`

The UI does not write SQL directly and does not own business rules.

### Service Layer

Location: `src/Services`

Responsibilities:

- implement application workflows,
- enforce authorization rules,
- coordinate validation and repository calls,
- return testable result objects.

Important classes:

- `AuthService`
- `EmployeeService`
- `CustomerService`
- `AnimalService`
- `VisitService`
- `ClinicLookupService`
- `ClinicAppServices`
- `OperationResult<T>`
- `AuthenticationResult`
- `ValidationMessages`

### Validator Layer

Location: `src/Validators`

Responsibilities:

- centralize input validation,
- expose deterministic rules suitable for unit testing,
- keep validation outside WinForms controls.

Important classes:

- `ValidationRules`
- `EmployeeValidator`
- `CustomerValidator`
- `AnimalValidator`
- `VisitValidator`
- `MedicineValidator`

### Repository Layer

Location: `src/Repositories`

Responsibilities:

- hide SQLite access from services and UI,
- use parameterized SQL,
- convert database rows into model objects,
- expose interfaces for testability.

Examples:

- `IEmployeeRepository` / `SqliteEmployeeRepository`
- `ICustomerRepository` / `CustomerRepository`
- `IAnimalRepository` / `AnimalRepository`
- `IVisitRepository` / `VisitRepository`
- `IMedicineRepository` / `MedicineRepository`
- `IClinicLookupRepository` / `ClinicLookupRepository`

### Models Layer

Location: `src/Models`

Models are simple data objects used between layers.

Examples:

- `Employee`
- `Customer`
- `Animal`
- `Visit`
- `Medicine`
- `StaffRole`

### Data Layer

Location: `src/Data`

Responsibilities:

- define the SQLite database file location,
- create tables,
- seed required lookup/demo data.

Important classes:

- `DatabaseSettings`
- `ClinicDatabaseInitializer`

## 4. Detailed Data Flow

### Login Flow

Relevant files:

- `src/UI/LoginForm.cs`
- `src/Services/AuthService.cs`
- `src/Repositories/IEmployeeRepository.cs`
- `src/Repositories/SqliteEmployeeRepository.cs`

Flow:

1. The user enters username and password in `LoginForm`.
2. `LoginForm.LoginButton_Click` calls `_services.AuthService.Login(...)`.
3. `AuthService.Login` calls `EmployeeValidator.ValidateLogin`.
4. If validation fails, `AuthenticationResult.Failure` is returned to the form.
5. If validation passes, `AuthService` calls `IEmployeeRepository.FindByUsername`.
6. `SqliteEmployeeRepository` reads the employee from SQLite by username.
7. `AuthService` compares the entered password with the saved database password value.
8. If the employee does not exist or the password is wrong, login fails with `ValidationMessages.WrongCredentials`.
9. If login succeeds, `AuthService.CurrentUser` is set.
10. `LoginForm` opens:
    - `VeterinarianDashboardForm` for `StaffRole.Veterinarian`,
    - `SecretaryDashboardForm` for `StaffRole.Secretary`.
11. After the dashboard closes, the login form logs out and clears the password box.

### Register Employee Flow

Relevant files:

- `src/UI/RegisterEmployeeForm.cs`
- `src/Services/EmployeeService.cs`
- `src/Validators/EmployeeValidator.cs`
- `src/Validators/ValidationRules.cs`
- `src/Repositories/IEmployeeRepository.cs`
- `src/Repositories/SqliteEmployeeRepository.cs`

Flow:

1. The user opens `RegisterEmployeeForm`.
2. The form collects:
   - username,
   - password,
   - employee number,
   - email,
   - Israeli ID,
   - role (`Secretary` or `Veterinarian`).
3. The form calls `EmployeeService.RegisterEmployee`.
4. `EmployeeService` calls `EmployeeValidator.ValidateRegistration`.
5. `EmployeeValidator` delegates low-level checks to `ValidationRules`.
6. If validation fails, an `OperationResult<Employee>.Failure` is returned with a clear validation message.
7. If validation passes, `EmployeeService` calls `IEmployeeRepository.ExistsByRegistrationFields`.
8. If a duplicate username, employee number, email, or identity number exists, registration fails.
9. If no duplicate exists, `EmployeeService` stores the entered password value on the employee model.
10. `EmployeeService` creates an `Employee` model.
11. `SqliteEmployeeRepository.Add` inserts the employee into SQLite.
12. The saved employee is returned to the form.
13. The form displays success and clears the input fields.

### Register Customer Flow

Relevant files:

- `src/UI/CustomerForm.cs`
- `src/Services/CustomerService.cs`
- `src/Validators/CustomerValidator.cs`
- `src/Validators/ValidationRules.cs`
- `src/Repositories/ICustomerRepository.cs`
- `src/Repositories/CustomerRepository.cs`

Flow:

1. A logged-in user opens `CustomerForm` through the secretary dashboard.
2. The form checks the current user role for UI behavior.
3. The form collects:
   - full name,
   - Israeli ID,
   - phone,
   - email.
4. `CustomerForm.AddCustomer` calls `CustomerService.RegisterCustomer`.
5. `CustomerService` first checks authorization with `CanManageCustomers`.
6. Only `StaffRole.Secretary` is allowed.
7. If the role is not secretary or the user is null, the service returns a failure.
8. If authorized, `CustomerValidator.ValidateCustomer` validates customer input.
9. If validation passes, `CustomerService` checks duplicate identity number through `ICustomerRepository.ExistsByIdentityNumber`.
10. If no duplicate exists, a `Customer` model is created.
11. `CustomerRepository.Add` inserts the customer into SQLite.
12. The form displays success, clears input fields, and calls `DisplayCustomer`.

### Search Customer Flow

Relevant files:

- `src/UI/CustomerForm.cs`
- `src/Services/CustomerService.cs`
- `src/Repositories/CustomerRepository.cs`
- `src/Repositories/AnimalRepository.cs`

Flow:

1. The user enters a search value in `CustomerForm`.
2. The search can be an Israeli ID or phone number.
3. The form checks for empty input.
4. `CustomerForm.SearchCustomer` calls `CustomerService.SearchByIdentityOrPhone`.
5. `CustomerService` checks that the current user is a secretary.
6. If unauthorized, the service returns a failure.
7. If authorized, `CustomerRepository.FindByIdentityOrPhone` queries SQLite.
8. The repository returns a `Customer` model or `null`.
9. If no customer is found, the UI clears details and shows a "not found" status.
10. If a customer is found, `CustomerForm.DisplayCustomer` shows customer details.
11. `DisplayCustomer` calls `CustomerService.GetCustomerAnimals`.
12. `CustomerService` again checks secretary authorization.
13. `AnimalRepository.FindByOwnerCustomerId` returns linked animals.
14. The UI fills the animals list.

## 5. SQLite Structure

The database file is named:

```text
clinicvets.db
```

It is created in:

```text
AppContext.BaseDirectory
```

The path and connection string are defined in `src/Data/DatabaseSettings.cs`.

The schema is created by `src/Data/ClinicDatabaseInitializer.cs`.

Repositories communicate with SQLite through `Microsoft.Data.Sqlite`. Repository methods open a connection, enable foreign keys, execute parameterized SQL, and return model objects.

### Tables

| Table | Purpose |
|---|---|
| `Roles` | Stores supported roles: `Veterinarian`, `Secretary`. |
| `Employees` | Stores clinic staff login and registration data. |
| `Customers` | Stores animal owners. |
| `AnimalCategories` | Stores animal category lookup rows. |
| `Animals` | Stores animal patient records. |
| `Medicines` | Stores medicine inventory. |
| `Visits` | Stores veterinarian visit records. |
| `VisitMedicines` | Join table between visits and medicines. |

### Key Relationships

- `Employees.RoleId` references `Roles.Id`.
- `Animals.OwnerCustomerId` references `Customers.Id`.
- `Animals.CategoryId` references `AnimalCategories.Id`.
- `Visits.AnimalId` references `Animals.Id`.
- `Visits.VeterinarianEmployeeId` references `Employees.Id`.
- `VisitMedicines.VisitId` references `Visits.Id`.
- `VisitMedicines.MedicineId` references `Medicines.Id`.

Important uniqueness rules include:

- employee username,
- employee number,
- employee email,
- employee identity number,
- customer identity number,
- customer email,
- animal chip number,
- medicine name.

## 6. Validation System

Validation is centralized in `src/Validators`.

### ValidationRules

`ValidationRules` contains low-level reusable checks:

- username format,
- password format,
- employee number format,
- Israeli ID format,
- email format,
- phone format,
- name format,
- animal weight,
- animal birth date.

### EmployeeValidator

`EmployeeValidator` validates:

- login input,
- employee registration input.

Employee registration rules:

- Username: 6-8 English letters/digits.
- Username may contain at most 2 digits.
- Password: 8-10 characters.
- Password must contain at least one letter.
- Password must contain at least one digit.
- Password must contain at least one of `!`, `#`, `$`.
- Employee number: exactly 4 digits.
- Email: valid email format.
- Israeli ID: exactly 9 numeric digits.

### CustomerValidator

`CustomerValidator` validates:

- full name,
- Israeli ID,
- phone,
- email.

Customer rules:

- Full name: English or Hebrew letters with optional spaces.
- Israeli ID: exactly 9 numeric digits.
- Phone: starts with `0` and contains 9 or 10 digits total.
- Email: valid email format.

### Validation Flow

Validation normally happens inside services:

- `EmployeeService.RegisterEmployee` calls `EmployeeValidator`.
- `AuthService.Login` calls `EmployeeValidator.ValidateLogin`.
- `CustomerService.RegisterCustomer` calls `CustomerValidator`.
- `AnimalService.AddAnimal` calls `AnimalValidator`.
- `VisitService.OpenVisit` calls `VisitValidator`.

The UI may show hints, but validators are the source of truth.

## 7. Authentication and Authorization

### Password Storage

Employee passwords are stored as entered for the current course implementation.

The SQLite column is named `PasswordHash` because that is the existing schema name, but the current stored value is the entered password text.

### Authentication

Authentication is handled by `AuthService`.

`AuthService.Login`:

1. validates username/password input,
2. loads the employee by username,
3. compares the entered password with the saved database value,
4. stores the logged-in employee in `CurrentUser`.

### Authorization

Authorization rules are implemented mainly in services.

Customer management:

- `CustomerService.RegisterCustomer`
- `CustomerService.SearchByIdentityOrPhone`
- `CustomerService.GetCustomerAnimals`

These require:

```csharp
currentUser?.Role == StaffRole.Secretary
```

Visit opening:

- `VisitService.OpenVisit`

This requires:

```csharp
currentUser.Role == StaffRole.Veterinarian
```

The UI also disables or closes screens in some cases, but service checks are the important protection boundary.

## 8. Dashboard Navigation System

After login, the application opens a dashboard based on role:

- `SecretaryDashboardForm`
- `VeterinarianDashboardForm`

Both dashboards use a sidebar-style navigation approach and a main content panel.

Implemented pattern:

1. The dashboard creates a shell layout.
2. Sidebar items trigger navigation actions.
3. Some screens are opened as embedded child forms.
4. The dashboard sets:
   - `TopLevel = false`,
   - `FormBorderStyle = None`,
   - `Dock = Fill`.
5. The current content panel is cleared.
6. The selected form/view is added to the dashboard content area.

This avoids scattering many independent windows during normal dashboard navigation and keeps the workflow centered in one dashboard window after login.

For the assignment, dashboard navigation matters mainly because:

- secretary users can reach `CustomerForm`,
- veterinarian users must not manage customers.

Other dashboard sections are broader application modules.

Current examples:

- `SecretaryDashboardForm.OpenEmbeddedForm`
- `VeterinarianDashboardForm.OpenEmbeddedForm`

## 9. UI/UX Implementation Notes

The UI uses WinForms layout containers to improve maintainability and scaling:

- `TableLayoutPanel` for structured rows/columns,
- `FlowLayoutPanel` for lists and groups of actions,
- `AutoScroll` in dashboard content areas,
- `Dock` and `Anchor` properties for resizing behavior,
- centralized styling through `UiTheme`.

Assignment forms with notable layout work:

- `LoginForm`: centered login card and role-based dashboard transition.
- `RegisterEmployeeForm`: structured registration form with field hints.
- `CustomerForm`: customer add/search/details/animals layout.

Other existing forms:

- `SecretaryDashboardForm`: broader sidebar navigation and embedded content.
- `VeterinarianDashboardForm`: broader patient sections and visit-related actions.

Important note: some veterinarian dashboard action cards are visual workflow placeholders where no action is wired yet. The documentation and tests do not treat those as completed service features.

## 10. Testing Architecture

Automated tests are in:

```text
tests/ClinicVets.Tests
```

The project uses xUnit.

The test project references the main application project and uses fake repositories from `TestSupport` to avoid depending on the real SQLite database.

### Test Folder Organization

```text
tests/ClinicVets.Tests/
  UnitTests/
  BoundaryTests/
  EquivalenceClassTests/
  AuthorizationTests/
  FunctionalTests/
  RegressionTests/
  DecisionTableTests/
  TestSupport/
```

### UnitTests

Purpose:

- test small deterministic units,
- focus on validators and validation rules.

Files:

- `UnitTests/ValidationRulesTests.cs`
- `UnitTests/EmployeeValidatorTests.cs`
- `UnitTests/CustomerValidatorTests.cs`

### BoundaryTests

Purpose:

- verify boundary value analysis from the course.

Covered boundaries:

- username length: 5, 6, 8, 9,
- password length: 7, 8, 10, 11,
- employee ID length: 3, 4, 5,
- Israeli ID length: 8, 9, 10,
- phone length: 8, 9, 10, 11.

### EquivalenceClassTests

Purpose:

- test representative valid and invalid partitions.

Examples:

- valid username,
- too many username digits,
- invalid username characters,
- valid password,
- password missing required groups,
- valid/invalid phone,
- valid/invalid email,
- valid/invalid customer name.

### AuthorizationTests

Purpose:

- verify role-based access rules.

Assignment-focused examples:

- secretary can register/search customers,
- veterinarian cannot register customers,
- veterinarian cannot search customers,
- veterinarian cannot view linked customer animals through customer management,
- null user cannot perform restricted customer actions.

### FunctionalTests

Purpose:

- test service-level workflows end to end within the service layer.

Files:

- `AuthServiceFunctionalTests.cs`
- `EmployeeServiceFunctionalTests.cs`
- `CustomerServiceFunctionalTests.cs`

Examples:

- valid login succeeds,
- wrong password fails,
- unknown username fails,
- register employee succeeds,
- duplicate employee fails,
- register customer succeeds,
- duplicate customer fails,
- search by ID/phone succeeds,
- not found search returns success with null value.

### RegressionTests

Purpose:

- verify that core flows still work after changes.

Assignment-focused covered flows:

- login,
- employee registration validation,
- customer registration,
- customer search.

### DecisionTableTests

Purpose:

- represent decision-table combinations from the course material.

Employee registration rows:

- all fields valid and no duplicate => success,
- invalid username => fail,
- invalid password => fail,
- invalid employee ID => fail,
- invalid email => fail,
- invalid Israeli ID => fail,
- duplicate employee => fail.

Customer registration rows:

- secretary + valid data + no duplicate => success,
- veterinarian + valid data => fail,
- secretary + invalid data => fail,
- secretary + duplicate identity => fail.

### TestSupport

Purpose:

- provide fake repositories and reusable test users.

File:

- `TestSupport/TestDoubles.cs`

This supports isolation and avoids real database dependency in service tests.

## 11. Mapping to Assignment Requirements

### Employee Login

Requirement:

- login with username and password against stored employee data.

Implemented by:

- UI: `src/UI/LoginForm.cs`
- Service: `src/Services/AuthService.cs`
- Repository: `src/Repositories/IEmployeeRepository.cs`
- SQLite implementation: `src/Repositories/SqliteEmployeeRepository.cs`
- Password comparison: `src/Services/AuthService.cs`
- Tests: `FunctionalTests/AuthServiceFunctionalTests.cs`, `RegressionTests/CoreRegressionTests.cs`

### Employee Registration

Requirement:

- register a new employee with username, password, employee ID, email, Israeli ID, and role.

Implemented by:

- UI: `src/UI/RegisterEmployeeForm.cs`
- Service: `src/Services/EmployeeService.cs`
- Validator: `src/Validators/EmployeeValidator.cs`
- Low-level rules: `src/Validators/ValidationRules.cs`
- Repository: `src/Repositories/SqliteEmployeeRepository.cs`
- SQLite table: `Employees`
- Tests:
  - `UnitTests/EmployeeValidatorTests.cs`
  - `BoundaryTests/EmployeeBoundaryTests.cs`
  - `EquivalenceClassTests/EmployeeEquivalenceClassTests.cs`
  - `FunctionalTests/EmployeeServiceFunctionalTests.cs`
  - `DecisionTableTests/RegisterEmployeeDecisionTableTests.cs`

### Username Rules

Requirement:

- 6-8 characters,
- at most 2 digits,
- remaining characters are English letters.

Implemented by:

- `ValidationRules.IsUsernameValid`

### Password Rules

Requirement:

- 8-10 characters,
- at least one letter,
- at least one digit,
- at least one special character from `!`, `$`, `#`.

Implemented by:

- `ValidationRules.IsPasswordValid`

### Employee ID

Requirement:

- exactly 4 digits.

Implemented by:

- `ValidationRules.IsEmployeeNumberValid`

### Israeli ID

Requirement:

- exactly 9 numeric digits.

Implemented by:

- `ValidationRules.IsIdentityNumberValid`

No checksum validation is implemented because the assignment-level requirement is format/length only.

### Email

Requirement:

- valid email format.

Implemented by:

- `ValidationRules.IsEmailValid`

### Role

Requirement:

- `Veterinarian` or `Secretary`.

Implemented by:

- `StaffRole`
- `RegisterEmployeeForm.CreateRoleComboBox`
- `ClinicDatabaseInitializer.SeedRoles`

### Customer Registration

Requirement:

- only secretary can register customers,
- full name letters only,
- Israeli ID exactly 9 digits,
- phone format valid,
- email format valid.

Implemented by:

- UI: `src/UI/CustomerForm.cs`
- Service: `src/Services/CustomerService.cs`
- Validator: `src/Validators/CustomerValidator.cs`
- Repository: `src/Repositories/CustomerRepository.cs`
- SQLite table: `Customers`
- Tests:
  - `UnitTests/CustomerValidatorTests.cs`
  - `BoundaryTests/CustomerBoundaryTests.cs`
  - `EquivalenceClassTests/CustomerEquivalenceClassTests.cs`
  - `AuthorizationTests/RoleAuthorizationTests.cs`
  - `FunctionalTests/CustomerServiceFunctionalTests.cs`
  - `DecisionTableTests/RegisterCustomerDecisionTableTests.cs`

### Customer Search

Requirement:

- search customer by Israeli ID or phone.

Implemented by:

- UI: `CustomerForm.SearchCustomer`
- Service: `CustomerService.SearchByIdentityOrPhone`
- Repository: `CustomerRepository.FindByIdentityOrPhone`
- Tests: `FunctionalTests/CustomerServiceFunctionalTests.cs`

### View Customer Animals

Requirement:

- view all animals linked to a customer.

Implemented by:

- UI: `CustomerForm.DisplayCustomer`
- Service: `CustomerService.GetCustomerAnimals`
- Repository: `AnimalRepository.FindByOwnerCustomerId`
- SQLite relationship: `Animals.OwnerCustomerId -> Customers.Id`

## 12. Known Limitations and Future Improvements

Known limitations:

- Israeli ID validation is intentionally assignment-level only: exactly 9 numeric digits.
- GUI automation tests are not yet implemented.
- SQLite repository integration tests can be expanded.
- Some WinForms classes are large, which makes direct unit testing of UI behavior harder.
- The application is desktop-focused and does not include mobile/web UI.
- Some veterinarian dashboard action cards are placeholders without wired service actions.
- Search customer input is checked for empty input in the UI; stricter service-level search format validation could be added later if required.

Possible future improvements:

- add GUI tests for login, registration, customer workflows, and role navigation,
- add integration tests using a temporary SQLite database,
- split large forms into smaller UI components where useful,
- add structured defect reports,
- add formal CFG diagrams and decision tables as separate documentation artifacts,
- improve observability with logging if the course or deployment requires it.

## 13. Testability and Software Testing Concepts

### Separation of Concerns

The project separates:

- UI rendering,
- business workflows,
- validation rules,
- persistence,
- domain models.

This allows most correctness rules to be tested without opening WinForms.

### Testability

Services depend on repository interfaces. Tests can replace SQLite repositories with fake repositories.

This supports:

- isolation,
- deterministic tests,
- faster test execution,
- service-level functional testing.

### Functional Testing

Functional tests check complete service workflows:

- login,
- registration,
- duplicate handling,
- search behavior.

### GUI Testing

The current architecture supports future GUI tests because:

- forms call services,
- validation messages are centralized,
- controls have meaningful field responsibilities,
- dashboards have clear role-based navigation.

GUI automation is listed as future work.

### Boundary Testing

Boundary tests cover critical field lengths and limits:

- username,
- password,
- employee ID,
- Israeli ID,
- phone.

### Equivalence Classes

Equivalence-class tests cover representative valid and invalid input groups. This avoids testing every possible string while still covering meaningful partitions.

### Regression Testing

The regression suite checks the main system flows after changes, helping detect accidental breakage.

### Validation and Verification

Validation is handled by validators and services. Verification is supported by automated tests that assert expected behavior and expected error messages.

### Maintainability

Centralized validators, service boundaries, repository interfaces, and organized tests make the code easier to modify and review.

### Observability

User-facing operations return `OperationResult<T>` or `AuthenticationResult`, which include:

- success/failure state,
- returned value,
- clear error message.

These result objects act as test oracles and make behavior easier to inspect.

## 14. Important File Map

| Purpose | File / Folder |
|---|---|
| Application startup | `src/Program.cs` |
| Database path | `src/Data/DatabaseSettings.cs` |
| Database schema and seed data | `src/Data/ClinicDatabaseInitializer.cs` |
| Login UI | `src/UI/LoginForm.cs` |
| Employee registration UI | `src/UI/RegisterEmployeeForm.cs` |
| Customer management UI | `src/UI/CustomerForm.cs` |
| Secretary dashboard | `src/UI/SecretaryDashboardForm.cs` |
| Veterinarian dashboard, broader module | `src/UI/VeterinarianDashboardForm.cs` |
| UI styling | `src/UI/UiTheme.cs` |
| Authentication service | `src/Services/AuthService.cs` |
| Employee service | `src/Services/EmployeeService.cs` |
| Customer service | `src/Services/CustomerService.cs` |
| Animal service, broader module | `src/Services/AnimalService.cs` |
| Visit service, broader module | `src/Services/VisitService.cs` |
| Login password comparison | `src/Services/AuthService.cs` |
| Validation messages | `src/Services/ValidationMessages.cs` |
| Shared service container | `src/Services/ClinicAppServices.cs` |
| Low-level validation rules | `src/Validators/ValidationRules.cs` |
| Employee validation | `src/Validators/EmployeeValidator.cs` |
| Customer validation | `src/Validators/CustomerValidator.cs` |
| Employee repository interface | `src/Repositories/IEmployeeRepository.cs` |
| Employee SQLite repository | `src/Repositories/SqliteEmployeeRepository.cs` |
| Customer repository interface | `src/Repositories/ICustomerRepository.cs` |
| Customer SQLite repository | `src/Repositories/CustomerRepository.cs` |
| Domain models | `src/Models` |
| Test project | `tests/ClinicVets.Tests` |
| Unit tests | `tests/ClinicVets.Tests/UnitTests` |
| Boundary tests | `tests/ClinicVets.Tests/BoundaryTests` |
| Equivalence-class tests | `tests/ClinicVets.Tests/EquivalenceClassTests` |
| Authorization tests | `tests/ClinicVets.Tests/AuthorizationTests` |
| Functional tests | `tests/ClinicVets.Tests/FunctionalTests` |
| Regression tests | `tests/ClinicVets.Tests/RegressionTests` |
| Decision-table tests | `tests/ClinicVets.Tests/DecisionTableTests` |
| Test doubles | `tests/ClinicVets.Tests/TestSupport` |

## 15. Build and Test Commands

Build the application:

```powershell
dotnet build
```

Run the application:

```powershell
dotnet run --project ClinicVets.csproj
```

Run tests:

```powershell
dotnet test tests\ClinicVets.Tests\ClinicVets.Tests.csproj
```

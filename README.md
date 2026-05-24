# ClinicVets

ClinicVets is a C# WinForms veterinary clinic management system developed for a Software Testing course project.

The implemented system uses:

- C# WinForms
- .NET `net10.0-windows`
- SQLite with `Microsoft.Data.Sqlite`
- xUnit automated tests

## My Implementation Scope

This assignment focuses on authentication, authorization, Secretary workflows, customer management, SQLite persistence, and automated testing.

The system supports two roles:

- `Secretary`
- `Veterinarian`

The main implemented flow for this assignment is the `Secretary` flow.

## Implemented User Stories

### Employee Registration

New employees can register with:

- username
- password
- employee number
- email
- Israeli identity number
- role selection: `Secretary` or `Veterinarian`

Passwords are saved in the SQLite database as plain strings for this course implementation.

Relevant files:

- `src/UI/RegisterEmployeeForm.cs`
- `src/Services/EmployeeService.cs`
- `src/Validators/EmployeeValidator.cs`
- `src/Repositories/EmployeeRepository.cs`

### Employee Login and Logout

Employees can login with username and password. After login, the application opens the correct dashboard according to the employee role.

Logout clears the current logged-in user session.

Relevant files:

- `src/UI/LoginForm.cs`
- `src/Services/AuthService.cs`
- `src/Services/AuthenticationResult.cs`

### Role-Based Authorization

Customer management is restricted to `Secretary` users.

`Veterinarian` users cannot register customers, search customers, or view customer animals through the customer management flow.

Relevant files:

- `src/Services/CustomerService.cs`
- `src/Models/StaffRole.cs`
- `src/Services/ValidationMessages.cs`

### Secretary Customer Management

Secretary users can:

- register customers
- search customers by Israeli ID or phone number
- view animals linked to a selected customer

Relevant files:

- `src/UI/CustomerForm.cs`
- `src/Services/CustomerService.cs`
- `src/Validators/CustomerValidator.cs`
- `src/Repositories/CustomerRepository.cs`
- `src/Repositories/AnimalRepository.cs`

## SQLite Persistence

SQLite is initialized when the application starts.

Relevant files:

- `src/Data/DatabaseSettings.cs`
- `src/Data/ClinicDatabaseInitializer.cs`
- `src/Program.cs`

The database file name is:

```text
clinicvets.db
```

The database is created in the application output folder because the connection string uses `AppContext.BaseDirectory`.

When running the published EXE, the database is created beside:

```text
ClinicVets.exe
```

If an existing demo database is needed, copy `clinicvets.db` into the same folder as the EXE.

## Project Structure

```text
src/
  Data/          SQLite schema creation and seed data.
  Models/        Domain models such as Employee, Customer, Animal, and roles.
  Repositories/  SQLite repository implementations and repository interfaces.
  Services/      Business logic, authentication, authorization, and workflows.
  Validators/    Validation rules for employee and customer input.
  UI/            WinForms screens.
  Program.cs     Application startup and dependency wiring.

tests/
  ClinicVets.Tests/  xUnit automated tests.
```

## Run the Project

```powershell
dotnet run --project ClinicVets.csproj
```

## Run Tests

```powershell
dotnet test tests\ClinicVets.Tests\ClinicVets.Tests.csproj
```

Current verified result:

```text
Passed: 182
Failed: 0
Skipped: 0
```

## Create Runnable Windows EXE

Use this command from the project root:

```powershell
dotnet publish ClinicVets.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:PublishDir=C:\clinucvets-new\publish\win-x64\
```

The EXE output folder is:

```text
C:\clinucvets-new\publish\win-x64\
```

Runnable file:

```text
C:\clinucvets-new\publish\win-x64\ClinicVets.exe
```

The `.pdb` file in the publish folder is a debug symbols file. The runnable application is the `.exe`.

## PDF Documentation

The implementation-based PDF content is prepared in:

```text
PDF_PART_SECRETARY_IMPLEMENTATION.md
```

It explains the real implemented flow, including:

- authentication
- authorization
- Secretary customer management
- validation logic
- SQLite interaction
- related tests
- EXE publish process

## Notes

Do not include `bin/`, `obj/`, or `.vs/` in Git.

The repository also contains modules for animals, visits, medicine, and dashboards. They exist in the codebase, but the main assignment scope documented here is the Secretary/customer/authentication flow.

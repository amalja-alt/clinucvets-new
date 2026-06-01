# ClinicVets

ClinicVets is a Windows Forms veterinary clinic management system written in C# for a Software Testing course project.

The project includes the application implementation, SQLite persistence, and automated xUnit tests.

## Technology

- C# WinForms
- .NET `net10.0-windows`
- SQLite using `Microsoft.Data.Sqlite`
- xUnit tests
- Windows target runtime: `win-x64`

## Project Structure

```text
ClinicVets.csproj
src/
  Data/          Database settings and SQLite initialization.
  Models/        Domain models: employees, customers, animals, medicines, visits, roles.
  Repositories/  SQLite data access classes and repository interfaces.
  Services/      Business logic, authentication, authorization, and workflows.
  Validators/    Input validation rules.
  UI/            WinForms screens.
  Program.cs     Application startup.

tests/
  ClinicVets.Tests/  Automated xUnit tests.
```

## Main Implemented Features

- Employee registration
- Employee login and logout
- Role-based access for `Secretary` and `Veterinarian`
- Secretary customer management
- Animal management
- Animal category management
- Medicine management
- Visit management
- SQLite database creation and persistence
- Automated tests for validation, services, repositories, authorization, and assignment flows

## Before Running

Use Windows with the .NET SDK installed.

The project targets:

```text
net10.0-windows
```

All commands below should be run from the project root folder:

```text
clinucvets-new-main
```

## Run the Application From Source

Use this command:

```powershell
dotnet run --project ClinicVets.csproj
```

This starts the WinForms application and opens the login screen.

On a new database, there are no employees yet. First register an employee from the application, then login with that employee.

## Build the Project

Use this command:

```powershell
dotnet build ClinicVets.csproj -c Release
```

The normal build output is created here:

```text
bin\Release\net10.0-windows\
```

The normal build EXE is:

```text
bin\Release\net10.0-windows\ClinicVets.exe
```

## Run Automated Tests

Use this command:

```powershell
dotnet test tests\ClinicVets.Tests\ClinicVets.Tests.csproj -c Release
```

Verified result:

```text
Passed: 182
Failed: 0
Skipped: 0
Total: 182
```

## Create a Fresh Runnable EXE

If the checker wants to create the EXE again from clean output folders, run:

```powershell
Remove-Item -Recurse -Force bin, obj
dotnet publish ClinicVets.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

The published EXE will be created here:

```text
bin\Release\net10.0-windows\win-x64\publish\ClinicVets.exe
```

This is the recommended EXE file to run or submit.

## Run the Published EXE

Open this file:

```text
bin\Release\net10.0-windows\win-x64\publish\ClinicVets.exe
```

You can run it by double-clicking the file in Windows Explorer.

You can also run it from PowerShell:

```powershell
.\bin\Release\net10.0-windows\win-x64\publish\ClinicVets.exe
```

Do not run files from the `obj` folder. The `obj` folder contains intermediate .NET build files, not the final application for checking or submission.

## Database Information

The application uses SQLite.

The database file name is:

```text
clinicvets.db
```

The database location depends on where the application is running from. The code uses `AppContext.BaseDirectory`, so the database is created beside the running application output.

When running with `dotnet run`, the database is created in the build output folder.

When running the published EXE, the database is created beside:

```text
bin\Release\net10.0-windows\win-x64\publish\ClinicVets.exe
```

So the published database path is:

```text
bin\Release\net10.0-windows\win-x64\publish\clinicvets.db
```

If the database file does not exist, the application creates it automatically on startup.

Important: each run output folder has its own `clinicvets.db`. If an employee is registered while running one output, then the checker opens a different EXE/output folder, that employee will not exist in the other database.

Common examples:

```text
bin\Debug\net10.0-windows\clinicvets.db
bin\Release\net10.0-windows\clinicvets.db
bin\Release\net10.0-windows\win-x64\clinicvets.db
bin\Release\net10.0-windows\win-x64\publish\clinicvets.db
```

If login says `Username or password is incorrect` after registering an employee, make sure you are running the same output folder where the employee was registered, or copy the correct `clinicvets.db` beside the EXE you want to run.

The initializer creates these tables:

- `Roles`
- `Employees`
- `Customers`
- `AnimalCategories`
- `Animals`
- `Medicines`
- `Visits`
- `VisitMedicines`

The initializer also seeds:

- roles: `Veterinarian`, `Secretary`
- animal categories: `Dog`, `Cat`, `Reptile`, `Bird`

Employee users are not seeded automatically. Register an employee in the application before logging in on a fresh database.

## Important Files for Checking

- `src/Program.cs` starts the application and wires services, repositories, validators, and the database initializer.
- `src/Data/DatabaseSettings.cs` defines the SQLite database name and location.
- `src/Data/ClinicDatabaseInitializer.cs` creates the database tables and seed data.
- `src/UI/LoginForm.cs` contains the login screen.
- `src/UI/RegisterEmployeeForm.cs` contains employee registration.
- `src/UI/SecretaryDashboardForm.cs` contains the secretary dashboard.
- `src/UI/VeterinarianDashboardForm.cs` contains the veterinarian dashboard.
- `tests/ClinicVets.Tests/` contains the automated test suite.

## Quick Checker Commands

From the project root:

```powershell
dotnet build ClinicVets.csproj -c Release
dotnet test tests\ClinicVets.Tests\ClinicVets.Tests.csproj -c Release
dotnet run --project ClinicVets.csproj
```

To recreate the published EXE:

```powershell
Remove-Item -Recurse -Force bin, obj
dotnet publish ClinicVets.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

Published EXE location:

```text
bin\Release\net10.0-windows\win-x64\publish\ClinicVets.exe
```

## Notes

- The `.pdb` file is only a debug symbols file. The runnable application is `ClinicVets.exe`.
- `bin` and `obj` are generated build folders.
- `clinicvets.db` is generated automatically when the application starts.
- No manual database setup is required.

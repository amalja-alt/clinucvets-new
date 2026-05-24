# ClinicVets

GUI-based veterinary clinic management system developed in C# WinForms for the Software Testing course project.

## My Assignment Scope

This course assignment focuses mainly on two workflows:

1. Employee login and employee registration.
2. Secretary-only customer management.

Employee scope includes SQLite-backed login, authentication flow, employee registration, username/password/employee ID/email/Israeli ID validation, and role selection for `Secretary` or `Veterinarian`. Passwords are saved in the database as entered for the current course implementation.

Customer scope includes registering customers, searching customers by Israeli ID or phone number, and displaying animals linked to a customer. Customer management is restricted to `Secretary` users. `Veterinarian` users must not register, search, or manage customer information.

## Other Existing Modules

The codebase also contains animal, medicine, visit, dashboard, and lookup modules. These support the broader application, but they are not the main implementation/testing responsibility for this assignment.

## Project Structure

```text
src/
  Data/          SQLite schema creation and seed data.
  Models/        Domain models. Assignment focus: Employee and Customer.
  Repositories/  Repository interfaces and implementations.
  Services/      Business logic, permissions, authentication, and workflows.
  Validators/    Testable validation classes split by module.
  UI/            WinForms GUI screens.
  Program.cs     Application startup and dependency wiring.
tests/
  ClinicVets.Tests/  xUnit unit and functional/service tests.
```

The system currently supports two roles only: `Secretary` and `Veterinarian`.

SQLite is used through the repository layer. The database file is created as `clinicvets.db` in the application output folder when the app starts.

For a full assignment progress summary, including what is implemented and what still needs to be added, see:

- [ASSIGNMENT_PROGRESS.md](ASSIGNMENT_PROGRESS.md)

Run the project:

```powershell
dotnet run --project ClinicVets.csproj
```

Run automated tests:

```powershell
dotnet test tests\ClinicVets.Tests\ClinicVets.Tests.csproj
```

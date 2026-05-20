# ClinicVets

GUI-based veterinary clinic management system developed in C# WinForms for the Software Testing course project.

## Project Structure

```text
src/
  Data/          SQLite schema creation and seed data.
  Models/        Domain models: Employee, Customer, Animal, Visit, Medicine.
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

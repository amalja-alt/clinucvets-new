# Tests

This folder contains the automated test project:

- `ClinicVets.Tests`

Current test groups:

- `UnitTests`: focused tests for `ValidationRules`, `EmployeeValidator`, and `CustomerValidator`.
- `BoundaryTests`: boundary value analysis for username, password, employee ID, Israeli ID, and phone.
- `EquivalenceClassTests`: valid and invalid input partitions.
- `AuthorizationTests`: role-based access tests.
- `FunctionalTests`: service-level workflow tests.
- `RegressionTests`: small core regression suite for main workflows.
- `DecisionTableTests`: decision-table rows for employee and customer registration.
- `TestSupport`: fake repositories and reusable test data builders.

Run:

```powershell
dotnet test tests\ClinicVets.Tests\ClinicVets.Tests.csproj
```

Future GUI tests should cover WinForms behavior such as login failures, successful login navigation, employee registration validation messages, and secretary-only customer management.

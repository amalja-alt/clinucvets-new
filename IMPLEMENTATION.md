# ClinicVets Implementation Summary

This file describes what has been implemented so far in the `ClinicVets` project.

<<<<<<< HEAD
## My Assignment Scope

The current student assignment part focuses mainly on:

1. Employee login and employee registration.
2. Customer management for animal owners, allowed only for secretary users.

Customer management is secretary-only. Veterinarian users must not register customers, search customers, or manage customer information.

## Other Existing Modules

The project also includes animal, medicine, visit, and dashboard modules. These are broader project features and are separate from the main assignment responsibility.

=======
## Current Assignment Part

The current student assignment part focuses on:

1. Login and employee registration for clinic staff.
2. Customer management for animal owners, allowed only for secretary users.

>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
## Implemented So Far

- C# WinForms application.
- Clean layered structure with `UI`, `Services`, `Validators`, `Repositories`, `Models`, and `Data`.
- Login GUI in `LoginForm`.
- Improved `LoginForm` design with a centered clinic-style card.
- Dashboard after login.
- Role-specific dashboards: `SecretaryDashboardForm` and `VeterinarianDashboardForm`.
- Employee validation and registration logic.
- Customer validation, service logic, search, and WinForms screen.
<<<<<<< HEAD
- Secretary-only permission rules for customer registration, search, and linked-animal display.
- SQLite database setup for clinic entities.
- Seeded roles: `Veterinarian` and `Secretary`.
- Employee passwords are stored as entered for the current course implementation and compared during login.
=======
- Secretary-only permission rule for customer registration.
- SQLite database setup for clinic entities.
- Seeded roles: `Veterinarian` and `Secretary`.
- Employee passwords are stored as entered for the current course implementation.
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
- Israeli ID format validation: exactly 9 numeric digits.
- xUnit test project with validation and service-level tests.

## Still Needed

- Add GUI automation tests.
- Add SQLite repository integration tests.
- Add formal course deliverables: user stories, decision tables, boundary tables, CFG diagrams, and defect reports.

## Demo Users

```text
Username: secret1
Password: Secret#1
Role: Secretary

Username: vetuser
Password: Vetuser#1
Role: Veterinarian
```

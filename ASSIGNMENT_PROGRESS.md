# ClinicVets Assignment Progress

This document summarizes the current implementation for the Software Testing course project.

<<<<<<< HEAD
<<<<<<< HEAD
## My Assignment Scope

The current student assignment part focuses mainly on:

1. Employee login and employee registration.
2. Secretary-only customer management for animal owners.
=======
## Assignment Scope
=======
## My Assignment Scope
>>>>>>> main

The current student assignment part focuses mainly on:

<<<<<<< HEAD
1. Login and employee registration for clinic staff.
2. Customer management for animal owners.
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
1. Employee login and employee registration.
2. Secretary-only customer management for animal owners.
>>>>>>> main

The system has two roles only:

- `Secretary`
- `Veterinarian`

## Login And Employee Registration

Required:

- Login with username and password from the database.
<<<<<<< HEAD
<<<<<<< HEAD
- Authenticate against SQLite employee data.
- Store registered employee passwords as entered for the current course implementation.
=======
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
- Authenticate against SQLite employee data.
- Store registered employee passwords as entered for the current course implementation.
>>>>>>> main
- Register a new employee.
- Username: 6-8 English letters/digits, at most 2 digits.
- Password: 8-10 characters, at least one letter, one digit, and one special character from `!`, `#`, `$`.
- Employee number: exactly 4 digits.
- Email: valid format.
- Israeli ID: exactly 9 numeric digits.
- Role: `Secretary` or `Veterinarian`.

Implemented:

- `LoginForm` GUI.
- `RegisterEmployeeForm` GUI.
- `AuthService.Login`.
- `EmployeeService.RegisterEmployee`.
- `EmployeeValidator`.
- `ValidationRules`.
<<<<<<< HEAD
<<<<<<< HEAD
- Login password comparison against the stored database value.
=======
- Employee password storage and login comparison are implemented without hashing for the current course implementation.
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
- Login password comparison against the stored database value.
>>>>>>> main
- SQLite employee persistence through `SqliteEmployeeRepository`.
- Role-specific dashboards after login.
- Demo users for secretary and veterinarian workflows.
- Unit and service tests for validation and employee workflows.

## Customer Management

Required:

- Only `Secretary` can access customer management actions.
<<<<<<< HEAD
<<<<<<< HEAD
- `Veterinarian` must not register, search, or manage customers.
=======
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
- `Veterinarian` must not register, search, or manage customers.
>>>>>>> main
- Register a new customer.
- Full name: letters only.
- Israeli ID: exactly 9 numeric digits.
- Phone: valid phone format.
- Email: valid format.
- Search customer by Israeli ID or phone.
- View all animals linked to a customer.

Implemented:

- `CustomerForm` GUI.
- `CustomerValidator`.
- `CustomerService.RegisterCustomer`.
- `CustomerService.SearchByIdentityOrPhone`.
- `CustomerService.GetCustomerAnimals`.
- `CustomerRepository` with SQLite persistence.
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> main
- Secretary-only service-level permission enforcement for registration, search, and linked-animal display.
- Functional and authorization tests for customer registration, search, and customer-management restrictions.

## Other Existing Modules

The project also contains broader clinic modules such as animals, animal categories, medicines, visits, and dashboards. They exist to support the full application, especially the requirement to display animals linked to a customer, but they are not the main assignment scope described above.
<<<<<<< HEAD
=======
- Secretary-only service-level permission enforcement.
- Functional tests for customer registration and search.
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
>>>>>>> main

## Architecture

The project uses a layered structure:

- `UI`: WinForms screens collect input, show output, and call services.
- `Services`: workflows, permissions, business decisions, and testable results.
- `Validators`: reusable validation rules for unit tests and boundary testing.
- `Repositories`: data access behind interfaces.
- `Models`: domain objects.
- `Data`: SQLite schema and seed data.

This supports:

- Unit Testing,
- Functional Testing,
- GUI Testing,
- Validation Testing,
- Regression Testing,
- Integration Testing.

## Testing Status

Automated test project:

```text
tests/ClinicVets.Tests
```

Implemented test areas:

- `ValidationRules`
- `EmployeeValidator`
- `CustomerValidator`
- `AuthService.Login`
- `EmployeeService.RegisterEmployee`
- `CustomerService.RegisterCustomer`
- `CustomerService.SearchByIdentityOrPhone`
<<<<<<< HEAD
<<<<<<< HEAD
- `CustomerService.GetCustomerAnimals`
- role authorization for secretary-only customer management
=======
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
- `CustomerService.GetCustomerAnimals`
- role authorization for secretary-only customer management
>>>>>>> main

Testing techniques covered by the first tests:

- valid and invalid equivalence classes,
- boundary values,
- validation test oracles,
- role-based functional tests,
- service isolation with fake repositories.

## Good Next Testing Work

- GUI tests for `LoginForm`, `RegisterEmployeeForm`, and `CustomerForm`.
- SQLite repository integration tests.
- Decision table for login.
- Decision table for employee registration.
- Decision table for customer registration.
- CFG for `AuthService.Login`.
- CFG for `CustomerService.RegisterCustomer`.
- Defect report table for validation and permission defects.

## Current Risk Summary

Highest risks:

- GUI behavior is not automated yet.
- SQLite repository integration behavior is not tested yet.
- Some forms are large and harder to unit test directly.
- Search input validation can be strengthened further at the service level.

Mitigation already started:

- validation rules are centralized,
- services depend on repository interfaces,
- xUnit tests now cover validation and first functional workflows,
- validation messages are separated for clearer test oracles.

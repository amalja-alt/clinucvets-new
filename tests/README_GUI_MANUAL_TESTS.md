# Manual GUI Tests For Assignment Scope

These tests are manual GUI tests because the current xUnit suite is focused on services, validators, repositories, and SQLite integration.

## GUI-01 Login Form

| Field | Value |
|---|---|
| Screen | `LoginForm` |
| Goal | Verify employee login through the actual UI |
| Preconditions | Application runs and a secretary employee exists in the database |
| Steps | 1. Run the application. 2. Enter username `secret1`. 3. Enter password `Secret#1`. 4. Click Login. |
| Expected Result | Login succeeds and the secretary dashboard opens. |

## GUI-02 Login Failure

| Field | Value |
|---|---|
| Screen | `LoginForm` |
| Goal | Verify invalid credentials are rejected in the UI |
| Preconditions | Application runs and user `secret1` exists |
| Steps | 1. Enter username `secret1`. 2. Enter password `Wrong#1`. 3. Click Login. |
| Expected Result | Login fails, an error message is shown, and the login screen remains active. |

## GUI-03 Employee Registration Form

| Field | Value |
|---|---|
| Screen | `RegisterEmployeeForm` |
| Goal | Verify employee registration validation and success flow |
| Preconditions | Application runs |
| Steps | 1. Open employee registration. 2. Enter valid employee details. 3. Submit. 4. Repeat with invalid username/password/email values. |
| Expected Result | Valid employee is saved; invalid input shows the matching validation message. |

## GUI-04 Customer Management Form

| Field | Value |
|---|---|
| Screen | `CustomerForm` |
| Goal | Verify secretary-only customer management through the UI |
| Preconditions | User is logged in as `Secretary` |
| Steps | 1. Open customer management. 2. Register a valid customer. 3. Search by identity number. 4. Search by phone number. |
| Expected Result | Customer is saved and can be found by identity number or phone. |

## GUI-05 Veterinarian Restriction

| Field | Value |
|---|---|
| Screen | `VeterinarianDashboardForm` / `CustomerForm` |
| Goal | Verify customer management is not available to veterinarian users |
| Preconditions | User is logged in as `Veterinarian` |
| Steps | 1. Log in as veterinarian. 2. Try to access customer management or perform customer registration/search if the screen is reachable. |
| Expected Result | Customer management is unavailable or the action is blocked. |

# Taskflow Developer Guide

## Architecture Overview
- `Program.cs`: dependency injection, identity, middleware, endpoint mapping.
- `Data/ApplicationDbContext.cs`: EF Core context with task-to-user relationship.
- `Models/TaskItem.cs`: task domain model + validation attributes.
- `Services/TaskService.cs`: task business/data access layer.
- `Components/Pages/*`: route pages (home, auth, tasks, settings, errors).
- `Components/Layout/*`: app shell and reconnect UX.

## Key Engineering Decisions
- Identity-based authorization ensures user-scoped task ownership.
- Service layer enforces user-id checks and logs exceptions for observability.
- UI forms use data annotations and validation components.
- Task operations provide user-facing feedback for success/failure states.
- Theme settings are persisted in local storage with graceful fallback.

## Error Handling Strategy
- Global error page for unhandled exceptions (`/Error`).
- Reconnect modal for transient Blazor circuit interruptions.
- Try/catch with logging in service methods.
- Page-level fallback messages for task load/save failures.
- Guard checks for invalid user context.

## Accessibility Strategy (WCAG-Oriented)
- Semantic layout regions and clear heading structure.
- Keyboard focus visibility on links, buttons, and inputs.
- Skip link for keyboard/screen-reader efficiency.
- ARIA labels and `aria-live` for dynamic status/error updates.
- Sufficient control size and contrast-aware theme variables.

## Local Development
```bash
cd TodoApp/TodoApp
dotnet restore
dotnet build
dotnet run
```

## Quality Checklist Before Submission
- Build passes with no warnings/errors.
- Register/login/logout flow validated.
- CRUD + filters validated with multiple tasks.
- Settings actions validated (profile/security/theme/export/delete).
- Error paths tested (invalid form submit, simulated task save/load failure).
- Documentation files updated for release.

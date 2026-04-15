# Taskflow - Blazor Task Management Platform

Taskflow is a production-style ASP.NET Core Blazor application focused on secure task management, fast workflows, accessible UI patterns, and resilient error handling.

## Highlights
- Secure authentication and authorization using ASP.NET Core Identity.
- Full task lifecycle (create, read, update, toggle completion, delete).
- Advanced filtering by category, priority, status, overdue, and view modes.
- Settings center for profile updates, password changes, theme preferences, and data export/delete controls.
- Responsive dashboard and WCAG-oriented interaction patterns (focus states, skip link, semantic landmarks, ARIA feedback messaging).
- Automatic EF Core database migrations at startup.

## Technology Stack
- .NET 10 / ASP.NET Core Blazor (Interactive Server)
- Entity Framework Core + SQLite
- ASP.NET Core Identity
- Bootstrap 5 + custom design system CSS

## Run Locally
1. Open a terminal in the repository root.
2. Navigate to the web project:
   ```bash
   cd TodoApp/TodoApp
   ```
3. Build and run:
   ```bash
   dotnet run
   ```
4. Open the URL shown in the terminal (typically `http://localhost:5xxx` or `https://localhost:7xxx`).

## Build Verification
```bash
cd TodoApp/TodoApp
dotnet build
```

## Database and Migrations
From `TodoApp/TodoApp`:
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Documentation Index
- User documentation: `docs/USER_GUIDE.md`
- Developer documentation: `docs/DEVELOPER_GUIDE.md`
- Project board snapshot and completion evidence: `docs/PROJECT_BOARD.md`

## CI/CD
GitHub Actions workflow is configured in `.github/workflows/main_cse325team01.yml` for build and Azure deployment.

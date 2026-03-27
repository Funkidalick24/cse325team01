# TaskFlow - Simple Task Manager

## Project Description
A simple Task Manager / To-Do List web application using C# .NET Core with Blazor. It uses pure Static Server-Side Rendering (SSR) for a clean, minimalist application.

### Features
* User Authentication System (ASP.NET Core Identity, SQLite)
* CRUD Operations for Tasks (Create, Read, Update, Delete)
* Task Categories & Priority Flags
* Mobile-responsive interface
* Database migrations and SQLite file-based DB

## Setup Instructions

### Prerequisites
* .NET 10 SDK

### How to Run the Application
1. Open a terminal and navigate to the project root directory where the `.sln` or `TodoApp.csproj` is located:
   ```bash
   cd TodoApp
   cd TodoApp
   ```
2. Build and run the project using the .NET CLI:
   ```bash
   dotnet run
   ```
   *This command will automatically build the application and start a local server.*
3. Open a web browser and navigate to the URL indicated in the terminal (typically `http://localhost:5249`).
4. Click "Register" to create a new account and begin creating tasks.

*Note: The database will be created automatically on the first run using Entity Framework Migrations.*

### Migration Commands
If you need to update or recreate the database manually, you can run the following EF Core CLI commands inside the `TodoApp/TodoApp` directory:

```bash
# To add a new migration:
dotnet ef migrations add <MigrationName>

# To update the database schema:
dotnet ef database update
```

### Package References
The application relies on the following NuGet packages:
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (Version 10.0.5)
- `Microsoft.AspNetCore.Identity.UI` (Version 10.0.5)
- `Microsoft.EntityFrameworkCore.Sqlite` (Version 10.0.5)
- `Microsoft.EntityFrameworkCore.Tools` (Version 10.0.5)

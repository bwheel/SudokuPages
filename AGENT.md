# AGENT.md — SudokuPages

## 1. Project Overview

**SudokuPages** is a server-rendered web application that displays random Sudoku puzzles and their solutions. Users can view puzzles, reveal solutions, print, and fetch new random puzzles from a SQLite database seeded with millions of puzzles.

**Key goals for any agent working in this repo:**
- Understand the puzzle display and navigation domain before making changes
- Preserve the puzzle grid rendering and solution display behavior
- The database is read-only at runtime — all puzzle data is pre-seeded via the `scripts/CreateInitialDb` utility

---

## 2. Architecture & Tech Stack

**SudokuPages** is a server-rendered monolithic web application.

### Stack
| Layer | Technology |
|---|---|
| Frontend | ASP.NET Core MVC / Razor Views |
| Client-side | jQuery, Bootstrap 5, Bootstrap Icons |
| Backend | ASP.NET Core (.NET 9) |
| ORM | Entity Framework Core 9 |
| Database | SQLite |
| Third-party Services | None |

### Architecture Overview
- **Server-rendered** — UI is rendered on the server via Razor Views. Avoid introducing client-side frameworks unless explicitly required.
- **MVC** — controllers delegate to services and pass ViewModels to Razor views.
- **Entity Framework Core** — all database access goes through EF Core. Do not write raw SQL unless there is a documented performance reason.
- **jQuery** — used sparingly for client-side interactivity. Do not introduce additional JS frameworks or libraries without discussion.

---

## 3. Folder Structure

```
/
├── SudokuPages.sln              # Visual Studio solution file
├── global.json                  # .NET SDK version pin (9.0.102)
├── src/
│   ├── SudokuPages/             # ASP.NET Core MVC web frontend
│   │   ├── Controllers/         # MVC controllers (Puzzle, Solution)
│   │   ├── Domain/              # Domain services (PuzzleService)
│   │   ├── Models/              # View models (PuzzleViewModel, ErrorViewModel)
│   │   ├── Views/               # Razor templates
│   │   ├── wwwroot/             # Static assets (restored via libman)
│   │   └── libman.json          # Client-side library manager config
│   └── SudokuPages.Data/        # EF Core data layer (class library)
│       ├── Migrations/          # EF Core migrations
│       ├── Models/              # Entity models (Puzzle)
│       └── Repos/               # Repository implementations (PuzzleRepo)
├── test/
│   ├── SudokuPages.Test/        # Domain logic tests (PuzzleService)
│   └── SudokuPages.Data.Test/   # Data layer tests (PuzzleRepo)
├── ci/
│   └── docker-compose.yaml      # CI environment setup
├── scripts/
│   └── CreateInitialDb/         # One-off CSV-to-SQLite seeding utility
├── data/                        # SQLite database files (not committed)
└── docs/                        # Documentation assets
```

### Conventions
- All source projects live under `src/` and are named `SudokuPages.<Layer>`.
- Test projects live under `test/` and mirror their source project with a `.Test` suffix.
- Do not place application code in `scripts/` — it is for one-off utility scripts only.
- Do not add CI/infrastructure config outside of `ci/`.

---

## 4. Build, Run & Dev Setup

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) 9.0.102+ (see `global.json`)

### Local Tool Manifest
This project uses a dotnet tool manifest to manage local CLI tools. After cloning, restore them first:
```bash
dotnet tool restore
```

This installs:
- `dotnet-ef` — Entity Framework Core CLI for managing migrations
- `libman` — Client-side library manager for restoring front-end libraries (jQuery, Bootstrap, etc.) into `wwwroot`. Runs automatically on build via the `.csproj` integration — no manual restore needed.

### First-Time Setup
```bash
# 1. Restore local dotnet tools
dotnet tool restore

# 2. Build the solution (also restores NuGet packages and client-side libraries via libman)
dotnet build

# 3. Copy and configure local app settings
cp src/SudokuPages/appsettings.json src/SudokuPages/appsettings.Development.json
# Edit appsettings.Development.json — set the ConnectionStrings:Default to point to your SQLite db:
# "Data Source=../../data/sudoku.db"

# 4. Seed the database (requires a Sudoku CSV dataset, e.g. from Kaggle)
cd scripts/CreateInitialDb
dotnet run -- <csv_file_path> <output_db_path>
```

### Running the App
```bash
dotnet run --project src/SudokuPages
```
Or open `SudokuPages.sln` in Visual Studio Code and use the built-in run/debug tools.

### Entity Framework Migrations
```bash
# Add a new migration
dotnet ef migrations add <MigrationName> --project src/SudokuPages.Data --startup-project src/SudokuPages

# Apply migrations
dotnet ef database update --project src/SudokuPages.Data --startup-project src/SudokuPages

# Revert last migration
dotnet ef migrations remove --project src/SudokuPages.Data --startup-project src/SudokuPages
```

### Running Tests
```bash
dotnet test
```

### Configuration
- `appsettings.json` — committed base configuration, no secrets
- `appsettings.Development.json` — local overrides including the DB connection string, **never commit this file**
- Ensure `appsettings.Development.json` is listed in `.gitignore`

---

## 5. Code Patterns & Conventions

### Architecture Pattern
- **Repositories** (`SudokuPages.Data/Repos/`) — responsible for all data access via Entity Framework Core. No business logic.
- **Domain Services** (`SudokuPages/Domain/`) — contain business logic (e.g. converting puzzle strings to 2D grids). Consume repositories, never DbContext directly.
- **Controllers** (`SudokuPages/Controllers/`) — thin. Delegate to services, pass ViewModels to views. No business logic here.

### Folder Structure (within each project)
```
SudokuPages/
├── Controllers/        # MVC controllers
├── Domain/             # Domain services
├── Models/             # ViewModels (one per view)
├── Views/              # Razor views
└── wwwroot/            # Static assets (restored via libman)

SudokuPages.Data/
├── Migrations/         # EF Core migrations
├── Models/             # Entity models (Puzzle)
└── Repos/              # Repository implementations
```

### Naming Conventions
| Type | Convention | Example |
|---|---|---|
| Service | `<Name>Service` | `PuzzleService` |
| Repository | `<Name>Repo` | `PuzzleRepo` |
| ViewModel | `<Name>ViewModel` | `PuzzleViewModel` |
| EF Model | `<Name>` (no suffix) | `Puzzle` |
| Controller | `<Name>Controller` | `PuzzleController` |

### Async/Await
- Repository methods that perform I/O must be `async` and return `Task` or `Task<T>`.
- Never use `.Result` or `.Wait()` — always `await`.

### Configuration
- Use `IOptions<T>` for strongly typed configuration.
- Never hardcode configuration values — all environment-specific values belong in `appsettings.Development.json`.

### ViewModels
- Every view has its own dedicated ViewModel class in `SudokuPages/Models/`.
- Do not pass EF entity models directly to views.

---

## 6. Testing Expectations

### Test Types
| Type | Scope | Location |
|---|---|---|
| Unit | Domain services in isolation | `test/SudokuPages.Test/` |
| Integration | Repository methods with in-memory SQLite | `test/SudokuPages.Data.Test/` |

### Frameworks & Libraries
- **Test framework:** xUnit
- **Database (integration):** In-memory SQLite connection via `DbContextFixture`

### What to Test
- **Domain services** — all business logic must have unit tests.
- **Repositories** — test against an in-memory SQLite database. Do not mock DbContext.

### Writing Tests
- Test class names mirror the class under test with a `Tests` suffix (e.g. `PuzzleServiceTests`).
- Test method names follow the pattern: `<MethodName>_<Scenario>_<ExpectedResult>`
  - e.g. `ConvertPuzzleTo2DGrid_WhenInputIsNull_ThrowsArgumentException`
- Each test should follow the **Arrange / Act / Assert** pattern.
- One assertion concept per test.

### Running Tests
```bash
# Run all tests
dotnet test

# Run tests for a specific project
dotnet test test/SudokuPages.Test
dotnet test test/SudokuPages.Data.Test

# Run tests with coverage report
dotnet test --collect:"XPlat Code Coverage"
```

---

## 7. Feature Implementation Workflow

When implementing a feature, follow these steps in order:

### Step 1 — Read Before Writing
- Read all existing code related to the feature area before writing anything.
- Identify the relevant models, repos, services, controllers, and tests.

### Step 2 — Confirm the Approach
- Before writing any code, summarize your intended approach and confirm it is correct.
- If the feature requires a schema change, confirm before writing migrations.

### Step 3 — Implement the Feature
Follow this order:

1. **Model** — add or update EF Core entity models in `SudokuPages.Data/Models/`
2. **Repository** — add or update the repository in `SudokuPages.Data/Repos/`
3. **Domain Service** — add or update the service in `SudokuPages/Domain/`
4. **ViewModel** — create or update the ViewModel in `SudokuPages/Models/`
5. **Controller** — add or update the controller in `SudokuPages/Controllers/`
6. **View** — add or update the Razor view in `SudokuPages/Views/`
7. **Migration** — if the data model changed, generate an EF Core migration:
   ```bash
   dotnet ef migrations add <MigrationName> --project src/SudokuPages.Data --startup-project src/SudokuPages
   ```

### Step 4 — Write Tests
- Unit test all new or modified domain service methods.
- Integration test repository behavior using in-memory SQLite.
- Ensure all existing tests still pass:
  ```bash
  dotnet test
  ```

### Step 5 — Verify
Before marking a feature complete:
- [ ] `dotnet build` completes with no errors or warnings
- [ ] `dotnet test` passes with no failures
- [ ] No business logic exists in controllers or views
- [ ] No EF entity models are passed directly to views
- [ ] No secrets or environment-specific values are hardcoded
- [ ] `appsettings.Development.json` is not committed

---

## 8. What NOT to Do

### Architecture
- **Do not put business logic in controllers or views.** All business logic belongs in domain services.
- **Do not use DbContext directly in controllers or services.** All data access goes through repositories.
- **Do not pass EF Core entity models directly to views.** Always map to a ViewModel first.

### Code Quality
- **Do not use `.Result` or `.Wait()` on async methods.** Always use `await`.
- **Do not hardcode configuration values.** All environment-specific values belong in `appsettings.Development.json`.

### Testing
- **Do not write tests purely to hit a coverage number.** Tests should assert meaningful behavior and edge cases.
- **Do not mock DbContext directly.** Use the in-memory SQLite provider for repository tests.

### General
- **Do not introduce new libraries or frameworks** without explicit discussion and approval.
- **Do not commit `appsettings.Development.json`** or any file containing secrets or local configuration.
- **Do not commit the `data/` directory** — database files are generated locally and excluded from source control.
- **Do not create files or new abstractions speculatively.** Only add what is needed for the current feature.
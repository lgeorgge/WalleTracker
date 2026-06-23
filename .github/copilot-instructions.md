# Copilot Instructions

## Build and run

- Build the API: `dotnet build src/WalletTracker.API/WalletTracker.API.csproj`
- Run the API: `dotnet run --project src/WalletTracker.API/WalletTracker.API.csproj`
- Watch mode: `dotnet watch run --project src/WalletTracker.API/WalletTracker.API.csproj`
- VS Code task `build` targets the API project and is used by the launch config.

## Architecture

- This is a layered .NET 10 solution with four main projects:
  - `WalletTracker.Domain`: entities, enums, and domain exceptions.
  - `WalletTracker.Application`: request/response models, validators, specifications, and service interfaces.
  - `WalletTracker.Infrastructure`: EF Core, repository/unit-of-work implementations, auth services, and migrations.
  - `WalletTracker.API`: controllers, DI setup, JWT auth, Serilog, OpenAPI/Scalar hosting.
- `WalletTracker.API` depends on `Application` and `Infrastructure`; `Infrastructure` implements interfaces defined in `Application`.
- Reads use the specification pattern (`ISpecification`, `BaseSpecification`, `SpecEvaluator`) through a generic repository.
- Persistence is EF Core + Npgsql with `WalletDBContext` applying all entity configurations from the infrastructure assembly.
- Auth is JWT-based. `CurrentUser` reads claims from `HttpContext`, and `JwtTokenGenerator` emits `sub`, email, and role claims.

## Conventions

- Domain entities own their invariants. Constructors validate inputs and throw `DomainException`; mutable state is changed through methods like `SetPasswordHash`, `Deposit`, `Withdraw`, and `SetUpdatedAtUTC`.
- Strings are normalized in the domain where needed (`User.Email` is trimmed and lowercased).
- Keep controllers thin: they orchestrate repositories/services and shape HTTP responses rather than containing business rules.
- Use validators from `WalletTracker.Application.Features.*` with FluentValidation; `Program.cs` auto-registers validators from the auth assembly.
- Follow the existing pagination/query pattern: `UserQueryParameters` caps page size at 100, `UsersSpecification` applies pagination and optional search, and `PagedResponse<T>` carries the result envelope.
- EF mappings live in `WalletTracker.Infrastructure.Data.Configurations` and define table names, keys, lengths, precision, and indexes.
- Existing API routes are under `api/auth` and `api/users`; keep new endpoints consistent with that naming style.

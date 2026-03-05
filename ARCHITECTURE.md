# PropertyInventory – Architecture Overview

## Solution Structure (Onion / Layered)

| Project | Purpose |
|--------|--------|
| **PropertyInventory.API** | Web API – controllers, middleware pipeline, DI registration |
| **PropertyInventory.Models** | DTOs, entities, view models, request/response types (`ServerResponse<T>`, `PagedResult<T>`, etc.) |
| **PropertyInventory.Interfaces** | Contracts – repository and service interfaces |
| **PropertyInventory.Repository** | Data access – generic repository, `PropertyRepository`, `ContactRepository`, uses Data `DbContext` |
| **PropertyInventory.Services** | Business logic – `PropertyService`, `ContactService` (orchestrate repositories, validation) |
| **PropertyInventory.Utils** | Cross-cutting – global exception middleware, extensions |
| **PropertyInventory.Data** | Persistence – `PropertyInventoryDbContext`, migrations |

## Dependency Flow

```
API → Services, Utils, Models, Interfaces, Repository
Services → Interfaces, Models
Repository → Data, Interfaces, Models
Utils → Models
Interfaces → Models
Data → Models
Models → (no project refs)
```

## Key Types

- **PropertyInventoryDbContext** – EF Core context in PropertyInventory.Data.
- **PropertyInventoryDbContextModelSnapshot** – migrations snapshot in PropertyInventory.Data.Migrations.

## Running the API

From the solution directory:

```bash
dotnet build PropertyInventory.API\PropertyInventory.sln
dotnet run --project PropertyInventory.API
```

Ensure `appsettings.json` has a valid `ConnectionStrings:DefaultConnection` for SQL Server. Migrations run on startup.

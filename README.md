# Property Inventory System — Backend API

A .NET 8 REST API for managing Properties, Contacts, ownership history and price history.

## Project Structure

```
PropertyInventory.API Web API – Controllers, middleware pipeline, DI registration
PropertyInventory.Models – DTOs, entities, view models, request/response types (`ServerResponse<T>`, `PagedResult<T>`, etc.)
PropertyInventory.Interfaces – Repository and service interfaces
PropertyInventory.Repository – `PropertyRepository`, `ContactRepository`, uses Data `DbContext`
PropertyInventory.Services – `PropertyService`, `ContactService`
PropertyInventory.Utils – Global exception middleware, extensions
PropertyInventory.Data – `PropertyInventoryDbContext`, migrations
```

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server or LocalDB (LocalDB is configured by default)

### 1. Update Connection String
Edit `PropertyInventory.API/appsettings.json` if your SQL Server instance differs:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=PropertyInventoryDB;Integrated Security=True"
}
```

### 2. Run the API
```bash
cd PropertyInventory.API
dotnet run
```
Migrations are applied automatically on startup. Seed data (3 contacts, 2 properties, ownership history) is applied via `OnModelCreating`.

### 3. Swagger UI
Navigate to `https://localhost:7035/swagger` to explore and test all endpoints.

## Seed Data
The database is pre-seeded with the example data from the ISB Technologies spec:
- **Maisonette** — previously owned by Joshua Mifsud, currently owned by Carmen Attard
- **Penthouse** — currently owned by Joe Borg

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Contact` | List all contacts (paginated, filterable) |
| GET | `/api/Contact/{id}` | Get contact by ID |
| POST | `/api/Contact` | Create new contact |
| PUT | `/api/Contact/{id}` | Update contact |
| GET | `/api/Property` | List all properties (paginated, filterable) |
| GET | `/api/Property/{id}` | Get property by ID |
| POST | `/api/Property` | Create new property |
| PUT | `/api/Property/{id}` | Update property |
| POST | `/api/Property/{id}/transfer` | Transfer ownership |
| POST | `/api/Property/{id}/update-price` | Update asking price |
| GET | `/api/Dashboard/property-sales` | Property sales dashboard |

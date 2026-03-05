# Property Inventory – Database (manual setup)

No automatic migrations or seeding on app start. Create/update the database and data by running EF Core migrations manually.

---

## 1. Connection string

Set in **PropertyInventory.API** (appsettings.json or appsettings.Development.json):

```text
Server=SQLEXPRESS;Database=PropertyInventoryDB;User Id=sa;Password=1234;TrustServerCertificate=True;MultipleActiveResultSets=true
```

---

## 2. Run migrations manually

**Prerequisites**

- SQL Server running at `SQLEXPRESS` (or your configured server).
- Database `PropertyInventoryDB` created (optional; EF can create it if the user has permission).
- .NET EF Core tools: `dotnet tool install -g dotnet-ef` (once per machine).

**Apply migrations (creates/updates tables and seed data)**

From the **API** project folder:

```powershell
cd PropertyInventory.API
dotnet ef database update --project "..\PropertyInventory.Data\PropertyInventory.Data.csproj"
```

- **Startup project** = API (current directory), so the connection string from appsettings is used.
- **Context project** = PropertyInventory.Data.

The migration **20260226133553_InitialCreate** creates the four tables and inserts the initial seed data (Contacts, Properties, PropertyOwnerships, PropertyPriceHistories).

**Add a new migration** (after changing entities/DbContext):

```powershell
cd PropertyInventory.API
dotnet ef migrations add YourMigrationName --project "..\PropertyInventory.Data\PropertyInventory.Data.csproj"
```

Then apply it:

```powershell
dotnet ef database update --project "..\PropertyInventory.Data\PropertyInventory.Data.csproj"
```

---

## 3. Tables and seed data

| Table                 | Purpose |
|-----------------------|--------|
| **Contacts**          | People (owners/contacts). |
| **Properties**        | Properties (name, address, current price, etc.). |
| **PropertyOwnerships**| Links Contact ↔ Property with EffectiveFrom/EffectiveTill and acquisition price. |
| **PropertyPriceHistories** | Price history per property. |

Initial data is embedded in the **InitialCreate** migration (InsertData): 3 contacts (Carmen, Joshua, Joe), 2 properties (Maisonette, Penthouse), ownerships and price history rows.

---

## 4. Summary

1. Set connection string in API appsettings.
2. Create database (or let EF create it): ensure SQL Server is running.
3. Run: `dotnet ef database update --project "..\PropertyInventory.Data\PropertyInventory.Data.csproj"` from the **PropertyInventory.API** folder.
4. Start the API; no DB logic runs on startup.

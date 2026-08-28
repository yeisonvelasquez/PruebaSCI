# PruebaSCI API

Web API in .NET 10 for product management through SQL Server stored procedures and weather forecast queries through Open-Meteo.

## Run the database

Execute [database/PruebaSCI.sql](database/PruebaSCI.sql) in SQL Server Management Studio or Azure Data Studio. The script creates the `PruebaSCI` database, the `Products` table and all CRUD stored procedures.

## Run the API

```powershell
dotnet restore
dotnet build PruebaSCI.slnx
dotnet run --project PruebaSCI.Api
```

Swagger UI is available at `/swagger`.

## Tests

```powershell
dotnet test PruebaSCI.Tests/PruebaSCI.Tests.csproj
```

The tests cover application service behavior, the external weather response mapping and the global exception middleware.

## Endpoints

- `GET /api/products`
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`
- `GET /api/weather?latitude=52.52&longitude=13.41`

The SQL Server connection and Open-Meteo settings are in `PruebaSCI.Api/appsettings.json`.

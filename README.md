# PruebaSCI API

## Descripción

PruebaSCI es una Web API desarrollada con .NET 10 para administrar productos y consultar un pronóstico meteorológico horario.

La aplicación está organizada por responsabilidades:

- `PruebaSCI.Domain`: entidades del dominio.
- `PruebaSCI.Application`: contratos, DTOs, validaciones y lógica de negocio.
- `PruebaSCI.Infrastructure`: acceso a SQL Server mediante Dapper y consumo de Open-Meteo.
- `PruebaSCI.Api`: controladores HTTP, Swagger, CORS y middleware global de errores.
- `PruebaSCI.Tests`: pruebas unitarias de servicios, integración externa simulada y manejo global de excepciones.

Los productos se almacenan en SQL Server exclusivamente mediante procedimientos almacenados. La base de datos, la tabla `Products` y los procedimientos del CRUD están definidos en [database/PruebaSCI.sql](database/PruebaSCI.sql).

## Requisitos

- .NET SDK 10.
- SQL Server local.
- SQL Server Management Studio, Azure Data Studio o Visual Studio con acceso a SQL Server.
- Visual Studio 2022 con la carga de trabajo de ASP.NET y desarrollo web.

## Configuración

La conexión de SQL Server y la configuración de Open-Meteo están en [PruebaSCI.Api/appsettings.json](PruebaSCI.Api/appsettings.json):

```json
{
	"ConnectionStrings": {
		"DefaultConnection": "Server=localhost;Database=PruebaSCI;Trusted_Connection=True;TrustServerCertificate=True;"
	},
	"OpenMeteo": {
		"BaseUrl": "https://api.open-meteo.com/",
		"ForecastPath": "v1/forecast"
	}
}
```

La conexión utiliza autenticación integrada de Windows. Si tu instancia local utiliza una configuración diferente, actualiza `DefaultConnection` antes de ejecutar la API.

## Crear la base de datos

### Desde SQL Server Management Studio o Azure Data Studio

1. Abre SQL Server Management Studio o Azure Data Studio.
2. Conéctate al servidor local `localhost` usando autenticación de Windows.
3. Abre [database/PruebaSCI.sql](database/PruebaSCI.sql).
4. Ejecuta el script completo.
5. Comprueba que existan la base de datos `PruebaSCI`, la tabla `dbo.Products` y sus procedimientos almacenados.

El script es idempotente para la base de datos y la tabla. Los procedimientos se actualizan mediante `CREATE OR ALTER PROCEDURE`.

### Desde Visual Studio

1. Abre **Ver > Explorador de objetos de SQL Server**.
2. Conéctate a `localhost` mediante autenticación de Windows.
3. Abre [database/PruebaSCI.sql](database/PruebaSCI.sql) desde **Archivo > Abrir > Archivo**.
4. Selecciona la conexión a `localhost` en el editor SQL.
5. Ejecuta el script con **Ejecutar** o `Ctrl+Shift+E`.
6. Actualiza el explorador y verifica la base de datos `PruebaSCI`.

## Ejecutar la API desde PowerShell

```powershell
cd C:\Proyectos\PruebaSCI
dotnet restore
dotnet build PruebaSCI.slnx
dotnet run --project .\PruebaSCI.Api\PruebaSCI.Api.csproj
```

La API estará disponible en `http://localhost:5013`.

### URL completa de Swagger

```text
http://localhost:5013/swagger/index.html
```

Swagger debe abrirse mediante `http://` o `https://`, nunca mediante una URL `file://`.

## Ejecutar la API desde Visual Studio

1. Abre Visual Studio.
2. Selecciona **Abrir un proyecto o una solución**.
3. Abre [PruebaSCI.slnx](PruebaSCI.slnx).
4. Establece `PruebaSCI.Api` como proyecto de inicio.
5. Selecciona el perfil `http`.
6. Presiona `F5` o `Ctrl+F5`.
7. Abre `http://localhost:5013/swagger/index.html`.

## Ejecutar las pruebas desde PowerShell

```powershell
cd C:\Proyectos\PruebaSCI
dotnet test .\PruebaSCI.Tests\PruebaSCI.Tests.csproj
```

También puedes ejecutar todas las pruebas con `dotnet test .\PruebaSCI.slnx`.

## Ejecutar las pruebas desde Visual Studio

1. Abre [PruebaSCI.slnx](PruebaSCI.slnx).
2. Selecciona **Prueba > Explorador de pruebas**.
3. Espera a que se descubran las pruebas.
4. Selecciona **Ejecutar todas las pruebas**.

Las pruebas cubren la lógica de productos, el mapeo de Open-Meteo y el middleware global de excepciones.

## Endpoints

### Productos

| Método | Ruta | Descripción |
| --- | --- | --- |
| `GET` | `/api/products` | Obtiene todos los productos. |
| `GET` | `/api/products/{id}` | Obtiene un producto por su identificador. |
| `POST` | `/api/products` | Crea un producto y devuelve `201 Created`. |
| `PUT` | `/api/products/{id}` | Actualiza un producto y devuelve `204 No Content`. |
| `DELETE` | `/api/products/{id}` | Elimina un producto y devuelve `204 No Content`. |

Ejemplo para `POST /api/products`:

```json
{
	"name": "Producto de prueba",
	"description": "Inserción de producto de prueba",
	"price": 7.92
}
```

El precio debe estar entre `0.01` y `9999999999999999.99`, según `decimal(18,2)`. `Id` es autogenerado y `CreatedDate` se establece en la base de datos.

### Clima

| Método | Ruta | Descripción |
| --- | --- | --- |
| `GET` | `/api/weather?latitude=52.52&longitude=13.41` | Consulta las temperaturas horarias mediante Open-Meteo. |

La latitud debe estar entre `-90` y `90`, y la longitud entre `-180` y `180`.

## Respuestas y errores

- `200 OK`: consulta exitosa.
- `201 Created`: producto creado correctamente.
- `204 No Content`: producto actualizado o eliminado correctamente.
- `400 Bad Request`: datos o coordenadas inválidas.
- `404 Not Found`: producto inexistente.
- `502 Bad Gateway`: el servicio meteorológico no está disponible.
- `500 Internal Server Error`: error inesperado gestionado por el middleware global.

Los errores inesperados se registran mediante logging y no exponen detalles internos al cliente.

La conexión de SQL Server y la configuración de Open-Meteo están en [PruebaSCI.Api/appsettings.json](PruebaSCI.Api/appsettings.json).

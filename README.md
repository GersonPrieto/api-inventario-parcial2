# API de Inventario — Segundo parcial

API REST para administrar categorías y productos, desarrollada en Visual Basic .NET con ASP.NET Core, Entity Framework Core, SQL Server y Swagger.

## Requisitos

- SDK de .NET 10.
- SQL Server en ejecución.
- Una cuenta de Windows con permisos para crear o utilizar la base de datos.
- Conexión a Internet para restaurar los paquetes NuGet.

El proyecto utiliza Entity Framework Core 10.0.15 y Swashbuckle.AspNetCore 10.3.1. Las dependencias están declaradas en `ApiInventario/ApiInventario.vbproj`.

## Configuración

La conexión se encuentra en `ApiInventario/appsettings.json`:

```json
"ConnectionStrings": {
  "InventarioDB": "Server=localhost;Database=InventarioDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
}
```

La configuración predeterminada utiliza SQL Server local, autenticación de Windows y la base `InventarioDB`. Si se utiliza otra instancia, se debe cambiar el valor de `Server`. `TrustServerCertificate=True` permite utilizar el certificado del servidor de desarrollo local.

## Preparación y ejecución

Desde una terminal ubicada en la carpeta de `ApiInventario.sln`, ejecutar los siguientes comandos en orden:

```powershell
dotnet restore ApiInventario.sln
dotnet build ApiInventario.sln --no-restore
dotnet run --project ApiInventario/ApiInventario.vbproj --no-build -- --verificar-modelo
dotnet run --project ApiInventario/ApiInventario.vbproj --no-build -- --migrar
dotnet run --project ApiInventario/ApiInventario.vbproj
```

`--verificar-modelo` comprueba la coincidencia entre el modelo y el snapshot de EF Core. `--migrar` crea la base si no existe y aplica las migraciones pendientes. Estas opciones se ejecutan localmente y no son endpoints HTTP.

En Windows también se pueden utilizar `01_Preparar.cmd` y `02_Iniciar.cmd`, respectivamente. La solución puede abrirse con una versión de Visual Studio compatible con .NET 10.

- Swagger: http://localhost:5082/swagger
- Documento OpenAPI: http://localhost:5082/swagger/v1/swagger.json
- Inicio de la API: http://localhost:5082/

La terminal debe permanecer abierta mientras se utiliza la API. Para detenerla, presionar `Ctrl+C`.

## Base de datos y migraciones

La base contiene las tablas `Categorias` y `Productos`. La relación es uno a muchos: una categoría puede tener varios productos y cada producto debe pertenecer a una categoría existente.

Las migraciones y el snapshot están en `ApiInventario/Migrations/`. Como alternativa al comando `--migrar`, se puede ejecutar `sql/InventarioDB.sql` en SQL Server Management Studio. El script crea la base, las tablas, los índices, las restricciones y el historial de migraciones. Comprueba ese historial para no volver a aplicar una migración registrada.

Se debe comenzar con una base vacía o compatible con la migración incluida. Si existen tablas creadas manualmente sin historial de EF, es necesario revisar el esquema antes de aplicar la migración. Si se cambia el nombre de la base, también debe adaptarse el encabezado del script SQL.

## Arquitectura

| Carpeta | Responsabilidad |
| --- | --- |
| `Controllers` | Recibir solicitudes HTTP, llamar a los servicios y devolver respuestas. |
| `Services` | Implementar reglas de negocio, validar entradas y construir DTOs de respuesta. |
| `Repositories` | Consultar y guardar información mediante EF Core. |
| `Models` | Definir las entidades `Categoria` y `Producto`. |
| `DTOs` | Separar los contratos de creación, modificación y respuesta. |
| `Data` | Configurar el contexto, las relaciones y las restricciones de la base. |
| `Middleware` | Centralizar el manejo de excepciones. |
| `Exceptions` | Definir las excepciones controladas de negocio. |
| `Migrations` | Mantener las operaciones necesarias para reconstruir el esquema. |

Las dependencias se registran en `Program.vb`. Los controladores no acceden directamente al `DbContext`. Los repositorios utilizan `Include` para recuperar las relaciones entre productos y categorías.

## Endpoints

| Método | Ruta | Operación | Código exitoso |
| --- | --- | --- | ---: |
| GET | `/api/categorias` | Consultar categorías. | 200 |
| GET | `/api/categorias/{id}` | Consultar una categoría con sus productos. | 200 |
| POST | `/api/categorias` | Registrar una categoría. | 201 |
| PUT | `/api/categorias/{id}` | Modificar una categoría. | 200 |
| DELETE | `/api/categorias/{id}` | Eliminar una categoría sin productos. | 204 |
| GET | `/api/productos` | Consultar productos con su categoría. | 200 |
| GET | `/api/productos/{id}` | Consultar un producto. | 200 |
| GET | `/api/productos/categoria/{categoriaId}` | Consultar productos de una categoría. | 200 |
| POST | `/api/productos` | Registrar un producto. | 201 |
| PUT | `/api/productos/{id}` | Modificar un producto. | 200 |
| DELETE | `/api/productos/{id}` | Eliminar un producto. | 204 |

## Validaciones y reglas de negocio

- Los nombres son obligatorios y admiten hasta 50 caracteres.
- Las descripciones son opcionales y admiten hasta 200 caracteres.
- El precio es obligatorio, mayor que cero y admite hasta dos decimales, de acuerdo con su almacenamiento en `decimal(18,2)`. Se rechazan valores que requieran redondeo o superen el rango de esa columna.
- El stock es obligatorio, entero y no puede ser negativo.
- La categoría del producto es obligatoria y debe existir.
- No se permiten nombres duplicados de productos ni categorías. La comparación ignora mayúsculas y minúsculas y también se aplica al modificar, excluyendo el propio registro.
- Los índices únicos refuerzan la integridad de los nombres en SQL Server.
- No se permite eliminar una categoría que tenga productos. La relación no utiliza eliminación en cascada.
- La fecha de registro se asigna en UTC al crear el producto y se conserva al modificarlo.
- Los DTOs de respuesta incluyen las relaciones necesarias sin generar referencias JSON circulares.

## Respuestas de error

| Código | Situación |
| --- | --- |
| 400 | Datos inválidos, campos obligatorios ausentes o identificadores no positivos. |
| 404 | Producto o categoría inexistente. También se utiliza cuando la categoría indicada al registrar o modificar un producto no existe. |
| 409 | Nombre duplicado o intento de eliminar una categoría con productos asociados. |
| 500 | Error inesperado. |

El middleware transforma las excepciones en respuestas con `codigo`, `mensaje`, `traceId` y, cuando corresponde, `errores`. Los detalles internos de las excepciones inesperadas se registran en el servidor y no se devuelven al consumidor.

## Pruebas con Swagger

Los endpoints se documentan y ejecutan desde Swagger mediante **Try it out** y **Execute**. Las evidencias de las pruebas se incluyen en la carpeta `evidencias/`.

Las pruebas del trabajo práctico comprenden el registro de categoría y producto, las consultas relacionadas, el rechazo de productos duplicados y categorías inexistentes, la restricción de eliminación de categorías ocupadas, la modificación y la eliminación de productos. Cada captura debe mostrar la solicitud y el código de la sección **Server response**, junto con el cuerpo de respuesta cuando corresponda.

# ArquitecturaLimpia

The project was generated using the [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/ArquitecturaLimpia) version 8.0.6.

## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:

```bash
cd .\src\Web\
dotnet watch run
```

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Code Scaffolding

The template includes support to scaffold new commands and queries.

Start in the `.\src\Application\` folder.

Create a new command:

```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:

```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

If you encounter the error *"No templates or subcommands found matching: 'ca-usecase'."*, install the template and try again:

```bash
dotnet new install Clean.Architecture.Solution.Template::8.0.6
```

## Test

The solution contains unit, integration, functional, and acceptance tests.

To run the unit, integration, and functional tests (excluding acceptance tests):
```bash
dotnet test --filter "FullyQualifiedName!~AcceptanceTests"
```

To run the acceptance tests, first start the application:

```bash
cd .\src\Web\
dotnet run
```

Then, in a new console, run the tests:
```bash
cd .\src\Web\
dotnet test
```

## Help
To learn more about the template go to the [project website](https://github.com/jasontaylordev/CleanArchitecture). Here you can find additional guidance, request new features, report a bug, and discuss the template with other users.

# Migración de SQL Server a MySQL en Clean Architecture

Esta guía documenta los pasos necesarios para migrar la plantilla Clean Architecture de Jason Taylor de SQL Server a MySQL.

## Prerrequisitos

- Plantilla Clean Architecture de Jason Taylor configurada
- MySQL Server instalado y funcionando
- Base de datos MySQL creada

## Paso 1: Instalar el Proveedor de MySQL

Instalar el paquete NuGet de Pomelo para MySQL en el proyecto Infrastructure:

```bash
dotnet add src/Infrastructure/Infrastructure.csproj package Pomelo.EntityFrameworkCore.MySQL
```

## Paso 2: Actualizar la Configuración de Dependencias

En el archivo `src/Infrastructure/DependencyInjection.cs`:

### 2.1 Agregar el Using
```csharp
using Pomelo.EntityFrameworkCore.MySQL;
```

### 2.2 Cambiar el Proveedor de Base de Datos
Reemplazar esta línea:
```csharp
options.UseSqlServer(connectionString);
```

Por:
```csharp
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
```

## Paso 3: Actualizar Connection String

En `src/Web/appsettings.json` y `src/Web/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=cleanarchitecture_db;user=root;password=tu_password;"
  }
}
```

## Paso 4: Instalar Herramientas de EF Core en el Proyecto Web

```bash
dotnet add src/Web/Web.csproj package Microsoft.EntityFrameworkCore.Design
```

## Paso 5: Limpiar Migraciones Existentes

Las migraciones de SQL Server no son compatibles con MySQL, por lo que deben eliminarse:

```bash
# Eliminar carpeta de migraciones
Remove-Item -Recurse -Force src/Infrastructure/Migrations
```

## Paso 6: Deshabilitar Temporalmente NSwag (Si es necesario)

Si obtienes errores durante el build por NSwag intentando acceder a la base de datos, comenta temporalmente el target en `src/Web/Web.csproj`:

```xml
<!--
  <Target Name="NSwag" AfterTargets="PostBuildEvent" Condition=" '$(Configuration)' == 'Debug' And '$(SkipNSwag)' != 'True' ">
    <Exec ConsoleToMSBuild="true" ContinueOnError="true" WorkingDirectory="$(ProjectDir)" EnvironmentVariables="ASPNETCORE_ENVIRONMENT=Development" Command="$(NSwagExe_Net80) run config.nswag /variables:Configuration=$(Configuration)">
      <Output TaskParameter="ExitCode" PropertyName="NSwagExitCode" />
      <Output TaskParameter="ConsoleOutput" PropertyName="NSwagOutput" />
    </Exec>
    <Message Text="$(NSwagOutput)" Condition="'$(NSwagExitCode)' == '0'" Importance="low" />
    <Error Text="$(NSwagOutput)" Condition="'$(NSwagExitCode)' != '0'" />
  </Target>
-->
```

## Paso 7: Crear Nuevas Migraciones para MySQL

```bash
dotnet ef migrations add InitialCreateMySQL --project src/Infrastructure --startup-project src/Web
```

## Paso 8: Aplicar Migraciones

```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Web
```

## Paso 9: Rehabilitar NSwag

Una vez creadas las tablas, descomenta el target de NSwag en `src/Web/Web.csproj`.

## Verificación

Para verificar que todo funciona correctamente:

1. **Compilar el proyecto:**
   ```bash
   dotnet build
   ```

2. **Ejecutar la aplicación:**
   ```bash
   dotnet run --project src/Web
   ```

3. **Verificar la base de datos:**
   Conectarse a MySQL y verificar que las tablas se crearon correctamente:
   ```sql
   USE cleanarchitecture_db;
   SHOW TABLES;
   ```

## Diferencias Clave entre SQL Server y MySQL

| Aspecto | SQL Server | MySQL |
|---------|------------|-------|
| Proveedor EF Core | `Microsoft.EntityFrameworkCore.SqlServer` | `Pomelo.EntityFrameworkCore.MySQL` |
| Connection String | `Server=...;Database=...;Trusted_Connection=true;` | `server=...;database=...;user=...;password=...;` |
| Tipos de Datos | `nvarchar(max)` | `VARCHAR(255)`, `TEXT` |
| Auto Increment | `IDENTITY` | `AUTO_INCREMENT` |
| Configuración | `UseSqlServer()` | `UseMySql()` con `ServerVersion` |

## Problemas Comunes y Soluciones

### Error: "nvarchar(max) NULL" - Sintaxis MySQL
**Causa:** Migraciones de SQL Server aplicadas a MySQL
**Solución:** Eliminar migraciones existentes y crear nuevas para MySQL

### Error: "Table doesn't exist" durante build
**Causa:** NSwag intenta acceder a la base de datos antes de que las tablas existan
**Solución:** Deshabilitar temporalmente NSwag hasta crear las migraciones

### Error: "Microsoft.EntityFrameworkCore.Design" not referenced
**Causa:** Paquete de herramientas de EF Core faltante en proyecto startup
**Solución:** Instalar `Microsoft.EntityFrameworkCore.Design` en el proyecto Web

## Comandos de Referencia

```bash
# Crear migración
dotnet ef migrations add <NombreMigracion> --project src/Infrastructure --startup-project src/Web

# Aplicar migraciones
dotnet ef database update --project src/Infrastructure --startup-project src/Web

# Eliminar última migración
dotnet ef migrations remove --project src/Infrastructure --startup-project src/Web

# Ver migraciones
dotnet ef migrations list --project src/Infrastructure --startup-project src/Web
```

## Notas Adicionales

- **Backup:** Siempre hacer backup de los datos antes de aplicar migraciones en producción
- **Versionado:** Usar `ServerVersion.AutoDetect()` para detectar automáticamente la versión de MySQL
- **Performance:** MySQL puede tener diferencias de rendimiento comparado con SQL Server
- **Tipos de Datos:** Revisar que los tipos de datos se mapeen correctamente entre proveedores

La migración está completa cuando:
- ✅ La aplicación compila sin errores
- ✅ Las migraciones se aplican exitosamente
- ✅ La aplicación se ejecuta y conecta a MySQL
- ✅ Las funcionalidades básicas funcionan correctamente
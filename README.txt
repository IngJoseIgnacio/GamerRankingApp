######################################################
#              GUÍA DE INSTALACIÓN                   #
#              GamerRankingApp                       #
######################################################

Este documento detalla los pasos necesarios para instalar y configurar la aplicación web GamerRankingApp
utilizando Visual Studio 2022, .NET Framework 4.8 y SQL Server.

######################################################
#                   REQUISITOS                       #
######################################################

Antes de iniciar, asegúrate de tener instalados los siguientes componentes:

1.  **Visual Studio 2022:** Con las cargas de trabajo de "Desarrollo de ASP.NET y web" y "Desarrollo de escritorio con .NET".
2.  **SQL Server:** Una instancia de SQL Server (puede ser SQL Server Express o LocalDB, que viene con Visual Studio).
3.  **Conexión a Internet:** Para descargar paquetes NuGet.

######################################################
#               PASOS DE INSTALACIÓN                 #
######################################################

Sigue estos pasos para poner en marcha la aplicación:

**Paso 1: Clonar o Descargar el Proyecto**

1.  Si el proyecto está en un repositorio Git (GitHub, GitLab, Bitbucket), clónalo a tu máquina local:
    ```bash
    git clone <https://github.com/IngJoseIgnacio/GamerRankingApp.git>
    ```
2.  Si lo recibiste como un archivo ZIP, descomprímelo en una ubicación de tu elección.

**Paso 2: Abrir el Proyecto en Visual Studio 2022**

1.  Abre Visual Studio 2022.
2.  Selecciona `Abrir un proyecto o solución`.
3.  Navega a la carpeta donde clonaste/descomprimiste el proyecto y selecciona el archivo de solución (`.sln`) (ej. `GamerRankingApp.sln`).

**Paso 3: Configurar la Cadena de Conexión a la Base de Datos**

1.  En el `Explorador de soluciones` de Visual Studio, abre el archivo `Web.config` (en la raíz del proyecto).
2.  Localiza la sección `<connectionStrings>`.
3.  Asegúrate de que la cadena de conexión con `name="DefaultConnection"` apunte a tu instancia de SQL Server.

    * **Ejemplo para SQL LocalDB (común con VS):**
        ```xml
        <add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-GamerRankingApp.mdf;Initial Catalog=aspnet-GamerRankingApp;Integrated Security=True"
          providerName="System.Data.SqlClient" />
        ```
        *Nota: La ruta exacta del archivo `.mdf` dentro de `|DataDirectory|` puede variar.*

    * **Ejemplo para una instancia de SQL Server externa (reemplaza `TU_SERVIDOR_SQL` por el nombre de tu servidor/instancia):**
        ```xml
        <add name="DefaultConnection" connectionString="Data Source=TU_SERVIDOR_SQL;Initial Catalog=GamerRankingDB;Integrated Security=True;MultipleActiveResultSets=true" providerName="System.Data.SqlClient" />
        ```
        *Si usas autenticación de SQL Server, reemplaza `Integrated Security=True` por `User ID=<TuUsuario>;Password=<TuContraseña>`. Asegúrate de que el Initial Catalog no sea una base de datos existente o que puedas sobreescribirla.*

**Paso 4: Ejecutar Migraciones de Entity Framework**

Este paso creará la base de datos (si no existe), sus tablas y poblará los datos iniciales (videojuegos, roles y usuarios).

1.  En Visual Studio, ve a `Herramientas` -> `Administrador de paquetes NuGet` -> `Consola del Administrador de paquetes`.
2.  En la `Consola del Administrador de paquetes`, asegúrate de que el `Proyecto predeterminado` esté configurado como `GamerRankingApp`.
3.  Ejecuta los siguientes comandos en orden:
    * **Habilitar Migraciones (si no se ha hecho antes):**
        ```powershell
        Enable-Migrations
        ```
        *Esto creará la carpeta `Migrations` y el archivo `Configuration.cs` si no existen. Si ya están, puedes saltar este comando.*

    * **Crear la Migración Inicial (si no se ha hecho antes):**
        ```powershell
        Add-Migration InitialCreate
        ```
        *Esto escaneará tus modelos de datos y generará un archivo de migración con la estructura de la base de datos y la lógica de `Seed` para datos iniciales, roles y usuarios. Si ya existe `InitialCreate` u otra migración, puedes omitir este paso si solo quieres actualizar la base de datos existente.*

    * **Actualizar la Base de Datos y Ejecutar el Método `Seed`:**
        ```powershell
        Update-Database
        ```
        *Este comando aplicará las migraciones pendientes a tu base de datos. Lo más importante es que ejecutará el método `Seed` en `Migrations/Configuration.cs`, que poblará la tabla `Videojuegos` y creará los roles (`Administrator`, `Auxiliar de Registro`) y los usuarios iniciales:*
            * **Usuario Administrador:** `admin@gamerranking.com` / Contraseña: `AdminP@ssw0rd1!`
            * **Usuario Auxiliar de Registro:** `auxiliar@gamerranking.com` / Contraseña: `AuxiliarP@ssw0rd1!`

**Paso 5: Ejecutar el Procedimiento Almacenado de Generación de Calificaciones Masivas (Opcional)**

El proyecto incluye un procedimiento almacenado para generar 1,000,000 de calificaciones de forma aleatoria. Esto puede llevar tiempo.

1.  Abre SQL Server Management Studio (SSMS) o cualquier cliente SQL.
2.  Conéctate a tu instancia de SQL Server donde se creó la base de datos `GamerRankingDB` (o el nombre que configuraste).
3.  Abre el archivo `SqlScripts/1_GenerateRandomCalificaciones.sql` desde la carpeta del proyecto.
4.  Ejecuta todo el script. Esto creará el procedimiento almacenado `GenerateRandomCalificaciones` y luego lo ejecutará para insertar 1,000,000 de registros en la tabla `Calificaciones`.

    * Alternativamente, puedes ejecutar la generación de calificaciones desde la página `Admin/GenerateCalificaciones.aspx` de la aplicación una vez que esté funcionando (accesible solo para el rol `Administrator`).

**Paso 6: Compilar y Ejecutar la Aplicación**

1.  En Visual Studio, construye la solución para asegurarte de que no haya errores de compilación (`Ctrl + Shift + B`).
2.  Ejecuta la aplicación presionando `F5` o haciendo clic en el botón `IIS Express` en la barra de herramientas de Visual Studio.
3.  El navegador web se abrirá con la página de inicio de la aplicación.

**Paso 7: Probar la Aplicación**

1.  Navega a `~/Account/Login.aspx` para iniciar sesión.
2.  Usa las credenciales del usuario `Administrator` (`admin@gamerranking.com` / `AdminP@ssw0rd1!`) para probar todas las funcionalidades, incluida la gestión de videojuegos y la generación de ranking CSV.
3.  Usa las credenciales del usuario `Auxiliar de Registro` (`auxiliar@gamerranking.com` / `AuxiliarP@ssw0rd1!`) para probar las funcionalidades permitidas para este rol (ej. CRUD de videojuegos excepto eliminación).

¡Con estos pasos, tu aplicación GamerRankingApp debería estar instalada y funcionando correctamente!

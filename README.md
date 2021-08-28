# Template MVC
Este es un template básico para .Net Core MVC. Todavía es un trabajo en progreso, que por cuestiones de tiempo debí suspender.

## Incluye:
- Instalación de repositorio de git.
- Instalación de Swagger para documentar endpoints con Open API.
- Conexión con un servidor local MySQL usando EntityFramework con un contexto independiente.
- Implementación básica de controladores para recibir las request y consultar la capa de servicios.

## ToDo:
- Segmentar mejor los archivos que se suben al template (prunear los archivos innecesarios)
- Agregar soporte default para AutoMapper
- Agregar proyecto default para testing con xUnit (o similar)
- Agregar soporte para MongoDB
- Agregar soporte para Redis
- Agregar docker-compose y docker images necesarias para levantar los servicios locales (como MySQL) automáticamente

## Instrucciones de Instalación
1) Instalé Visual Studio
2) GitHub
    1. Cree una cuenta
    2. Cree un repositorio
    3. Copié el link del remote que tenía que agregar al repositorio local (Ej: https://github.com/jandresleiva/mvc-netcore3.git)
    4. Entré a PowerShell y me dirigí a la carpeta local
    5. Corrí los comandos para inicializar el repositorio local
        1. `git init`
        2. (Que haya por lo menos un archivo) `git add ./`
        3. `git commit -m "Initial commit"`
        4. `git remote add origin [link del remote]`
        5. `git push` (y me prompteo para ingresar usuario y contraseña)
        6. Abrí la ventana de Team Explorer `View > Team Explorer` y automáticamente me tomó la configuración

3) Swagger
    1. Fui al manager de paquetes `Tools > NuGet Package Manager > Manage NuGet Packages for Solution` 
    2. Fui a la pestaña Browse
    3. Busqué `Swashbuckle.AspNetCore`
    4. Tildé la caja Include prerelease (junto al cuadro de búsuqeda)
    5. Lo seleccioné y tildé el proyecto en la caja de la derecha.
    6. Debajo seleccioné la versión específica `5.0.0-rc4` (IMPORTANTE!)
    7. Presioné Install y acepté las licencias.
    8. Volví al Solution Explorer y abrí el Startup.cs para agregar y configurar el Swagger middleware
        1. Agregué `using Microsoft.OpenApi.Models;`
        2. Agregué el generador a la colección de servicios en el método `Startup.ConfigureServices` debajo del `AddControllers`
        ```
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                });
        ```
        3. Habilité el middleware en el método Startup.Configure para servir el JSON generado y el Swagger UI arriba de todo.
        ```
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        ```
4) MySQL
    1) Instalé el `MySQL.Data` desde el NuGet Manager
    2) Agregué al `appsettings.json` la siguiente entrada:
    ```
        "ConnectionStrings": {
            "DefaultConnection": "server=localhost;port=3306;database=sysadcon;user=root;password="
        }
    ```
    3) Cree una carpeta llamada Data y dentro una clase `APIDbContext.cs` para generar el contexto de la DB
    ```
        using Microsoft.EntityFrameworkCore;
        using MVC_netcore3.Models;

        namespace MVC_netcore3.Data
        {
          public class APIDbContext : DbContext
          {

            public DbSet<Empleado> Empleado { get; set; }
            public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
            {
            }
          }
        }
    ```
    4) Registro el contexto entre los servicios en el archivo `Startup.cs`
        1) Agrego las llamadas
        ```
            using Microsoft.EntityFrameworkCore;
            using MVC_netcore3.Data;
            using Pomelo.EntityFrameworkCore.MySql.Storage;
            using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
        ```
        2) Y agregando a `ConfigureServices`
        ```
              services.AddDbContextPool<APIDbContext>(options => options
                  .UseMySql( Configuration.GetConnectionString("DefaultConnection"), mySqlOptions => mySqlOptions
                      .ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql)) 
                ));
              }
        ```
    5) Creo el modelo correspondiente a la tabla, por ejemplo Empleado, en la carpeta `Models`
        ```
                public class Empleado
                {
                    public int id { set; get; }
                    public string nombre { set; get; }
                    public int dni { set; get; }
                    //public DateTime fecha_nacimiento { set; get; }
                    public int telefono { set; get; }
                    public int telefono2 { set; get; }
                    public string email { set; get; }
                    public string domicilio { set; get; }
                    public string rol { set; get; }
                //public DateTime fecha_alta { set; get; }
                //public DateTime fecha_borrado { set; get; }

              }
        ```
    6) Modifico el controller
        1) Llamo las dependencias:
        ```
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Microsoft.Extensions.Logging;
            using MVC_netcore3.Models;
            using MVC_netcore3.Data;
        ```
        2) Dentro de la clase principal (por ejemplo: `public class WeatherForecastController : ControllerBase`) agrego la declaración de la instancia del contexto
        ```
            private readonly APIDbContext _context;
        ```
        3) En el constructor de la clase, seteo el contexto:
        ```
            public WeatherForecastController(APIDbContext context)
            {
                _context = context;
            }
        ```
        4) En algno de los llamados `HTTP` (por ejemplo `GET`), interactúo con la DB:
        ```
            [HttpGet]
            public async Task<Empleado> Get(int id)
                {
                var empleado = await _context.Empleado.FindAsync(id);
                return empleado;
            }
        ```

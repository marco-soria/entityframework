using efproject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetEnv; // Importa DotNetEnv. se debe instalar un paquete de NuGet con dotnet add package DotNetEnv

var builder = WebApplication.CreateBuilder(args);

// Cargar el archivo .env
Env.Load();


//revealing connection
//builder.Services.AddSqlServer<TasksContext>(builder.Configuration.GetConnectionString("SQLServerConnection"));


// Obtener la cadena de conexión
var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");

// Imprimir la cadena de conexión para verificación
Console.WriteLine($"Connection String from .env: {connectionString}");

// Validar que la cadena de conexión no esté vacía
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The connection string was not found in the environment variables.");
}

// Configurar el contexto de base de datos para usar SQL Server con la cadena de conexión
builder.Services.AddDbContext<TasksContext>(options =>
    options.UseSqlServer(connectionString));


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconnection", async ([FromServices] TasksContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok($"Database connection is {dbContext.Database.IsInMemory()}");
});

app.Run();

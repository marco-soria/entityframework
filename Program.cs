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
//Console.WriteLine($"Connection String from .env: {connectionString}");

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

app.MapGet("/api/tasks", async ([FromServices] TasksContext dbContext)=>
{
    //return Results.Ok(dbContext.Tasks.Include(p=> p.Category).Where(p=> p.TaskPriority == efproject.Models.Priority.Low));
    return Results.Ok(dbContext.Tasks.Include(p=> p.Category));
});

app.MapPost("/api/tasks", async ([FromServices] TasksContext dbContext, [FromBody] efproject.Models.Task task)=>
{
    task.TaskId = Guid.NewGuid();
    task.CreationDate = DateTime.Now;
    await dbContext.AddAsync(task);
    //await dbContext.Tasks.AddAsync(task);

    await dbContext.SaveChangesAsync();

    return Results.Ok();   
});

app.MapPut("/api/tasks/{id}", async ([FromServices] TasksContext dbContext, [FromBody] efproject.Models.Task task,[FromRoute] Guid id)=>
{
    var currentTask = dbContext.Tasks.Find(id);

    if(currentTask!=null)
    {
        currentTask.CategoryId = task.CategoryId;
        currentTask.Title = task.Title;
        currentTask.TaskPriority = task.TaskPriority;
        currentTask.Description = task.Description;

        await dbContext.SaveChangesAsync();

        return Results.Ok();

    }

    return Results.NotFound();   
});

app.MapDelete("/api/tasks/{id}", async ([FromServices] TasksContext dbContext, [FromRoute] Guid id) =>
{
     var currentTask = dbContext.Tasks.Find(id);

     if(currentTask!=null)
     {
         dbContext.Remove(currentTask);
         await dbContext.SaveChangesAsync();

         return Results.Ok();
     }

     return Results.NotFound();
});


app.Run();

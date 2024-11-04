using efproject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<TasksContext>(p => p.UseInMemoryDatabase("TasksDB"));  
//builder.Services.AddSqlServer<TasksContext>("Data Source=DESKTOP-OJPGDF3;Initial Catalog=TasksDb;Trusted_Connection=True; Integrated Security=True;user id=sa;password=YourStrong!Passw0rd;TrustServerCertificate=True");

//builder.Services.AddSqlServer<TasksContext>("Server=sqlserver;Database=TasksDB;User=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True; Integrated Security=True");

builder.Services.AddSqlServer<TasksContext>(builder.Configuration.GetConnectionString("SQLServerConnection"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconnection", async ([FromServices] TasksContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok($"Database connection is {dbContext.Database.IsInMemory()}");
});

app.Run();

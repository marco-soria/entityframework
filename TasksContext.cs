using Microsoft.EntityFrameworkCore;
using efproject.Models;

namespace efproject;

public class TasksContext: DbContext
{
    public DbSet<Category> Categories {get;set;}
    public DbSet<efproject.Models.Task> Tasks {get;set;}

    public TasksContext(DbContextOptions<TasksContext> options) :base(options) { }
}
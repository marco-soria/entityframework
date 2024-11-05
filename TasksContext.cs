using Microsoft.EntityFrameworkCore;
using efproject.Models;

namespace efproject;

public class TasksContext: DbContext
{
    public DbSet<Category> Categories {get;set;}
    public DbSet<efproject.Models.Task> Tasks {get;set;}

    public TasksContext(DbContextOptions<TasksContext> options) :base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(category=> 
        {
            List<Category> categoriesInit = new List<Category>();
        categoriesInit.Add(new Category() { CategoryId = Guid.Parse("fe2de405-c38e-4c90-ac52-da0540dfb4ef"), Name = "Pending Activities", Weight = 20});
        categoriesInit.Add(new Category() { CategoryId = Guid.Parse("fe2de405-c38e-4c90-ac52-da0540dfb402"), Name = "Personal Activities", Weight = 50});

            category.ToTable("Category");
            category.HasKey(p=> p.CategoryId);

            category.Property(p=> p.Name).IsRequired().HasMaxLength(150);

            category.Property(p=> p.Description).IsRequired(false);

            category.Property(p=> p.Weight);

            category.HasData(categoriesInit);
        });

        List<efproject.Models.Task> tasksInit = new List<efproject.Models.Task>();

        tasksInit.Add(new efproject.Models.Task() { TaskId = Guid.Parse("fe2de405-c38e-4c90-ac52-da0540dfb410"), CategoryId = Guid.Parse("fe2de405-c38e-4c90-ac52-da0540dfb4ef"), TaskPriority = Priority.Medium, Title = "Pay Utilities", CreationDate = DateTime.Now });
        tasksInit.Add(new efproject.Models.Task() { TaskId  = Guid.Parse("fe2de405-c38e-4c90-ac52-da0540dfb411"), CategoryId = Guid.Parse("fe2de405-c38e-4c90-ac52-da0540dfb402"), TaskPriority = Priority.Low, Title = "Finish watching a movie on Netflix", CreationDate = DateTime.Now });

         modelBuilder.Entity<efproject.Models.Task>(task=>
        {
            task.ToTable("Task");
            task.HasKey(p=> p.TaskId);

            task.HasOne(p=> p.Category).WithMany(p=> p.Tasks).HasForeignKey(p=> p.CategoryId);

            task.Property(p=> p.Title).IsRequired().HasMaxLength(200);

            task.Property(p=> p.Description).IsRequired(false);

            task.Property(p=> p.TaskPriority);

            task.Property(p=> p.CreationDate);

            task.Ignore(p=> p.Summary);

            task.HasData(tasksInit);

        });

    }
}
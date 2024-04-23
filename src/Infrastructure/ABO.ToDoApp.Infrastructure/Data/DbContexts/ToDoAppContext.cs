using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Infrastructure.Data.EntitieMappings;
using ABO.ToDoApp.Infrastructure.Data.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ABO.ToDoApp.Infrastructure.Data.DbContexts;

public class ToDoAppContext(DbContextOptions<ToDoAppContext> options) : 
    IdentityDbContext<User>(options)
{
    public DbSet<TodoList> TodoLists { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new TodoListMapping());
        builder.ApplyConfiguration(new TodoItemMapping());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(new SaveChangesInterceptor());
    }
}

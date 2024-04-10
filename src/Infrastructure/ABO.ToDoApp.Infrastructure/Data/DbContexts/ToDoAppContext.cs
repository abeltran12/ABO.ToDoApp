using ABO.ToDoApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ABO.ToDoApp.Infrastructure.Data.DbContexts;

public class ToDoAppContext(DbContextOptions<ToDoAppContext> options) : IdentityDbContext(options)
{
    public DbSet<TodoList> TodoLists { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

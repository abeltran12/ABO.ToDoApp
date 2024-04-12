using ABO.ToDoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABO.ToDoApp.Infrastructure.Data.EntitieMappings;

public class TodoItemMapping : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar");

        builder
            .Property(x => x.Description)
            .HasMaxLength(100)
            .HasColumnType("varchar");

        builder
           .Property(x => x.Duedate)
           .IsRequired()
           .HasColumnType("date");

        builder
            .Property(x => x.Status)
            .IsRequired();

        builder
            .Property(x => x.TodoListId)
            .IsRequired();

        //Shadow property
        builder.Property<DateTime>("CreationDate")
            .IsRequired().HasColumnName("CreationDate");
        builder.Property<DateTime>("UpdatedDate")
            .IsRequired().HasColumnName("UpdatedDate");
    }
}

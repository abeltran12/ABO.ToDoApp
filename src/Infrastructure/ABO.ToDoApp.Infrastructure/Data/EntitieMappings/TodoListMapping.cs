using ABO.ToDoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABO.ToDoApp.Infrastructure.Data.EntitieMappings;

public class TodoListMapping : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder
            .ToTable("TodoLists")
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar");

        builder
            .Property(x => x.UserId)
            .IsRequired();

        builder
            .Property(x => x.Status)
            .IsRequired();

        //Shadow property
        builder.Property<DateTime>("CreationDate")
            .IsRequired().HasColumnName("CreationDate");
        builder.Property<DateTime>("UpdatedDate")
            .IsRequired().HasColumnName("UpdatedDate");
    }
}
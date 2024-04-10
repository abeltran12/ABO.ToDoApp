using ABO.ToDoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABO.ToDoApp.Infrastructure.Data.EntitieMappings;

public class TodoListMapping : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar");

        builder
            .Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(1);

        //Shadow property
        builder.Property<DateTime>("CreationDate")
            .IsRequired().HasColumnName("CreationDate").HasDefaultValue(DateTime.UtcNow);
        builder.Property<DateTime>("UpdatedDate")
            .IsRequired().HasColumnName("UpdatedDate").HasDefaultValue(DateTime.UtcNow); ;
        builder.Property<int>("CreatedBy")
            .IsRequired().HasColumnName("CreatedBy");
        builder.Property<int>("UpdatedBy")
            .IsRequired().HasColumnName("UpdatedBy");
    }
}
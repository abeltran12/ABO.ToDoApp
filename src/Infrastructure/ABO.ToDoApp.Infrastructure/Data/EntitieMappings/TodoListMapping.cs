﻿using ABO.ToDoApp.Domain.Entities;
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
            .Property(x => x.Description)
            .HasMaxLength(150)
            .HasColumnType("varchar");

        builder
            .Property(x => x.UserId)
            .HasMaxLength(100)
            .HasColumnType("varchar")
            .IsRequired();

        builder
            .Property(x => x.Status)
            .IsRequired();

        builder
            .HasQueryFilter(g => g.Status != Status.Deleted);

        //Shadow property
        builder.Property<DateTime>("CreationDate")
            .IsRequired().HasColumnName("CreationDate");
        builder.Property<DateTime>("UpdatedDate")
            .IsRequired().HasColumnName("UpdatedDate");
    }
}
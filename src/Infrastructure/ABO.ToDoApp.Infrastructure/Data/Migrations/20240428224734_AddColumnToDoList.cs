using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABO.ToDoApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnToDoList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TodoLists",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TodoLists");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo_App.Infrastructure.Persistence.Migrations
{
    public partial class Add_Delted_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "TodoLists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "TodoItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "TodoItems");
        }
    }
}

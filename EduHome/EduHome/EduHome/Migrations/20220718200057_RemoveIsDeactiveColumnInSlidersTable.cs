using Microsoft.EntityFrameworkCore.Migrations;

namespace EduHome.Migrations
{
    public partial class RemoveIsDeactiveColumnInSlidersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "Sliders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "Sliders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

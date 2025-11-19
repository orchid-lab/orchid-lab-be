using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Current_Stage_For_Report : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StageId",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageId",
                table: "Reports");
        }
    }
}

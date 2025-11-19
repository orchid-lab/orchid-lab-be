using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Process_Status_To_Linked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Linkeds");

            migrationBuilder.AddColumn<int>(
                name: "ProcessStatus",
                table: "Linkeds",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessStatus",
                table: "Linkeds");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Linkeds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

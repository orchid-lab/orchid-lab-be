using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Soft_Delete_For_Report : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Delete_by",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Delete_date",
                table: "Reports",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Update_by",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Update_date",
                table: "Reports",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delete_by",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Delete_date",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Update_by",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Update_date",
                table: "Reports");
        }
    }
}

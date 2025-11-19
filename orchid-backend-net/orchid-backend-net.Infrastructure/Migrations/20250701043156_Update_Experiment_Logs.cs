using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Experiment_Logs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Create_by",
                table: "ExperimentLogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Create_date",
                table: "ExperimentLogs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Delete_by",
                table: "ExperimentLogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Delete_date",
                table: "ExperimentLogs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Update_by",
                table: "ExperimentLogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Update_date",
                table: "ExperimentLogs",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create_by",
                table: "ExperimentLogs");

            migrationBuilder.DropColumn(
                name: "Create_date",
                table: "ExperimentLogs");

            migrationBuilder.DropColumn(
                name: "Delete_by",
                table: "ExperimentLogs");

            migrationBuilder.DropColumn(
                name: "Delete_date",
                table: "ExperimentLogs");

            migrationBuilder.DropColumn(
                name: "Update_by",
                table: "ExperimentLogs");

            migrationBuilder.DropColumn(
                name: "Update_date",
                table: "ExperimentLogs");
        }
    }
}

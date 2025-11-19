using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Name_To_Task_Attribute_And_Experiment_Log : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Referents");

            migrationBuilder.AddColumn<string>(
                name: "MeasurementUnit",
                table: "TaskAttributes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TaskAttributes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MeasurementUnit",
                table: "Referents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ExperimentLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurementUnit",
                table: "TaskAttributes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TaskAttributes");

            migrationBuilder.DropColumn(
                name: "MeasurementUnit",
                table: "Referents");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ExperimentLogs");

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Referents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

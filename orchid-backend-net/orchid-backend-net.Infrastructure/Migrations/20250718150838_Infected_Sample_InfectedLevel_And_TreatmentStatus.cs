using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Infected_Sample_InfectedLevel_And_TreatmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "InfectedSamples");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Samples",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InfectedLevel",
                table: "InfectedSamples",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TreatmentStatus",
                table: "InfectedSamples",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "InfectedLevel",
                table: "InfectedSamples");

            migrationBuilder.DropColumn(
                name: "TreatmentStatus",
                table: "InfectedSamples");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "InfectedSamples",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

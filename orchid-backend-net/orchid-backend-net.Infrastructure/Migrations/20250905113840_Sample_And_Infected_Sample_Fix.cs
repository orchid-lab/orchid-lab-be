using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sample_And_Infected_Sample_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InfectedSamples_SampleID",
                table: "InfectedSamples");

            migrationBuilder.DropColumn(
                name: "InfectedLevel",
                table: "InfectedSamples");

            migrationBuilder.DropColumn(
                name: "TreatmentStatus",
                table: "InfectedSamples");

            migrationBuilder.CreateIndex(
                name: "IX_InfectedSamples_SampleID",
                table: "InfectedSamples",
                column: "SampleID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InfectedSamples_SampleID",
                table: "InfectedSamples");

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

            migrationBuilder.CreateIndex(
                name: "IX_InfectedSamples_SampleID",
                table: "InfectedSamples",
                column: "SampleID");
        }
    }
}

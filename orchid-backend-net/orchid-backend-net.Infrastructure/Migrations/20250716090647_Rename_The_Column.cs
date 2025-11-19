using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename_The_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExperimentLogStageTaskId",
                table: "Linkeds",
                newName: "StageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StageID",
                table: "Linkeds",
                newName: "ExperimentLogStageTaskId");
        }
    }
}

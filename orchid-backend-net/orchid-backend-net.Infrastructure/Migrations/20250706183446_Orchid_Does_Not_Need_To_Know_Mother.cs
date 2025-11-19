using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Orchid_Does_Not_Need_To_Know_Mother : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMother",
                table: "Hybridization");

            migrationBuilder.RenameColumn(
                name: "Mother",
                table: "Seedlings",
                newName: "Parent2");

            migrationBuilder.RenameColumn(
                name: "Father",
                table: "Seedlings",
                newName: "Parent1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Parent2",
                table: "Seedlings",
                newName: "Mother");

            migrationBuilder.RenameColumn(
                name: "Parent1",
                table: "Seedlings",
                newName: "Father");

            migrationBuilder.AddColumn<bool>(
                name: "IsMother",
                table: "Hybridization",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

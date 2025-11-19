using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Scientific_Name_For_Seedling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Seedlings",
                newName: "ScientificName");

            migrationBuilder.AddColumn<string>(
                name: "LocalName",
                table: "Seedlings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalName",
                table: "Seedlings");

            migrationBuilder.RenameColumn(
                name: "ScientificName",
                table: "Seedlings",
                newName: "Name");
        }
    }
}

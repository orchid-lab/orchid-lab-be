using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Value_To_Report_Attribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "ReportAttributes",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "ReportAttributes");
        }
    }
}

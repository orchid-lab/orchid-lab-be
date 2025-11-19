using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Report_Manage_Review : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"ALTER TABLE ""Reports"" 
                ALTER COLUMN ""Status"" TYPE integer 
                USING CASE WHEN ""Status"" = true THEN 1 ELSE 0 END;"
            );

            migrationBuilder.AddColumn<string>(
                name: "ReviewReprot",
                table: "Reports",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewReprot",
                table: "Reports");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Reports",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}

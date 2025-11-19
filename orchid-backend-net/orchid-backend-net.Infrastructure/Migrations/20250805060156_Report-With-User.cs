using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReportWithUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM ""Reports""
                WHERE ""TechnicianID"" IS NOT NULL
                AND ""TechnicianID"" NOT IN (SELECT ""ID"" FROM ""Users"");");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TechnicianID",
                table: "Reports",
                column: "TechnicianID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_TechnicianID",
                table: "Reports",
                column: "TechnicianID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_TechnicianID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_TechnicianID",
                table: "Reports");
        }
    }
}

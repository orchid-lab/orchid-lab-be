using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Many_To_Many_For_ReportAttributes_And_Referent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportAttributes_Referents_ReferentDataID",
                table: "ReportAttributes");

            migrationBuilder.DropIndex(
                name: "IX_ReportAttributes_ReferentDataID",
                table: "ReportAttributes");

            migrationBuilder.DropColumn(
                name: "ReferentDataID",
                table: "ReportAttributes");

            migrationBuilder.CreateTable(
                name: "ReferentsReportAttributes",
                columns: table => new
                {
                    ReferentsID = table.Column<int>(type: "integer", nullable: false),
                    ReportAttributesID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferentsReportAttributes", x => new { x.ReferentsID, x.ReportAttributesID });
                    table.ForeignKey(
                        name: "FK_ReferentsReportAttributes_Referents_ReferentsID",
                        column: x => x.ReferentsID,
                        principalTable: "Referents",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferentsReportAttributes_ReportAttributes_ReportAttributes~",
                        column: x => x.ReportAttributesID,
                        principalTable: "ReportAttributes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferentsReportAttributes_ReportAttributesID",
                table: "ReferentsReportAttributes",
                column: "ReportAttributesID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferentsReportAttributes");

            migrationBuilder.AddColumn<int>(
                name: "ReferentDataID",
                table: "ReportAttributes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttributes_ReferentDataID",
                table: "ReportAttributes",
                column: "ReferentDataID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAttributes_Referents_ReferentDataID",
                table: "ReportAttributes",
                column: "ReferentDataID",
                principalTable: "Referents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

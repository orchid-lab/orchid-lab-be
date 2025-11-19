using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class For_Report_To_Check_Sample_Attribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferentsReportAttributes");

            migrationBuilder.AddColumn<string>(
                name: "Create_by",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Create_date",
                table: "Reports",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLatest",
                table: "Reports",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ReportAttributes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferentID",
                table: "ReportAttributes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttributes_ReferentID",
                table: "ReportAttributes",
                column: "ReferentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAttributes_Referents_ReferentID",
                table: "ReportAttributes",
                column: "ReferentID",
                principalTable: "Referents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportAttributes_Referents_ReferentID",
                table: "ReportAttributes");

            migrationBuilder.DropIndex(
                name: "IX_ReportAttributes_ReferentID",
                table: "ReportAttributes");

            migrationBuilder.DropColumn(
                name: "Create_by",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Create_date",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IsLatest",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ReportAttributes");

            migrationBuilder.DropColumn(
                name: "ReferentID",
                table: "ReportAttributes");

            migrationBuilder.CreateTable(
                name: "ReferentsReportAttributes",
                columns: table => new
                {
                    ReferentsID = table.Column<string>(type: "text", nullable: false),
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
    }
}

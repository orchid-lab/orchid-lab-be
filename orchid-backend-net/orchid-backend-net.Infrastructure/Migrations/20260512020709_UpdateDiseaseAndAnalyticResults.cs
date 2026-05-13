using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDiseaseAndAnalyticResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anthracnose",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "BacterialWilt",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "Blackrot",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "Brownspots",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "Healthy",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "MoldBacterial",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "MoldFungus",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "Oxidation",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "SoftRot",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "StemRot",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "Virus",
                table: "AnalyticResults");

            migrationBuilder.RenameColumn(
                name: "WitheredYellowRoot",
                table: "AnalyticResults",
                newName: "Confidence");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Diseases",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Diseases",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AnalyzedAt",
                table: "AnalyticResults",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AnalyzedBy",
                table: "AnalyticResults",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredictionsJson",
                table: "AnalyticResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TopDisease",
                table: "AnalyticResults",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "AnalyzedAt",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "AnalyzedBy",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "PredictionsJson",
                table: "AnalyticResults");

            migrationBuilder.DropColumn(
                name: "TopDisease",
                table: "AnalyticResults");

            migrationBuilder.RenameColumn(
                name: "Confidence",
                table: "AnalyticResults",
                newName: "WitheredYellowRoot");

            migrationBuilder.AddColumn<decimal>(
                name: "Anthracnose",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BacterialWilt",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Blackrot",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Brownspots",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Healthy",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MoldBacterial",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MoldFungus",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Oxidation",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SoftRot",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "StemRot",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Virus",
                table: "AnalyticResults",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Task_Details : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ElementInStageId",
                table: "TaskTemplateDetails",
                newName: "Element");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedValue",
                table: "TaskTemplateDetails",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Element",
                table: "TaskTemplateDetails",
                newName: "ElementInStageId");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedValue",
                table: "TaskTemplateDetails",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}

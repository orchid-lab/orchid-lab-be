using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangesEntityIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //drop foreign key constraints before altering the columns
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssigns_Users_TechnicianID",
                table: "TaskAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Methods_MethodID",
                table: "Stage");
            
            migrationBuilder.DropForeignKey(
                name: "FK_ReferentsReportAttributes_Referents_ReferentsID",
                table: "ReferentsReportAttributes");
            
            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentLogs_Methods_MethodID",
                table: "ExperimentLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementInStage_Elements_ElementID",
                table: "ElementInStage");

            //alter the columns to change their types from int to string (for Guid representation)
            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "TechnicianID",
                table: "TaskAssigns",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "MethodID",
                table: "Stage",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Mother",
                table: "Seedlings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Father",
                table: "Seedlings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ReferentsID",
                table: "ReferentsReportAttributes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "Referents",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "Methods",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "Linkeds",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "MethodID",
                table: "ExperimentLogs",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "Elements",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ElementID",
                table: "ElementInStage",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");


            //add foreign key constraints after altering the columns
            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssigns_Users_TechnicianID",
                table: "TaskAssigns",
                column: "TechnicianID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
            
            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Methods_MethodID",
                table: "Stage",
                column: "MethodID",
                principalTable: "Methods",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
            
            migrationBuilder.AddForeignKey(
                name: "FK_ReferentsReportAttributes_Referents_ReferentsID",
                table: "ReferentsReportAttributes",
                column: "ReferentsID",
                principalTable: "Referents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
            
            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentLogs_Methods_MethodID",
                table: "ExperimentLogs",
                column: "MethodID",
                principalTable: "Methods",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementInStage_Elements_ElementID",
                table: "ElementInStage",
                column: "ElementID",
                principalTable: "Elements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianID",
                table: "TaskAssigns",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "MethodID",
                table: "Stage",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Mother",
                table: "Seedlings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Father",
                table: "Seedlings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReferentsID",
                table: "ReferentsReportAttributes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Referents",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Methods",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Linkeds",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "MethodID",
                table: "ExperimentLogs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Elements",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "ElementID",
                table: "ElementInStage",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}

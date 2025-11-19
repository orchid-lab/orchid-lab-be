using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Task_Template : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskTemplates",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StageID = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTemplates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskTemplates_Stage_StageID",
                        column: x => x.StageID,
                        principalTable: "Stage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskTemplateDetails",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    TaskTemplateID = table.Column<string>(type: "text", nullable: false),
                    ElementInStageId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ExpectedValue = table.Column<decimal>(type: "numeric", nullable: true),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTemplateDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskTemplateDetails_TaskTemplates_TaskTemplateID",
                        column: x => x.TaskTemplateID,
                        principalTable: "TaskTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplateDetails_TaskTemplateID",
                table: "TaskTemplateDetails",
                column: "TaskTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplates_StageID",
                table: "TaskTemplates",
                column: "StageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskTemplateDetails");

            migrationBuilder.DropTable(
                name: "TaskTemplates");
        }
    }
}

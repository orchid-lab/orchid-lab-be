using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Prod_And_Local_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disease",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    solution = table.Column<string>(type: "text", nullable: false),
                    InfectedRate = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disease", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Element",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Element", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LabRoom",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabRoom", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Method",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Method", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Technician = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sample",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Dob = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sample", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Seedling",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Parent = table.Column<string>(type: "text", nullable: false),
                    Parent1 = table.Column<string>(type: "text", nullable: false),
                    Dob = table.Column<DateOnly>(type: "date", nullable: false),
                    Create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Create_by = table.Column<string>(type: "text", nullable: false),
                    Update_by = table.Column<string>(type: "text", nullable: false),
                    Delete_by = table.Column<string>(type: "text", nullable: false),
                    Delete_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seedling", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SeedlingAttribute",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeedlingAttribute", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StageAttribute",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageAttribute", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Researcher = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TissueCultureBatches",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    LabRoomID = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TissueCultureBatches", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TissueCultureBatches_LabRoom_LabRoomID",
                        column: x => x.LabRoomID,
                        principalTable: "LabRoom",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stage",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    MethodID = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stage_Method_MethodID",
                        column: x => x.MethodID,
                        principalTable: "Method",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Imgs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    ReportID = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imgs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Imgs_Report_ReportID",
                        column: x => x.ReportID,
                        principalTable: "Report",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    RoleID = table.Column<int>(type: "integer", nullable: false),
                    Create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Create_by = table.Column<string>(type: "text", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InfectedSamples",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    SampleID = table.Column<string>(type: "text", nullable: false),
                    DiseaseID = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfectedSamples", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InfectedSamples_Disease_DiseaseID",
                        column: x => x.DiseaseID,
                        principalTable: "Disease",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InfectedSamples_Sample_SampleID",
                        column: x => x.SampleID,
                        principalTable: "Sample",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Characteristic",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    SeedlingAttributeID = table.Column<string>(type: "text", nullable: false),
                    SeedlingID = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristic", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Characteristic_SeedlingAttribute_SeedlingAttributeID",
                        column: x => x.SeedlingAttributeID,
                        principalTable: "SeedlingAttribute",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characteristic_Seedling_SeedlingID",
                        column: x => x.SeedlingID,
                        principalTable: "Seedling",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportAttributes",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    ReferentDataID = table.Column<string>(type: "text", nullable: false),
                    ReportID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportAttributes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReportAttributes_Report_ReportID",
                        column: x => x.ReportID,
                        principalTable: "Report",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportAttributes_SeedlingAttribute_ReferentDataID",
                        column: x => x.ReferentDataID,
                        principalTable: "SeedlingAttribute",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskAttributes",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TaskID = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAttributes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskAttributes_Task_TaskID",
                        column: x => x.TaskID,
                        principalTable: "Task",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentLogs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    MethodID = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TissueCultureBatchID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentLogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExperimentLogs_Method_MethodID",
                        column: x => x.MethodID,
                        principalTable: "Method",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExperimentLogs_TissueCultureBatches_TissueCultureBatchID",
                        column: x => x.TissueCultureBatchID,
                        principalTable: "TissueCultureBatches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ElementInStage",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    StageID = table.Column<string>(type: "text", nullable: false),
                    ElementID = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementInStage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ElementInStage_Element_ElementID",
                        column: x => x.ElementID,
                        principalTable: "Element",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElementInStage_Stage_StageID",
                        column: x => x.StageID,
                        principalTable: "Stage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Referents",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StageID = table.Column<string>(type: "text", nullable: false),
                    StageAttributeID = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Referents_StageAttribute_StageAttributeID",
                        column: x => x.StageAttributeID,
                        principalTable: "StageAttribute",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Referents_Stage_StageID",
                        column: x => x.StageID,
                        principalTable: "Stage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskAssigns",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    TechnicianID = table.Column<int>(type: "integer", nullable: false),
                    TaskID = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAssigns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskAssigns_Task_TaskID",
                        column: x => x.TaskID,
                        principalTable: "Task",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAssigns_Users_TechnicianID",
                        column: x => x.TechnicianID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hybridization",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    ParentID = table.Column<string>(type: "text", nullable: false),
                    ExperimentLogID = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hybridization", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Hybridization_ExperimentLogs_ExperimentLogID",
                        column: x => x.ExperimentLogID,
                        principalTable: "ExperimentLogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hybridization_Seedling_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Seedling",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Linkeds",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskID = table.Column<string>(type: "text", nullable: false),
                    SampleID = table.Column<string>(type: "text", nullable: false),
                    ExperimentLogID = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Linkeds", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Linkeds_ExperimentLogs_ExperimentLogID",
                        column: x => x.ExperimentLogID,
                        principalTable: "ExperimentLogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Linkeds_Sample_SampleID",
                        column: x => x.SampleID,
                        principalTable: "Sample",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Linkeds_Task_TaskID",
                        column: x => x.TaskID,
                        principalTable: "Task",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characteristic_SeedlingAttributeID",
                table: "Characteristic",
                column: "SeedlingAttributeID");

            migrationBuilder.CreateIndex(
                name: "IX_Characteristic_SeedlingID",
                table: "Characteristic",
                column: "SeedlingID");

            migrationBuilder.CreateIndex(
                name: "IX_ElementInStage_ElementID",
                table: "ElementInStage",
                column: "ElementID");

            migrationBuilder.CreateIndex(
                name: "IX_ElementInStage_StageID",
                table: "ElementInStage",
                column: "StageID");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentLogs_MethodID",
                table: "ExperimentLogs",
                column: "MethodID");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentLogs_TissueCultureBatchID",
                table: "ExperimentLogs",
                column: "TissueCultureBatchID");

            migrationBuilder.CreateIndex(
                name: "IX_Hybridization_ExperimentLogID",
                table: "Hybridization",
                column: "ExperimentLogID");

            migrationBuilder.CreateIndex(
                name: "IX_Hybridization_ParentID",
                table: "Hybridization",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Imgs_ReportID",
                table: "Imgs",
                column: "ReportID");

            migrationBuilder.CreateIndex(
                name: "IX_InfectedSamples_DiseaseID",
                table: "InfectedSamples",
                column: "DiseaseID");

            migrationBuilder.CreateIndex(
                name: "IX_InfectedSamples_SampleID",
                table: "InfectedSamples",
                column: "SampleID");

            migrationBuilder.CreateIndex(
                name: "IX_Linkeds_ExperimentLogID",
                table: "Linkeds",
                column: "ExperimentLogID");

            migrationBuilder.CreateIndex(
                name: "IX_Linkeds_SampleID",
                table: "Linkeds",
                column: "SampleID");

            migrationBuilder.CreateIndex(
                name: "IX_Linkeds_TaskID",
                table: "Linkeds",
                column: "TaskID");

            migrationBuilder.CreateIndex(
                name: "IX_Referents_StageAttributeID",
                table: "Referents",
                column: "StageAttributeID");

            migrationBuilder.CreateIndex(
                name: "IX_Referents_StageID",
                table: "Referents",
                column: "StageID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttributes_ReferentDataID",
                table: "ReportAttributes",
                column: "ReferentDataID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttributes_ReportID",
                table: "ReportAttributes",
                column: "ReportID");

            migrationBuilder.CreateIndex(
                name: "IX_Stage_MethodID",
                table: "Stage",
                column: "MethodID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssigns_TaskID",
                table: "TaskAssigns",
                column: "TaskID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssigns_TechnicianID",
                table: "TaskAssigns",
                column: "TechnicianID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttributes_TaskID",
                table: "TaskAttributes",
                column: "TaskID");

            migrationBuilder.CreateIndex(
                name: "IX_TissueCultureBatches_LabRoomID",
                table: "TissueCultureBatches",
                column: "LabRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characteristic");

            migrationBuilder.DropTable(
                name: "ElementInStage");

            migrationBuilder.DropTable(
                name: "Hybridization");

            migrationBuilder.DropTable(
                name: "Imgs");

            migrationBuilder.DropTable(
                name: "InfectedSamples");

            migrationBuilder.DropTable(
                name: "Linkeds");

            migrationBuilder.DropTable(
                name: "Referents");

            migrationBuilder.DropTable(
                name: "ReportAttributes");

            migrationBuilder.DropTable(
                name: "TaskAssigns");

            migrationBuilder.DropTable(
                name: "TaskAttributes");

            migrationBuilder.DropTable(
                name: "Element");

            migrationBuilder.DropTable(
                name: "Seedling");

            migrationBuilder.DropTable(
                name: "Disease");

            migrationBuilder.DropTable(
                name: "ExperimentLogs");

            migrationBuilder.DropTable(
                name: "Sample");

            migrationBuilder.DropTable(
                name: "StageAttribute");

            migrationBuilder.DropTable(
                name: "Stage");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "SeedlingAttribute");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "TissueCultureBatches");

            migrationBuilder.DropTable(
                name: "Method");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "LabRoom");
        }
    }
}

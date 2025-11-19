using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changes_Entities_Data_Type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characteristic_SeedlingAttribute_SeedlingAttributeID",
                table: "Characteristic");

            migrationBuilder.DropForeignKey(
                name: "FK_Characteristic_Seedling_SeedlingID",
                table: "Characteristic");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementInStage_Element_ElementID",
                table: "ElementInStage");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentLogs_Method_MethodID",
                table: "ExperimentLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Hybridization_Seedling_ParentID",
                table: "Hybridization");

            migrationBuilder.DropForeignKey(
                name: "FK_Imgs_Report_ReportID",
                table: "Imgs");

            migrationBuilder.DropForeignKey(
                name: "FK_InfectedSamples_Disease_DiseaseID",
                table: "InfectedSamples");

            migrationBuilder.DropForeignKey(
                name: "FK_InfectedSamples_Sample_SampleID",
                table: "InfectedSamples");

            migrationBuilder.DropForeignKey(
                name: "FK_Linkeds_Sample_SampleID",
                table: "Linkeds");

            migrationBuilder.DropForeignKey(
                name: "FK_Linkeds_Task_TaskID",
                table: "Linkeds");

            migrationBuilder.DropForeignKey(
                name: "FK_Referents_StageAttribute_StageAttributeID",
                table: "Referents");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportAttributes_Report_ReportID",
                table: "ReportAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportAttributes_SeedlingAttribute_ReferentDataID",
                table: "ReportAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Method_MethodID",
                table: "Stage");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssigns_Task_TaskID",
                table: "TaskAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAttributes_Task_TaskID",
                table: "TaskAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TissueCultureBatches_LabRoom_LabRoomID",
                table: "TissueCultureBatches");

            migrationBuilder.DropTable(
                name: "Disease");

            migrationBuilder.DropTable(
                name: "Element");

            migrationBuilder.DropTable(
                name: "LabRoom");

            migrationBuilder.DropTable(
                name: "Method");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Sample");

            migrationBuilder.DropTable(
                name: "Seedling");

            migrationBuilder.DropTable(
                name: "SeedlingAttribute");

            migrationBuilder.DropTable(
                name: "StageAttribute");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Referents_StageAttributeID",
                table: "Referents");

            migrationBuilder.RenameColumn(
                name: "StageAttributeID",
                table: "Referents",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "DateOfProcessing",
                table: "Stage",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.AlterColumn<int>(
            //    name: "ReferentDataID",
            //    table: "ReportAttributes",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "text");
            migrationBuilder.Sql(
    "ALTER TABLE \"ReportAttributes\" ALTER COLUMN \"ReferentDataID\" TYPE integer USING \"ReferentDataID\"::integer;");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ReportAttributes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Referents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValueFrom",
                table: "Referents",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValueTo",
                table: "Referents",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Characteristic",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Solution = table.Column<string>(type: "text", nullable: false),
                    InfectedRate = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Elements",
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
                    table.PrimaryKey("PK_Elements", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LabRooms",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabRooms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Methods",
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
                    table.PrimaryKey("PK_Methods", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Samples",
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
                    table.PrimaryKey("PK_Samples", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SeedlingAttributes",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeedlingAttributes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Seedlings",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Mother = table.Column<string>(type: "text", nullable: false),
                    Father = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Seedlings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Researcher = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Create_by = table.Column<string>(type: "text", nullable: false),
                    Create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Update_by = table.Column<string>(type: "text", nullable: false),
                    Update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Delete_by = table.Column<string>(type: "text", nullable: false),
                    Delete_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TechnicianID = table.Column<string>(type: "text", nullable: false),
                    SampleID = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reports_Samples_SampleID",
                        column: x => x.SampleID,
                        principalTable: "Samples",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SampleID",
                table: "Reports",
                column: "SampleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Characteristic_SeedlingAttributes_SeedlingAttributeID",
                table: "Characteristic",
                column: "SeedlingAttributeID",
                principalTable: "SeedlingAttributes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characteristic_Seedlings_SeedlingID",
                table: "Characteristic",
                column: "SeedlingID",
                principalTable: "Seedlings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementInStage_Elements_ElementID",
                table: "ElementInStage",
                column: "ElementID",
                principalTable: "Elements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentLogs_Methods_MethodID",
                table: "ExperimentLogs",
                column: "MethodID",
                principalTable: "Methods",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hybridization_Seedlings_ParentID",
                table: "Hybridization",
                column: "ParentID",
                principalTable: "Seedlings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imgs_Reports_ReportID",
                table: "Imgs",
                column: "ReportID",
                principalTable: "Reports",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InfectedSamples_Diseases_DiseaseID",
                table: "InfectedSamples",
                column: "DiseaseID",
                principalTable: "Diseases",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InfectedSamples_Samples_SampleID",
                table: "InfectedSamples",
                column: "SampleID",
                principalTable: "Samples",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Linkeds_Samples_SampleID",
                table: "Linkeds",
                column: "SampleID",
                principalTable: "Samples",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Linkeds_Tasks_TaskID",
                table: "Linkeds",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAttributes_Referents_ReferentDataID",
                table: "ReportAttributes",
                column: "ReferentDataID",
                principalTable: "Referents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAttributes_Reports_ReportID",
                table: "ReportAttributes",
                column: "ReportID",
                principalTable: "Reports",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Methods_MethodID",
                table: "Stage",
                column: "MethodID",
                principalTable: "Methods",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssigns_Tasks_TaskID",
                table: "TaskAssigns",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAttributes_Tasks_TaskID",
                table: "TaskAttributes",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TissueCultureBatches_LabRooms_LabRoomID",
                table: "TissueCultureBatches",
                column: "LabRoomID",
                principalTable: "LabRooms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characteristic_SeedlingAttributes_SeedlingAttributeID",
                table: "Characteristic");

            migrationBuilder.DropForeignKey(
                name: "FK_Characteristic_Seedlings_SeedlingID",
                table: "Characteristic");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementInStage_Elements_ElementID",
                table: "ElementInStage");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentLogs_Methods_MethodID",
                table: "ExperimentLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Hybridization_Seedlings_ParentID",
                table: "Hybridization");

            migrationBuilder.DropForeignKey(
                name: "FK_Imgs_Reports_ReportID",
                table: "Imgs");

            migrationBuilder.DropForeignKey(
                name: "FK_InfectedSamples_Diseases_DiseaseID",
                table: "InfectedSamples");

            migrationBuilder.DropForeignKey(
                name: "FK_InfectedSamples_Samples_SampleID",
                table: "InfectedSamples");

            migrationBuilder.DropForeignKey(
                name: "FK_Linkeds_Samples_SampleID",
                table: "Linkeds");

            migrationBuilder.DropForeignKey(
                name: "FK_Linkeds_Tasks_TaskID",
                table: "Linkeds");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportAttributes_Referents_ReferentDataID",
                table: "ReportAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportAttributes_Reports_ReportID",
                table: "ReportAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Methods_MethodID",
                table: "Stage");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssigns_Tasks_TaskID",
                table: "TaskAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAttributes_Tasks_TaskID",
                table: "TaskAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TissueCultureBatches_LabRooms_LabRoomID",
                table: "TissueCultureBatches");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "LabRooms");

            migrationBuilder.DropTable(
                name: "Methods");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "SeedlingAttributes");

            migrationBuilder.DropTable(
                name: "Seedlings");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropColumn(
                name: "DateOfProcessing",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReportAttributes");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Referents");

            migrationBuilder.DropColumn(
                name: "ValueFrom",
                table: "Referents");

            migrationBuilder.DropColumn(
                name: "ValueTo",
                table: "Referents");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Referents",
                newName: "StageAttributeID");

            migrationBuilder.AlterColumn<string>(
                name: "ReferentDataID",
                table: "ReportAttributes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Characteristic",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateTable(
                name: "Disease",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    InfectedRate = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    solution = table.Column<string>(type: "text", nullable: false)
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
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
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
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Technician = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sample",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Dob = table.Column<DateOnly>(type: "date", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    Create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Create_by = table.Column<string>(type: "text", nullable: false),
                    Delete_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Delete_by = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Dob = table.Column<DateOnly>(type: "date", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Parent = table.Column<string>(type: "text", nullable: false),
                    Parent1 = table.Column<string>(type: "text", nullable: false),
                    Update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Update_by = table.Column<string>(type: "text", nullable: false)
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
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    Create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    End_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Researcher = table.Column<string>(type: "text", nullable: false),
                    Start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Referents_StageAttributeID",
                table: "Referents",
                column: "StageAttributeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Characteristic_SeedlingAttribute_SeedlingAttributeID",
                table: "Characteristic",
                column: "SeedlingAttributeID",
                principalTable: "SeedlingAttribute",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characteristic_Seedling_SeedlingID",
                table: "Characteristic",
                column: "SeedlingID",
                principalTable: "Seedling",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementInStage_Element_ElementID",
                table: "ElementInStage",
                column: "ElementID",
                principalTable: "Element",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentLogs_Method_MethodID",
                table: "ExperimentLogs",
                column: "MethodID",
                principalTable: "Method",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hybridization_Seedling_ParentID",
                table: "Hybridization",
                column: "ParentID",
                principalTable: "Seedling",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imgs_Report_ReportID",
                table: "Imgs",
                column: "ReportID",
                principalTable: "Report",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InfectedSamples_Disease_DiseaseID",
                table: "InfectedSamples",
                column: "DiseaseID",
                principalTable: "Disease",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InfectedSamples_Sample_SampleID",
                table: "InfectedSamples",
                column: "SampleID",
                principalTable: "Sample",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Linkeds_Sample_SampleID",
                table: "Linkeds",
                column: "SampleID",
                principalTable: "Sample",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Linkeds_Task_TaskID",
                table: "Linkeds",
                column: "TaskID",
                principalTable: "Task",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Referents_StageAttribute_StageAttributeID",
                table: "Referents",
                column: "StageAttributeID",
                principalTable: "StageAttribute",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAttributes_Report_ReportID",
                table: "ReportAttributes",
                column: "ReportID",
                principalTable: "Report",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAttributes_SeedlingAttribute_ReferentDataID",
                table: "ReportAttributes",
                column: "ReferentDataID",
                principalTable: "SeedlingAttribute",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Method_MethodID",
                table: "Stage",
                column: "MethodID",
                principalTable: "Method",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssigns_Task_TaskID",
                table: "TaskAssigns",
                column: "TaskID",
                principalTable: "Task",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAttributes_Task_TaskID",
                table: "TaskAttributes",
                column: "TaskID",
                principalTable: "Task",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TissueCultureBatches_LabRoom_LabRoomID",
                table: "TissueCultureBatches",
                column: "LabRoomID",
                principalTable: "LabRoom",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

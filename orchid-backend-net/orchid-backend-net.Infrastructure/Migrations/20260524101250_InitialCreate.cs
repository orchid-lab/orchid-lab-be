using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalyticResults",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    PredictionsJson = table.Column<string>(type: "text", nullable: false),
                    TopDisease = table.Column<string>(type: "text", nullable: false),
                    Confidence = table.Column<decimal>(type: "numeric", nullable: false),
                    AnalyzedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AnalyzedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticResults", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Characteristics",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristics", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Chemicals",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ConcentrationUnit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chemicals", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    ConfigName = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OnnxClassName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Imgs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    TargetType = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsNewest = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imgs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LabRooms",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabRooms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Unit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Methods",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Methods", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MethodStageDefinition",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodStageDefinition", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NotificationTargetType = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SafeProcedures",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    ProcedureName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProcedureType = table.Column<string>(type: "text", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafeProcedures", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SamplesRequirementDefinitions",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    CharacteristicCode = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Unit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplesRequirementDefinitions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SampleStageDefinition",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    MinDurationDays = table.Column<int>(type: "integer", nullable: true),
                    MaxDurationDays = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleStageDefinition", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Seedlings",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    LocalName = table.Column<string>(type: "text", nullable: false),
                    ScientificName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ParentAId = table.Column<string>(type: "text", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seedlings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Seedlings_Seedlings_ParentAId",
                        column: x => x.ParentAId,
                        principalTable: "Seedlings",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SkillName = table.Column<string>(type: "text", nullable: false),
                    SkillDescription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StageId = table.Column<int>(type: "integer", nullable: true),
                    ResearcherId = table.Column<string>(type: "text", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LabRoomId = table.Column<int>(type: "integer", nullable: false),
                    BatchName = table.Column<string>(type: "text", nullable: false),
                    BatchSizeWidth = table.Column<decimal>(type: "numeric", nullable: false),
                    BatchSizeHeight = table.Column<decimal>(type: "numeric", nullable: false),
                    WidthUnit = table.Column<string>(type: "text", nullable: false),
                    HeightUnit = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Batches_LabRooms_LabRoomId",
                        column: x => x.LabRoomId,
                        principalTable: "LabRooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodStages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MethodId = table.Column<int>(type: "integer", nullable: false),
                    MethodStageDefinitionId = table.Column<int>(type: "integer", nullable: false),
                    DurationsDays = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsSampleGenerated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodStages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MethodStages_MethodStageDefinition_MethodStageDefinitionId",
                        column: x => x.MethodStageDefinitionId,
                        principalTable: "MethodStageDefinition",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodStages_Methods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Methods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    RoleID = table.Column<int>(type: "integer", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FcmToken = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SafeProcedureStep",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    SafeProcedureId = table.Column<string>(type: "text", nullable: false),
                    SafeProcedureStepName = table.Column<string>(type: "text", nullable: false),
                    StepNumber = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafeProcedureStep", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SafeProcedureStep_SafeProcedures_SafeProcedureId",
                        column: x => x.SafeProcedureId,
                        principalTable: "SafeProcedures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StageRequirementDefinitions",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    SampleStageDefinitionId = table.Column<int>(type: "integer", nullable: false),
                    SampleRequirementDefinitionId = table.Column<string>(type: "text", nullable: false),
                    ExpectedValue = table.Column<decimal>(type: "numeric", nullable: false),
                    MinValue = table.Column<decimal>(type: "numeric", nullable: true),
                    MaxValue = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageRequirementDefinitions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StageRequirementDefinitions_SampleStageDefinition_SampleSta~",
                        column: x => x.SampleStageDefinitionId,
                        principalTable: "SampleStageDefinition",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StageRequirementDefinitions_SamplesRequirementDefinitions_S~",
                        column: x => x.SampleRequirementDefinitionId,
                        principalTable: "SamplesRequirementDefinitions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeedlingsTraits",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    SeedlingId = table.Column<string>(type: "text", nullable: false),
                    CharacteristicId = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeedlingsTraits", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SeedlingsTraits_Characteristics_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalTable: "Characteristics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeedlingsTraits_Seedlings_SeedlingId",
                        column: x => x.SeedlingId,
                        principalTable: "Seedlings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskAttributes",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    ChemicalId = table.Column<int>(type: "integer", nullable: true),
                    MaterialId = table.Column<int>(type: "integer", nullable: true),
                    TaskId = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAttributes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskAttributes_Chemicals_ChemicalId",
                        column: x => x.ChemicalId,
                        principalTable: "Chemicals",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TaskAttributes_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TaskAttributes_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskChecks",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    TaskId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskChecks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskChecks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentLogs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    SeedlingParentId = table.Column<string>(type: "text", nullable: false),
                    MethodId = table.Column<int>(type: "integer", nullable: false),
                    BatchId = table.Column<int>(type: "integer", nullable: false),
                    ExpectedSampleCount = table.Column<int>(type: "integer", nullable: false),
                    CurrentStageOrder = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AssignedTo = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpectedEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Objective = table.Column<string>(type: "text", nullable: true),
                    Conclusion = table.Column<string>(type: "text", nullable: true),
                    Issues = table.Column<string>(type: "text", nullable: true),
                    Recommendations = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentLogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExperimentLogs_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentLogs_Methods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Methods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentLogs_Seedlings_SeedlingParentId",
                        column: x => x.SeedlingParentId,
                        principalTable: "Seedlings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StageChemicals",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    ChemicalId = table.Column<int>(type: "integer", nullable: false),
                    StageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageChemicals", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StageChemicals_Chemicals_ChemicalId",
                        column: x => x.ChemicalId,
                        principalTable: "Chemicals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StageChemicals_MethodStages_StageId",
                        column: x => x.StageId,
                        principalTable: "MethodStages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StageMaterials",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    MaterialId = table.Column<int>(type: "integer", nullable: false),
                    StageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageMaterials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StageMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StageMaterials_MethodStages_StageId",
                        column: x => x.StageId,
                        principalTable: "MethodStages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskAssignments",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    TaskId = table.Column<string>(type: "text", nullable: false),
                    TechnicianId = table.Column<string>(type: "text", nullable: false),
                    TargetType = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpectedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAssignments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskAssignments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAssignments_Users_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSkill",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    SkillId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkill", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkill_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCheckListItems",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    TaskCheckListId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ExpectedUnit = table.Column<string>(type: "text", nullable: true),
                    ExpectedMinValue = table.Column<decimal>(type: "numeric", nullable: true),
                    ExpectedMaxValue = table.Column<decimal>(type: "numeric", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    MeasurementUnit = table.Column<string>(type: "text", nullable: true),
                    MesuredValue = table.Column<decimal>(type: "numeric", nullable: true),
                    IsPass = table.Column<bool>(type: "boolean", nullable: true),
                    Evaluated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCheckListItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskCheckListItems_TaskChecks_TaskCheckListId",
                        column: x => x.TaskCheckListId,
                        principalTable: "TaskChecks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ExperimentLogId = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    ExecutionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    InitialCondition = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Samples_ExperimentLogs_ExperimentLogId",
                        column: x => x.ExperimentLogId,
                        principalTable: "ExperimentLogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SampleStages",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    SampleId = table.Column<string>(type: "text", nullable: false),
                    SampleStageDefinitionId = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    CompletedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleStages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SampleStages_SampleStageDefinition_SampleStageDefinitionId",
                        column: x => x.SampleStageDefinitionId,
                        principalTable: "SampleStageDefinition",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SampleStages_Samples_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Samples",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitoringLogs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    AnalyticResultId = table.Column<string>(type: "text", nullable: true),
                    SampleStageId = table.Column<string>(type: "text", nullable: false),
                    DiseaseId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    IsNewest = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringLogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MonitoringLogs_AnalyticResults_AnalyticResultId",
                        column: x => x.AnalyticResultId,
                        principalTable: "AnalyticResults",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MonitoringLogs_Diseases_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Diseases",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MonitoringLogs_SampleStages_SampleStageId",
                        column: x => x.SampleStageId,
                        principalTable: "SampleStages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonitoringLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiseaseIncidents",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    SampleStageId = table.Column<string>(type: "text", nullable: false),
                    MonitoringLogId = table.Column<string>(type: "text", nullable: true),
                    DiseaseId = table.Column<int>(type: "integer", nullable: false),
                    AIConfidence = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReviewNote = table.Column<string>(type: "text", nullable: true),
                    ReviewedBy = table.Column<string>(type: "text", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseaseIncidents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DiseaseIncidents_Diseases_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Diseases",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiseaseIncidents_MonitoringLogs_MonitoringLogId",
                        column: x => x.MonitoringLogId,
                        principalTable: "MonitoringLogs",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DiseaseIncidents_SampleStages_SampleStageId",
                        column: x => x.SampleStageId,
                        principalTable: "SampleStages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitoringLogDetails",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    MonitoringLogsId = table.Column<string>(type: "text", nullable: false),
                    StageRequirementDefinitionId = table.Column<string>(type: "text", nullable: false),
                    MeasuredValue = table.Column<decimal>(type: "numeric", nullable: false),
                    IsMatch = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringLogDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MonitoringLogDetails_MonitoringLogs_MonitoringLogsId",
                        column: x => x.MonitoringLogsId,
                        principalTable: "MonitoringLogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonitoringLogDetails_StageRequirementDefinitions_StageRequi~",
                        column: x => x.StageRequirementDefinitionId,
                        principalTable: "StageRequirementDefinitions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_LabRoomId",
                table: "Batches",
                column: "LabRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseIncidents_DiseaseId",
                table: "DiseaseIncidents",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseIncidents_MonitoringLogId",
                table: "DiseaseIncidents",
                column: "MonitoringLogId");

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseIncidents_SampleStageId",
                table: "DiseaseIncidents",
                column: "SampleStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentLogs_BatchId",
                table: "ExperimentLogs",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentLogs_MethodId",
                table: "ExperimentLogs",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentLogs_SeedlingParentId",
                table: "ExperimentLogs",
                column: "SeedlingParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodStages_MethodId",
                table: "MethodStages",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodStages_MethodStageDefinitionId",
                table: "MethodStages",
                column: "MethodStageDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringLogDetails_MonitoringLogsId",
                table: "MonitoringLogDetails",
                column: "MonitoringLogsId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringLogDetails_StageRequirementDefinitionId",
                table: "MonitoringLogDetails",
                column: "StageRequirementDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringLogs_AnalyticResultId",
                table: "MonitoringLogs",
                column: "AnalyticResultId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringLogs_DiseaseId",
                table: "MonitoringLogs",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringLogs_SampleStageId",
                table: "MonitoringLogs",
                column: "SampleStageId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringLogs_UserId",
                table: "MonitoringLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SafeProcedureStep_SafeProcedureId",
                table: "SafeProcedureStep",
                column: "SafeProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Samples_ExperimentLogId",
                table: "Samples",
                column: "ExperimentLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleStages_SampleId",
                table: "SampleStages",
                column: "SampleId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleStages_SampleStageDefinitionId",
                table: "SampleStages",
                column: "SampleStageDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Seedlings_ParentAId",
                table: "Seedlings",
                column: "ParentAId");

            migrationBuilder.CreateIndex(
                name: "IX_SeedlingsTraits_CharacteristicId",
                table: "SeedlingsTraits",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_SeedlingsTraits_SeedlingId",
                table: "SeedlingsTraits",
                column: "SeedlingId");

            migrationBuilder.CreateIndex(
                name: "IX_StageChemicals_ChemicalId",
                table: "StageChemicals",
                column: "ChemicalId");

            migrationBuilder.CreateIndex(
                name: "IX_StageChemicals_StageId",
                table: "StageChemicals",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_StageMaterials_MaterialId",
                table: "StageMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_StageMaterials_StageId",
                table: "StageMaterials",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_StageRequirementDefinitions_SampleRequirementDefinitionId",
                table: "StageRequirementDefinitions",
                column: "SampleRequirementDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_StageRequirementDefinitions_SampleStageDefinitionId",
                table: "StageRequirementDefinitions",
                column: "SampleStageDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_TaskId",
                table: "TaskAssignments",
                column: "TaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_TechnicianId",
                table: "TaskAssignments",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttributes_ChemicalId",
                table: "TaskAttributes",
                column: "ChemicalId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttributes_MaterialId",
                table: "TaskAttributes",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttributes_TaskId",
                table: "TaskAttributes",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCheckListItems_TaskCheckListId",
                table: "TaskCheckListItems",
                column: "TaskCheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskChecks_TaskId",
                table: "TaskChecks",
                column: "TaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_SkillId",
                table: "UserSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserId",
                table: "UserSkill",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "DiseaseIncidents");

            migrationBuilder.DropTable(
                name: "Imgs");

            migrationBuilder.DropTable(
                name: "MonitoringLogDetails");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SafeProcedureStep");

            migrationBuilder.DropTable(
                name: "SeedlingsTraits");

            migrationBuilder.DropTable(
                name: "StageChemicals");

            migrationBuilder.DropTable(
                name: "StageMaterials");

            migrationBuilder.DropTable(
                name: "TaskAssignments");

            migrationBuilder.DropTable(
                name: "TaskAttributes");

            migrationBuilder.DropTable(
                name: "TaskCheckListItems");

            migrationBuilder.DropTable(
                name: "UserSkill");

            migrationBuilder.DropTable(
                name: "MonitoringLogs");

            migrationBuilder.DropTable(
                name: "StageRequirementDefinitions");

            migrationBuilder.DropTable(
                name: "SafeProcedures");

            migrationBuilder.DropTable(
                name: "Characteristics");

            migrationBuilder.DropTable(
                name: "MethodStages");

            migrationBuilder.DropTable(
                name: "Chemicals");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "TaskChecks");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "AnalyticResults");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "SampleStages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SamplesRequirementDefinitions");

            migrationBuilder.DropTable(
                name: "MethodStageDefinition");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "SampleStageDefinition");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ExperimentLogs");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropTable(
                name: "Methods");

            migrationBuilder.DropTable(
                name: "Seedlings");

            migrationBuilder.DropTable(
                name: "LabRooms");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalyticResults",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Anthracnose = table.Column<decimal>(type: "numeric", nullable: false),
                    BacterialWilt = table.Column<decimal>(type: "numeric", nullable: false),
                    Blackrot = table.Column<decimal>(type: "numeric", nullable: false),
                    Brownspots = table.Column<decimal>(type: "numeric", nullable: false),
                    MoldBacterial = table.Column<decimal>(type: "numeric", nullable: false),
                    MoldFungus = table.Column<decimal>(type: "numeric", nullable: false),
                    SoftRot = table.Column<decimal>(type: "numeric", nullable: false),
                    StemRot = table.Column<decimal>(type: "numeric", nullable: false),
                    WitheredYellowRoot = table.Column<decimal>(type: "numeric", nullable: false),
                    Healthy = table.Column<decimal>(type: "numeric", nullable: false),
                    Oxidation = table.Column<decimal>(type: "numeric", nullable: false),
                    Virus = table.Column<decimal>(type: "numeric", nullable: false)
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
                    ID = table.Column<int>(type: "integer", nullable: false),
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
                name: "Disease",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disease", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LabRooms",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabRooms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
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
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Methods", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
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
                    ParentBId = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_Seedlings_Seedlings_ParentBId",
                        column: x => x.ParentBId,
                        principalTable: "Seedlings",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "StageDefinition",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageDefinition", x => x.ID);
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
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    LabRoomId = table.Column<int>(type: "integer", nullable: false)
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
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "Hybridzations",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    ParentAId = table.Column<string>(type: "text", nullable: false),
                    ParentBId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hybridzations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Hybridzations_Seedlings_ParentAId",
                        column: x => x.ParentAId,
                        principalTable: "Seedlings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hybridzations_Seedlings_ParentBId",
                        column: x => x.ParentBId,
                        principalTable: "Seedlings",
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
                name: "Stages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    MethodId = table.Column<int>(type: "integer", nullable: false),
                    StageDefinitionId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DurationsDays = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stages_Methods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Methods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stages_StageDefinition_StageDefinitionId",
                        column: x => x.StageDefinitionId,
                        principalTable: "StageDefinition",
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
                name: "ExperimentLogs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    HybridzationId = table.Column<string>(type: "text", nullable: false),
                    HybridzationsID = table.Column<string>(type: "text", nullable: true),
                    MethodId = table.Column<int>(type: "integer", nullable: false),
                    BatchId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
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
                        name: "FK_ExperimentLogs_Hybridzations_HybridzationsID",
                        column: x => x.HybridzationsID,
                        principalTable: "Hybridzations",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ExperimentLogs_Methods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Methods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SamplesRequirements",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    StageId = table.Column<int>(type: "integer", nullable: false),
                    CharacteristicCode = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MinValue = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxValue = table.Column<decimal>(type: "numeric", nullable: false),
                    ExpectedValue = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplesRequirements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SamplesRequirements_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
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
                        name: "FK_StageChemicals_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
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
                        name: "FK_StageMaterials_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
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
                    CurrentStageOrder = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    ExecutionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
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
                name: "MonitoringLogs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    AnalyticResultId = table.Column<string>(type: "text", nullable: false),
                    SampleId = table.Column<string>(type: "text", nullable: false),
                    DiseaseId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    SampleStageOrder = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringLogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MonitoringLogs_AnalyticResults_AnalyticResultId",
                        column: x => x.AnalyticResultId,
                        principalTable: "AnalyticResults",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonitoringLogs_Disease_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Disease",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MonitoringLogs_Samples_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Samples",
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
                name: "Imgs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    MonitoringLogsId = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imgs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Imgs_MonitoringLogs_MonitoringLogsId",
                        column: x => x.MonitoringLogsId,
                        principalTable: "MonitoringLogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitoringLogDetails",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    RequirementId = table.Column<string>(type: "text", nullable: false),
                    MonitoringLogsId = table.Column<string>(type: "text", nullable: false),
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
                        name: "FK_MonitoringLogDetails_SamplesRequirements_RequirementId",
                        column: x => x.RequirementId,
                        principalTable: "SamplesRequirements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_LabRoomId",
                table: "Batches",
                column: "LabRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentLogs_BatchId",
                table: "ExperimentLogs",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentLogs_HybridzationsID",
                table: "ExperimentLogs",
                column: "HybridzationsID");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentLogs_MethodId",
                table: "ExperimentLogs",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Hybridzations_ParentAId",
                table: "Hybridzations",
                column: "ParentAId");

            migrationBuilder.CreateIndex(
                name: "IX_Hybridzations_ParentBId",
                table: "Hybridzations",
                column: "ParentBId");

            migrationBuilder.CreateIndex(
                name: "IX_Imgs_MonitoringLogsId",
                table: "Imgs",
                column: "MonitoringLogsId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringLogDetails_MonitoringLogsId",
                table: "MonitoringLogDetails",
                column: "MonitoringLogsId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringLogDetails_RequirementId",
                table: "MonitoringLogDetails",
                column: "RequirementId");

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
                name: "IX_MonitoringLogs_SampleId",
                table: "MonitoringLogs",
                column: "SampleId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringLogs_UserId",
                table: "MonitoringLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Samples_ExperimentLogId",
                table: "Samples",
                column: "ExperimentLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplesRequirements_StageId",
                table: "SamplesRequirements",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Seedlings_ParentAId",
                table: "Seedlings",
                column: "ParentAId");

            migrationBuilder.CreateIndex(
                name: "IX_Seedlings_ParentBId",
                table: "Seedlings",
                column: "ParentBId");

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
                name: "IX_Stages_MethodId",
                table: "Stages",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_StageDefinitionId",
                table: "Stages",
                column: "StageDefinitionId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imgs");

            migrationBuilder.DropTable(
                name: "MonitoringLogDetails");

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
                name: "MonitoringLogs");

            migrationBuilder.DropTable(
                name: "SamplesRequirements");

            migrationBuilder.DropTable(
                name: "Characteristics");

            migrationBuilder.DropTable(
                name: "Chemicals");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "AnalyticResults");

            migrationBuilder.DropTable(
                name: "Disease");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "ExperimentLogs");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "StageDefinition");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropTable(
                name: "Hybridzations");

            migrationBuilder.DropTable(
                name: "Methods");

            migrationBuilder.DropTable(
                name: "LabRooms");

            migrationBuilder.DropTable(
                name: "Seedlings");
        }
    }
}

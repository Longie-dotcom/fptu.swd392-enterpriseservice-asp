using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    AuditLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditLogId);
                });

            migrationBuilder.CreateTable(
                name: "Enterprises",
                columns: table => new
                {
                    EnterpriseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TIN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AvatarName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enterprises", x => x.EnterpriseID);
                });

            migrationBuilder.CreateTable(
                name: "WasteTypes",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteTypes", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "Capacities",
                columns: table => new
                {
                    CapacityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaxDailyCapacity = table.Column<double>(type: "float", nullable: false),
                    RegionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitOfMeasure = table.Column<int>(type: "int", nullable: false),
                    CurrentLoad = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WasteType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EnterpriseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capacities", x => x.CapacityID);
                    table.ForeignKey(
                        name: "FK_Capacities_Enterprises_EnterpriseID",
                        column: x => x.EnterpriseID,
                        principalTable: "Enterprises",
                        principalColumn: "EnterpriseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnassignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnterpriseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_Members_Enterprises_EnterpriseID",
                        column: x => x.EnterpriseID,
                        principalTable: "Enterprises",
                        principalColumn: "EnterpriseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RewardPolicies",
                columns: table => new
                {
                    RewardPolicyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasePoint = table.Column<int>(type: "int", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnterpriseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardPolicies", x => x.RewardPolicyID);
                    table.ForeignKey(
                        name: "FK_RewardPolicies_Enterprises_EnterpriseID",
                        column: x => x.EnterpriseID,
                        principalTable: "Enterprises",
                        principalColumn: "EnterpriseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionAssignments",
                columns: table => new
                {
                    CollectionAssignmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionReportID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssigneeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriorityLevel = table.Column<int>(type: "int", nullable: false),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CapacityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionAssignments", x => x.CollectionAssignmentID);
                    table.ForeignKey(
                        name: "FK_CollectionAssignments_Capacities_CapacityID",
                        column: x => x.CapacityID,
                        principalTable: "Capacities",
                        principalColumn: "CapacityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonusRules",
                columns: table => new
                {
                    PenaltyRuleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BonusPoint = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RewardPolicyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusRules", x => x.PenaltyRuleID);
                    table.ForeignKey(
                        name: "FK_BonusRules_RewardPolicies_RewardPolicyID",
                        column: x => x.RewardPolicyID,
                        principalTable: "RewardPolicies",
                        principalColumn: "RewardPolicyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PenaltyRules",
                columns: table => new
                {
                    PenaltyRuleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PenaltyPoint = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RewardPolicyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyRules", x => x.PenaltyRuleID);
                    table.ForeignKey(
                        name: "FK_PenaltyRules_RewardPolicies_RewardPolicyID",
                        column: x => x.RewardPolicyID,
                        principalTable: "RewardPolicies",
                        principalColumn: "RewardPolicyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BonusRules_RewardPolicyID",
                table: "BonusRules",
                column: "RewardPolicyID");

            migrationBuilder.CreateIndex(
                name: "IX_Capacities_EnterpriseID_WasteType",
                table: "Capacities",
                columns: new[] { "EnterpriseID", "WasteType" });

            migrationBuilder.CreateIndex(
                name: "IX_CollectionAssignments_CapacityID",
                table: "CollectionAssignments",
                column: "CapacityID");

            migrationBuilder.CreateIndex(
                name: "IX_Enterprises_TIN",
                table: "Enterprises",
                column: "TIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_EnterpriseID",
                table: "Members",
                column: "EnterpriseID");

            migrationBuilder.CreateIndex(
                name: "IX_PenaltyRules_RewardPolicyID",
                table: "PenaltyRules",
                column: "RewardPolicyID");

            migrationBuilder.CreateIndex(
                name: "IX_RewardPolicies_EnterpriseID",
                table: "RewardPolicies",
                column: "EnterpriseID");

            migrationBuilder.CreateIndex(
                name: "IX_WasteTypes_Type",
                table: "WasteTypes",
                column: "Type",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BonusRules");

            migrationBuilder.DropTable(
                name: "CollectionAssignments");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "PenaltyRules");

            migrationBuilder.DropTable(
                name: "WasteTypes");

            migrationBuilder.DropTable(
                name: "Capacities");

            migrationBuilder.DropTable(
                name: "RewardPolicies");

            migrationBuilder.DropTable(
                name: "Enterprises");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkForceManagementV0.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsPhone = table.Column<bool>(type: "bit", nullable: false),
                    IsAbsence = table.Column<bool>(type: "bit", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsWorkTime = table.Column<bool>(type: "bit", nullable: false),
                    IsBreak = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DisableEdit = table.Column<bool>(type: "bit", nullable: false),
                    IsUndefined = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NeedBackup = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DurationTolerance = table.Column<double>(type: "float", nullable: false),
                    OfferedTolerance = table.Column<double>(type: "float", nullable: false),
                    ServiceLevel = table.Column<double>(type: "float", nullable: false),
                    ServiceTime = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExceptDates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSaved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forecasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeadOfSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadOfSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Intervals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeMap = table.Column<TimeSpan>(type: "time", nullable: false),
                    OrderMap = table.Column<int>(type: "int", nullable: false),
                    CoverMap = table.Column<int>(type: "int", nullable: false),
                    Tolerance = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IpccAgents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffMemberEmployeeId = table.Column<int>(type: "int", nullable: false),
                    StaffMemberName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtcDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpccAgents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    ForecastId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shiftRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartAfter = table.Column<int>(type: "int", nullable: false),
                    EndBefore = table.Column<int>(type: "int", nullable: false),
                    BreakBetween = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shiftRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaffTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SwapRequestStatuses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapRequestStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsAbsence = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DisableEdit = table.Column<bool>(type: "bit", nullable: false),
                    Hidden = table.Column<bool>(type: "bit", nullable: false),
                    DefaultActivityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceTypes_Activities_DefaultActivityId",
                        column: x => x.DefaultActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetTermValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<double>(type: "float", nullable: false),
                    IsOptional = table.Column<bool>(type: "bit", nullable: false),
                    AssetTypeId = table.Column<int>(type: "int", nullable: false),
                    AssetTermId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTermValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetTermValues_AssetTerms_AssetTermId",
                        column: x => x.AssetTermId,
                        principalTable: "AssetTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetTermValues_AssetTypes_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "AssetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForecastDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForecastId = table.Column<int>(type: "int", nullable: false),
                    IntervalId = table.Column<int>(type: "int", nullable: false),
                    DayoffWeek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForecastDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForecastDetails_Forecasts_ForecastId",
                        column: x => x.ForecastId,
                        principalTable: "Forecasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForecastDetails_Intervals_IntervalId",
                        column: x => x.IntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForecastHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Offered = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    IntervalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForecastHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForecastHistories_Intervals_IntervalId",
                        column: x => x.IntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EarlyStartIntervalId = table.Column<int>(type: "int", nullable: true),
                    LateEndIntervalId = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_Intervals_EarlyStartIntervalId",
                        column: x => x.EarlyStartIntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shifts_Intervals_LateEndIntervalId",
                        column: x => x.LateEndIntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TmpForecastDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TmpForecastId = table.Column<int>(type: "int", nullable: false),
                    IntervalId = table.Column<int>(type: "int", nullable: false),
                    DayoffWeek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TmpForecastDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TmpForecastDetails_Intervals_IntervalId",
                        column: x => x.IntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Specs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    AssetTypeId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_AssetTypes_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "AssetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assets_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubLocation_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForeCastings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    IntervalId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForeCastings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForeCastings_Intervals_IntervalId",
                        column: x => x.IntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForeCastings_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BkpDailyAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyAttendanceId = table.Column<int>(type: "int", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StaffMemberId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceTypeId = table.Column<int>(type: "int", nullable: false),
                    TransportationRouteId = table.Column<int>(type: "int", nullable: true),
                    HeadOfSectionId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BkpDailyAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BkpDailyAttendances_AttendanceTypes_AttendanceTypeId",
                        column: x => x.AttendanceTypeId,
                        principalTable: "AttendanceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportationRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubLocationId = table.Column<int>(type: "int", nullable: true),
                    ArriveIntervalId = table.Column<int>(type: "int", nullable: false),
                    DepartIntervalId = table.Column<int>(type: "int", nullable: true),
                    IsIgnored = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportationRoutes_Intervals_ArriveIntervalId",
                        column: x => x.ArriveIntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransportationRoutes_Intervals_DepartIntervalId",
                        column: x => x.DepartIntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationRoutes_SubLocation_SubLocationId",
                        column: x => x.SubLocationId,
                        principalTable: "SubLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BkpScheduleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyAttendanceId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    IntervalId = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: true),
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BkpScheduleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BkpScheduleDetails_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BkpScheduleDetails_BkpDailyAttendances_DailyAttendanceId",
                        column: x => x.DailyAttendanceId,
                        principalTable: "BkpDailyAttendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BkpScheduleDetails_Intervals_IntervalId",
                        column: x => x.IntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedLeaveDays = table.Column<int>(type: "int", nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffTypeId = table.Column<int>(type: "int", nullable: false),
                    TransportationRouteId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    HeadOfSectionId = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffMembers_HeadOfSections_HeadOfSectionId",
                        column: x => x.HeadOfSectionId,
                        principalTable: "HeadOfSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffMembers_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffMembers_StaffTypes_StaffTypeId",
                        column: x => x.StaffTypeId,
                        principalTable: "StaffTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffMembers_TransportationRoutes_TransportationRouteId",
                        column: x => x.TransportationRouteId,
                        principalTable: "TransportationRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "breakTypeOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffMemberId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    AttendenceTypeId = table.Column<int>(type: "int", nullable: false),
                    TransportationRouteId = table.Column<int>(type: "int", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_breakTypeOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_breakTypeOptions_AttendanceTypes_AttendenceTypeId",
                        column: x => x.AttendenceTypeId,
                        principalTable: "AttendanceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_breakTypeOptions_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_breakTypeOptions_StaffMembers_StaffMemberId",
                        column: x => x.StaffMemberId,
                        principalTable: "StaffMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_breakTypeOptions_TransportationRoutes_TransportationRouteId",
                        column: x => x.TransportationRouteId,
                        principalTable: "TransportationRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DailyAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffMemberId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceTypeId = table.Column<int>(type: "int", nullable: false),
                    TransportationRouteId = table.Column<int>(type: "int", nullable: true),
                    HeadOfSectionId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    HaveBackup = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyAttendances_AttendanceTypes_AttendanceTypeId",
                        column: x => x.AttendanceTypeId,
                        principalTable: "AttendanceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyAttendances_HeadOfSections_HeadOfSectionId",
                        column: x => x.HeadOfSectionId,
                        principalTable: "HeadOfSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyAttendances_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyAttendances_StaffMembers_StaffMemberId",
                        column: x => x.StaffMemberId,
                        principalTable: "StaffMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DailyAttendances_TransportationRoutes_TransportationRouteId",
                        column: x => x.TransportationRouteId,
                        principalTable: "TransportationRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dayOffOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffMemberId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    DayOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dayOffOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dayOffOptions_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dayOffOptions_StaffMembers_StaffMemberId",
                        column: x => x.StaffMemberId,
                        principalTable: "StaffMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyAttendanceId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    IntervalId = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: true),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    BackupStaffId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleDetail_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleDetail_DailyAttendances_DailyAttendanceId",
                        column: x => x.DailyAttendanceId,
                        principalTable: "DailyAttendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleDetail_Intervals_IntervalId",
                        column: x => x.IntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleDetail_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ScheduleDetail_StaffMembers_BackupStaffId",
                        column: x => x.BackupStaffId,
                        principalTable: "StaffMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SwapRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: true),
                    SourceDailyAttendanceId = table.Column<int>(type: "int", nullable: true),
                    DestinationDailyAttendanceId = table.Column<int>(type: "int", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequesterAlias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequesterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponderName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SwapRequests_DailyAttendances_DestinationDailyAttendanceId",
                        column: x => x.DestinationDailyAttendanceId,
                        principalTable: "DailyAttendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SwapRequests_DailyAttendances_SourceDailyAttendanceId",
                        column: x => x.SourceDailyAttendanceId,
                        principalTable: "DailyAttendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SwapRequests_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SwapRequests_SwapRequestStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "SwapRequestStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TmpScheduleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyAttendanceId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    IntervalId = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TmpScheduleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TmpScheduleDetails_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TmpScheduleDetails_DailyAttendances_DailyAttendanceId",
                        column: x => x.DailyAttendanceId,
                        principalTable: "DailyAttendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TmpScheduleDetails_Intervals_IntervalId",
                        column: x => x.IntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SwapRequestDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SwapRequestId = table.Column<int>(type: "int", nullable: false),
                    InvolvedAlias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapRequestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SwapRequestDetails_SwapRequests_SwapRequestId",
                        column: x => x.SwapRequestId,
                        principalTable: "SwapRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Color",
                table: "Activities",
                column: "Color",
                unique: true,
                filter: "[Color] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Name",
                table: "Activities",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetTypeId",
                table: "Assets",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Barcode",
                table: "Assets",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_LocationId",
                table: "Assets",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTerms_Name",
                table: "AssetTerms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetTermValues_AssetTermId",
                table: "AssetTermValues",
                column: "AssetTermId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTermValues_AssetTypeId",
                table: "AssetTermValues",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypes_Name",
                table: "AssetTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceTypes_DefaultActivityId",
                table: "AttendanceTypes",
                column: "DefaultActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceTypes_Name",
                table: "AttendanceTypes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BkpDailyAttendances_AttendanceTypeId",
                table: "BkpDailyAttendances",
                column: "AttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BkpScheduleDetails_ActivityId",
                table: "BkpScheduleDetails",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_BkpScheduleDetails_DailyAttendanceId",
                table: "BkpScheduleDetails",
                column: "DailyAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_BkpScheduleDetails_IntervalId",
                table: "BkpScheduleDetails",
                column: "IntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_breakTypeOptions_AttendenceTypeId",
                table: "breakTypeOptions",
                column: "AttendenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_breakTypeOptions_ScheduleId",
                table: "breakTypeOptions",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_breakTypeOptions_StaffMemberId",
                table: "breakTypeOptions",
                column: "StaffMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_breakTypeOptions_TransportationRouteId",
                table: "breakTypeOptions",
                column: "TransportationRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_colors_ColorName",
                table: "colors",
                column: "ColorName",
                unique: true,
                filter: "[ColorName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendances_AttendanceTypeId",
                table: "DailyAttendances",
                column: "AttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendances_HeadOfSectionId",
                table: "DailyAttendances",
                column: "HeadOfSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendances_ScheduleId",
                table: "DailyAttendances",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendances_StaffMemberId_ScheduleId_AttendanceTypeId_TransportationRouteId_Day",
                table: "DailyAttendances",
                columns: new[] { "StaffMemberId", "ScheduleId", "AttendanceTypeId", "TransportationRouteId", "Day" },
                unique: true,
                filter: "[TransportationRouteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendances_TransportationRouteId",
                table: "DailyAttendances",
                column: "TransportationRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_dayOffOptions_ScheduleId",
                table: "dayOffOptions",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_dayOffOptions_StaffMemberId",
                table: "dayOffOptions",
                column: "StaffMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ForecastDetails_ForecastId",
                table: "ForecastDetails",
                column: "ForecastId");

            migrationBuilder.CreateIndex(
                name: "IX_ForecastDetails_IntervalId",
                table: "ForecastDetails",
                column: "IntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_ForecastHistories_IntervalId",
                table: "ForecastHistories",
                column: "IntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_ForeCastings_IntervalId",
                table: "ForeCastings",
                column: "IntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_ForeCastings_ScheduleId",
                table: "ForeCastings",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_Name",
                table: "Forecasts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeadOfSections_Alias",
                table: "HeadOfSections",
                column: "Alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeadOfSections_EmployeeId",
                table: "HeadOfSections",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                table: "Locations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDetail_ActivityId",
                table: "ScheduleDetail",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDetail_BackupStaffId",
                table: "ScheduleDetail",
                column: "BackupStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDetail_DailyAttendanceId",
                table: "ScheduleDetail",
                column: "DailyAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDetail_IntervalId",
                table: "ScheduleDetail",
                column: "IntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDetail_ScheduleId",
                table: "ScheduleDetail",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_Name",
                table: "Schedules",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_EarlyStartIntervalId",
                table: "Shifts",
                column: "EarlyStartIntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_LateEndIntervalId",
                table: "Shifts",
                column: "LateEndIntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Name",
                table: "Shifts",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_Alias",
                table: "StaffMembers",
                column: "Alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_Email",
                table: "StaffMembers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_EmployeeId",
                table: "StaffMembers",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_HeadOfSectionId",
                table: "StaffMembers",
                column: "HeadOfSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_LocationId",
                table: "StaffMembers",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_Name",
                table: "StaffMembers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_StaffTypeId",
                table: "StaffMembers",
                column: "StaffTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_TransportationRouteId",
                table: "StaffMembers",
                column: "TransportationRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTypes_Name",
                table: "StaffTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubLocation_LocationId",
                table: "SubLocation",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequestDetails_SwapRequestId",
                table: "SwapRequestDetails",
                column: "SwapRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_DestinationDailyAttendanceId",
                table: "SwapRequests",
                column: "DestinationDailyAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_ScheduleId",
                table: "SwapRequests",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_SourceDailyAttendanceId",
                table: "SwapRequests",
                column: "SourceDailyAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_StatusId",
                table: "SwapRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TmpForecastDetails_IntervalId",
                table: "TmpForecastDetails",
                column: "IntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_TmpScheduleDetails_ActivityId",
                table: "TmpScheduleDetails",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_TmpScheduleDetails_DailyAttendanceId",
                table: "TmpScheduleDetails",
                column: "DailyAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_TmpScheduleDetails_IntervalId",
                table: "TmpScheduleDetails",
                column: "IntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationRoutes_ArriveIntervalId",
                table: "TransportationRoutes",
                column: "ArriveIntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationRoutes_DepartIntervalId",
                table: "TransportationRoutes",
                column: "DepartIntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationRoutes_Name",
                table: "TransportationRoutes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportationRoutes_SubLocationId",
                table: "TransportationRoutes",
                column: "SubLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "AssetTermValues");

            migrationBuilder.DropTable(
                name: "BkpScheduleDetails");

            migrationBuilder.DropTable(
                name: "breakTypeOptions");

            migrationBuilder.DropTable(
                name: "colors");

            migrationBuilder.DropTable(
                name: "dayOffOptions");

            migrationBuilder.DropTable(
                name: "ForecastDetails");

            migrationBuilder.DropTable(
                name: "ForecastHistories");

            migrationBuilder.DropTable(
                name: "ForeCastings");

            migrationBuilder.DropTable(
                name: "IpccAgents");

            migrationBuilder.DropTable(
                name: "ScheduleDetail");

            migrationBuilder.DropTable(
                name: "shiftRules");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "SwapRequestDetails");

            migrationBuilder.DropTable(
                name: "TmpForecastDetails");

            migrationBuilder.DropTable(
                name: "TmpScheduleDetails");

            migrationBuilder.DropTable(
                name: "AssetTerms");

            migrationBuilder.DropTable(
                name: "AssetTypes");

            migrationBuilder.DropTable(
                name: "BkpDailyAttendances");

            migrationBuilder.DropTable(
                name: "Forecasts");

            migrationBuilder.DropTable(
                name: "SwapRequests");

            migrationBuilder.DropTable(
                name: "DailyAttendances");

            migrationBuilder.DropTable(
                name: "SwapRequestStatuses");

            migrationBuilder.DropTable(
                name: "AttendanceTypes");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "StaffMembers");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "HeadOfSections");

            migrationBuilder.DropTable(
                name: "StaffTypes");

            migrationBuilder.DropTable(
                name: "TransportationRoutes");

            migrationBuilder.DropTable(
                name: "Intervals");

            migrationBuilder.DropTable(
                name: "SubLocation");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}

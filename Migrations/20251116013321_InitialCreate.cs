using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RIMS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rimsDocuments",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Requirements = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ProcessingDays = table.Column<int>(type: "int", nullable: false),
                    ValidityDays = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsDocuments", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "rimsResidents",
                columns: table => new
                {
                    ResidentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    PlaceOfBirth = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CivilStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsHeadOfHousehold = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfDependents = table.Column<int>(type: "int", nullable: true),
                    HeadOfHouseholdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsResidents", x => x.ResidentId);
                    table.ForeignKey(
                        name: "FK_rimsResidents_rimsResidents_HeadOfHouseholdId",
                        column: x => x.HeadOfHouseholdId,
                        principalTable: "rimsResidents",
                        principalColumn: "ResidentId");
                });

            migrationBuilder.CreateTable(
                name: "rimsRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rimsStreets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Highway = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Oneway = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StreetId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Geometry = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsStreets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rimsUserActions",
                columns: table => new
                {
                    ActionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsUserActions", x => x.ActionID);
                });

            migrationBuilder.CreateTable(
                name: "rimsUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "rimsAssistance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResidentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsAssistance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rimsAssistance_rimsResidents_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "rimsResidents",
                        principalColumn: "ResidentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rimsHouseholdMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResidentId = table.Column<int>(type: "int", nullable: false),
                    HeadOfHouseholdId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Relationship = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsHeadOfHousehold = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfDependents = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsHouseholdMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rimsHouseholdMembers_HeadOfHousehold",
                        column: x => x.HeadOfHouseholdId,
                        principalTable: "rimsResidents",
                        principalColumn: "ResidentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rimsHouseholdMembers_Resident",
                        column: x => x.ResidentId,
                        principalTable: "rimsResidents",
                        principalColumn: "ResidentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rimsResidentCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsInformalSettler = table.Column<bool>(type: "bit", nullable: false),
                    IsPWD = table.Column<bool>(type: "bit", nullable: false),
                    Is4PsMember = table.Column<bool>(type: "bit", nullable: false),
                    IsSeniorCitizen = table.Column<bool>(type: "bit", nullable: false),
                    IsSoloParent = table.Column<bool>(type: "bit", nullable: false),
                    IsFAR = table.Column<bool>(type: "bit", nullable: false),
                    IsMAR = table.Column<bool>(type: "bit", nullable: false),
                    Disability = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FinancialAid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MedicalAid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequiresSpecification = table.Column<bool>(type: "bit", nullable: true),
                    FK_ResidentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsResidentCategories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_rimsResidentCategories_ResidentId",
                        column: x => x.FK_ResidentId,
                        principalTable: "rimsResidents",
                        principalColumn: "ResidentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rimsRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rimsRoleClaims_rimsRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "rimsRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rimsAddresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LotNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BlockNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BldgNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Purok = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FK_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsAddresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Addresses_Id",
                        column: x => x.FK_Id,
                        principalTable: "rimsStreets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rimsAuditTrail",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApplicationId = table.Column<int>(type: "int", nullable: true),
                    FK_UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LoginTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LogoutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ActionStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    FK_ActionId = table.Column<int>(type: "int", nullable: true),
                    KeyValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsAuditTrail", x => x.AuditId);
                    table.ForeignKey(
                        name: "FK_rimsAuditTrail_ActionId",
                        column: x => x.FK_ActionId,
                        principalTable: "rimsUserActions",
                        principalColumn: "ActionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rimsAuditTrail_UserId",
                        column: x => x.FK_UserId,
                        principalTable: "rimsUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rimsUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rimsUserClaims_rimsUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "rimsUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rimsUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_rimsUserLogins_rimsUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "rimsUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rimsUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_rimsUserRoles_rimsRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "rimsRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rimsUserRoles_rimsUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "rimsUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rimsDocumentApplication",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CivilStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ResidencyStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EducationalAttainment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmploymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MonthlyIncome = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PrecinctNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PollingPlace = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DateInsurance = table.Column<DateTime>(type: "date", nullable: true),
                    PeriodOfValidity = table.Column<DateTime>(type: "date", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "date", nullable: true),
                    OfficeHotline = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FK_ResidentId = table.Column<int>(type: "int", nullable: false),
                    FK_DocumentId = table.Column<int>(type: "int", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressID = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RejectionRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rimsDocumentApplication", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_rimsDocumentApplication_AddressId",
                        column: x => x.AddressID,
                        principalTable: "rimsAddresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rimsDocumentApplication_DocumentId",
                        column: x => x.FK_DocumentId,
                        principalTable: "rimsDocuments",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rimsDocumentApplication_ResidentId",
                        column: x => x.FK_ResidentId,
                        principalTable: "rimsResidents",
                        principalColumn: "ResidentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rimsAddresses_FK_Id",
                table: "rimsAddresses",
                column: "FK_Id");

            migrationBuilder.CreateIndex(
                name: "IX_rimsAssistance_ResidentId",
                table: "rimsAssistance",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsAuditTrail_ActionDate",
                table: "rimsAuditTrail",
                column: "ActionDate");

            migrationBuilder.CreateIndex(
                name: "IX_rimsAuditTrail_ActionType",
                table: "rimsAuditTrail",
                column: "ActionType");

            migrationBuilder.CreateIndex(
                name: "IX_rimsAuditTrail_FK_ActionId",
                table: "rimsAuditTrail",
                column: "FK_ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsAuditTrail_UserId",
                table: "rimsAuditTrail",
                column: "FK_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsDocumentApplication_AddressID",
                table: "rimsDocumentApplication",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_rimsDocumentApplication_FK_DocumentId",
                table: "rimsDocumentApplication",
                column: "FK_DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsDocumentApplication_FK_ResidentId",
                table: "rimsDocumentApplication",
                column: "FK_ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsHouseholdMembers_HeadOfHouseholdId",
                table: "rimsHouseholdMembers",
                column: "HeadOfHouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsHouseholdMembers_IsHeadOfHousehold",
                table: "rimsHouseholdMembers",
                column: "IsHeadOfHousehold");

            migrationBuilder.CreateIndex(
                name: "IX_rimsHouseholdMembers_ResidentId",
                table: "rimsHouseholdMembers",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsResidentCategories_FK_ResidentId",
                table: "rimsResidentCategories",
                column: "FK_ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsResidents_HeadOfHouseholdId",
                table: "rimsResidents",
                column: "HeadOfHouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsResidents_LastName_FirstName",
                table: "rimsResidents",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_rimsRoleClaims_RoleId",
                table: "rimsRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsUserClaims_UserId",
                table: "rimsUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsUserLogins_UserId",
                table: "rimsUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_rimsUserRoles_RoleId",
                table: "rimsUserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rimsAssistance");

            migrationBuilder.DropTable(
                name: "rimsAuditTrail");

            migrationBuilder.DropTable(
                name: "rimsDocumentApplication");

            migrationBuilder.DropTable(
                name: "rimsHouseholdMembers");

            migrationBuilder.DropTable(
                name: "rimsResidentCategories");

            migrationBuilder.DropTable(
                name: "rimsRoleClaims");

            migrationBuilder.DropTable(
                name: "rimsUserClaims");

            migrationBuilder.DropTable(
                name: "rimsUserLogins");

            migrationBuilder.DropTable(
                name: "rimsUserRoles");

            migrationBuilder.DropTable(
                name: "rimsUserActions");

            migrationBuilder.DropTable(
                name: "rimsAddresses");

            migrationBuilder.DropTable(
                name: "rimsDocuments");

            migrationBuilder.DropTable(
                name: "rimsResidents");

            migrationBuilder.DropTable(
                name: "rimsRoles");

            migrationBuilder.DropTable(
                name: "rimsUsers");

            migrationBuilder.DropTable(
                name: "rimsStreets");
        }
    }
}

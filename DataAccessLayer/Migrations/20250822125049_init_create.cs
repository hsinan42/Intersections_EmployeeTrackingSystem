using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class init_create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Intersections",
                columns: table => new
                {
                    IntersectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KkcID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IntersectionStatus = table.Column<bool>(type: "bit", nullable: false),
                    RoadName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BondedOrganisation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CamCount = table.Column<int>(type: "int", nullable: true),
                    LoopCount = table.Column<int>(type: "int", nullable: true),
                    GroupCount = table.Column<int>(type: "int", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpuHex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverHex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    PedButton = table.Column<bool>(type: "bit", nullable: false),
                    UPS = table.Column<bool>(type: "bit", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intersections", x => x.IntersectionID);
                    table.ForeignKey(
                        name: "FK_Intersections_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "IntersectionChangeRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntersectionID = table.Column<int>(type: "int", nullable: true),
                    ChangeType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedByUserId = table.Column<int>(type: "int", nullable: false),
                    ReviewedByUserId = table.Column<int>(type: "int", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SnapshotUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntersectionChangeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntersectionChangeRequest_Intersections_IntersectionID",
                        column: x => x.IntersectionID,
                        principalTable: "Intersections",
                        principalColumn: "IntersectionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntersectionChangeRequest_Users_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntersectionChangeRequest_Users_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntersectionsImages",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageStatus = table.Column<bool>(type: "bit", nullable: false),
                    IntersectionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntersectionsImages", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_IntersectionsImages_Intersections_IntersectionID",
                        column: x => x.IntersectionID,
                        principalTable: "Intersections",
                        principalColumn: "IntersectionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntersectionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationID);
                    table.ForeignKey(
                        name: "FK_Locations_Intersections_IntersectionID",
                        column: x => x.IntersectionID,
                        principalTable: "Intersections",
                        principalColumn: "IntersectionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntersectionID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK_Reports_Intersections_IntersectionID",
                        column: x => x.IntersectionID,
                        principalTable: "Intersections",
                        principalColumn: "IntersectionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Substructures",
                columns: table => new
                {
                    SubstructureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubstructureStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubstructureFinishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubstructureBuilder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntersectionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Substructures", x => x.SubstructureID);
                    table.ForeignKey(
                        name: "FK_Substructures_Intersections_IntersectionID",
                        column: x => x.IntersectionID,
                        principalTable: "Intersections",
                        principalColumn: "IntersectionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntersectionChangeRequest_IntersectionID_Status",
                table: "IntersectionChangeRequest",
                columns: new[] { "IntersectionID", "Status" },
                unique: true,
                filter: "[IntersectionID] IS NOT NULL AND [Status] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_IntersectionChangeRequest_RequestedByUserId",
                table: "IntersectionChangeRequest",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IntersectionChangeRequest_ReviewedByUserId",
                table: "IntersectionChangeRequest",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Intersections_UserID",
                table: "Intersections",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_IntersectionsImages_IntersectionID",
                table: "IntersectionsImages",
                column: "IntersectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_IntersectionID",
                table: "Locations",
                column: "IntersectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_IntersectionID",
                table: "Reports",
                column: "IntersectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserID",
                table: "Reports",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Substructures_IntersectionID",
                table: "Substructures",
                column: "IntersectionID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntersectionChangeRequest");

            migrationBuilder.DropTable(
                name: "IntersectionsImages");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Substructures");

            migrationBuilder.DropTable(
                name: "Intersections");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

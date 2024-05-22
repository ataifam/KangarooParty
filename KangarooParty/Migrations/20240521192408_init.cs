using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KangarooParty.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prestige = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kangaroos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pic = table.Column<int>(type: "int", nullable: false),
                    HostingPartyId = table.Column<int>(type: "int", nullable: true),
                    AttendingPartyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kangaroos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kangaroos_Parties_AttendingPartyId",
                        column: x => x.AttendingPartyId,
                        principalTable: "Parties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Kangaroos_Parties_HostingPartyId",
                        column: x => x.HostingPartyId,
                        principalTable: "Parties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kangaroos_AttendingPartyId",
                table: "Kangaroos",
                column: "AttendingPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Kangaroos_HostingPartyId",
                table: "Kangaroos",
                column: "HostingPartyId",
                unique: true,
                filter: "[HostingPartyId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kangaroos");

            migrationBuilder.DropTable(
                name: "Parties");
        }
    }
}

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
                name: "Kangaroos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pic = table.Column<int>(type: "int", nullable: false),
                    AttendingPartyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kangaroos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HostId = table.Column<int>(type: "int", nullable: false),
                    Prestige = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parties_Kangaroos_HostId",
                        column: x => x.HostId,
                        principalTable: "Kangaroos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kangaroos_AttendingPartyId",
                table: "Kangaroos",
                column: "AttendingPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_HostId",
                table: "Parties",
                column: "HostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Kangaroos_Parties_AttendingPartyId",
                table: "Kangaroos",
                column: "AttendingPartyId",
                principalTable: "Parties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kangaroos_Parties_AttendingPartyId",
                table: "Kangaroos");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "Kangaroos");
        }
    }
}

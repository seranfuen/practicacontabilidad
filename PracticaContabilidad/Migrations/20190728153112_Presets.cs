using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PracticaContabilidad.Migrations
{
    public partial class Presets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JournalEntryGroupPresets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SoftDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryGroupPresets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryPresetEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DebitCredit = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    JournalEntryGroupPresetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryPresetEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntryPresetEntry_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntryPresetEntry_JournalEntryGroupPresets_JournalEntryGroupPresetId",
                        column: x => x.JournalEntryGroupPresetId,
                        principalTable: "JournalEntryGroupPresets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryPresetEntry_AccountId",
                table: "JournalEntryPresetEntry",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryPresetEntry_JournalEntryGroupPresetId",
                table: "JournalEntryPresetEntry",
                column: "JournalEntryGroupPresetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JournalEntryPresetEntry");

            migrationBuilder.DropTable(
                name: "JournalEntryGroupPresets");
        }
    }
}

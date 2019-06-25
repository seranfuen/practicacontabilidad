using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PracticaContabilidad.Migrations
{
    public partial class JournalEntryGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Accounts_AccountId",
                table: "LedgerEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LedgerEntries",
                table: "LedgerEntries");

            migrationBuilder.RenameTable(
                name: "LedgerEntries",
                newName: "LedgerEntry");

            migrationBuilder.RenameIndex(
                name: "IX_LedgerEntries_AccountId",
                table: "LedgerEntry",
                newName: "IX_LedgerEntry_AccountId");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Accounts",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 11);

            migrationBuilder.AddColumn<int>(
                name: "JournalEntryGroupId",
                table: "LedgerEntry",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LedgerEntry",
                table: "LedgerEntry",
                column: "LedgerEntryId");

            migrationBuilder.CreateTable(
                name: "JournalEntryGroups",
                columns: table => new
                {
                    JournalEntryGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryGroups", x => x.JournalEntryGroupId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntry_JournalEntryGroupId",
                table: "LedgerEntry",
                column: "JournalEntryGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntry_Accounts_AccountId",
                table: "LedgerEntry",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntry_JournalEntryGroups_JournalEntryGroupId",
                table: "LedgerEntry",
                column: "JournalEntryGroupId",
                principalTable: "JournalEntryGroups",
                principalColumn: "JournalEntryGroupId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntry_Accounts_AccountId",
                table: "LedgerEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntry_JournalEntryGroups_JournalEntryGroupId",
                table: "LedgerEntry");

            migrationBuilder.DropTable(
                name: "JournalEntryGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LedgerEntry",
                table: "LedgerEntry");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntry_JournalEntryGroupId",
                table: "LedgerEntry");

            migrationBuilder.DropColumn(
                name: "JournalEntryGroupId",
                table: "LedgerEntry");

            migrationBuilder.RenameTable(
                name: "LedgerEntry",
                newName: "LedgerEntries");

            migrationBuilder.RenameIndex(
                name: "IX_LedgerEntry_AccountId",
                table: "LedgerEntries",
                newName: "IX_LedgerEntries_AccountId");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Accounts",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 9);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LedgerEntries",
                table: "LedgerEntries",
                column: "LedgerEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Accounts_AccountId",
                table: "LedgerEntries",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

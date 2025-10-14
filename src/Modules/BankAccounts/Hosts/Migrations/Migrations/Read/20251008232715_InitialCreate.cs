using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLedger.Modules.BankAccounts.Hosts.Migrations.Migrations.Read
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "read");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "read",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "read",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Category = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.uuid);
                });

            migrationBuilder.CreateIndex(
                name: "idx_accounts_userid",
                schema: "read",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "idx_accounts_userid_createdat",
                schema: "read",
                table: "Accounts",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "idx_transactions_accountid",
                schema: "read",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "idx_transactions_accountid_category_createdat",
                schema: "read",
                table: "Transactions",
                columns: new[] { "AccountId", "Category", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "idx_transactions_userid",
                schema: "read",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "idx_transactions_userid_createdat",
                schema: "read",
                table: "Transactions",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "idx_transactions_userid_type_createdat",
                schema: "read",
                table: "Transactions",
                columns: new[] { "UserId", "Type", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "read");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "read");
        }
    }
}

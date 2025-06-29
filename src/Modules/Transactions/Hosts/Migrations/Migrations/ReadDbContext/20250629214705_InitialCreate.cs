using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLedger.Modules.Transactions.Hosts.Migrations.Migrations.ReadDbContext
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
                name: "AccountReadModel",
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
                    table.PrimaryKey("PK_AccountReadModel", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "TransactionReadModel",
                schema: "read",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionReadModel", x => x.uuid);
                });

            migrationBuilder.CreateIndex(
                name: "idx_accountreadmodel_userid",
                schema: "read",
                table: "AccountReadModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "idx_accountreadmodel_userid_createdat",
                schema: "read",
                table: "AccountReadModel",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "idx_transactionreadmodel_accountid",
                schema: "read",
                table: "TransactionReadModel",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "idx_transactionreadmodel_accountid_category_createdat",
                schema: "read",
                table: "TransactionReadModel",
                columns: new[] { "AccountId", "Category", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "idx_transactionreadmodel_userid",
                schema: "read",
                table: "TransactionReadModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "idx_transactionreadmodel_userid_createdat",
                schema: "read",
                table: "TransactionReadModel",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "idx_transactionreadmodel_userid_type_createdat",
                schema: "read",
                table: "TransactionReadModel",
                columns: new[] { "UserId", "Type", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountReadModel",
                schema: "read");

            migrationBuilder.DropTable(
                name: "TransactionReadModel",
                schema: "read");
        }
    }
}

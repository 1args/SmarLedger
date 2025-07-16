using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLedger.Modules.Budgets.Hosts.Migrations.Migrations.ReadDbContext
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
                name: "Budgets",
                schema: "read",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetItems",
                schema: "read",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    BudgetName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Limit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    decimal182 = table.Column<decimal>(name: "decimal(18,2)", type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    BudgetReadModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetItems", x => x.uuid);
                    table.ForeignKey(
                        name: "FK_BudgetItems_Budgets_BudgetReadModelId",
                        column: x => x.BudgetReadModelId,
                        principalSchema: "read",
                        principalTable: "Budgets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "idc_budgetitems_budgetid_userid",
                schema: "read",
                table: "BudgetItems",
                columns: new[] { "BudgetId", "Category" });

            migrationBuilder.CreateIndex(
                name: "idx_budgetitems_budgetid",
                schema: "read",
                table: "BudgetItems",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "idx_budgetitems_userid",
                schema: "read",
                table: "BudgetItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_BudgetReadModelId",
                schema: "read",
                table: "BudgetItems",
                column: "BudgetReadModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetItems",
                schema: "read");

            migrationBuilder.DropTable(
                name: "Budgets",
                schema: "read");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "to_do_lists",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    creation_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completion_date_time = table.Column<DateOnly>(type: "date", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_to_do_lists", x => x.id);
                    table.ForeignKey(
                        name: "FK_to_do_lists_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "to_do_tasks",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    creation_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completion_date_time = table.Column<DateOnly>(type: "date", nullable: false),
                    to_do_list_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_to_do_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_to_do_tasks_to_do_lists_to_do_list_id",
                        column: x => x.to_do_list_id,
                        principalSchema: "public",
                        principalTable: "to_do_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_to_do_lists_user_id",
                schema: "public",
                table: "to_do_lists",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_to_do_tasks_to_do_list_id",
                schema: "public",
                table: "to_do_tasks",
                column: "to_do_list_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "to_do_tasks",
                schema: "public");

            migrationBuilder.DropTable(
                name: "to_do_lists",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}

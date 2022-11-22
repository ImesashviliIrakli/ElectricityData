using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enitites.Migrations
{
    /// <inheritdoc />
    public partial class Main : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupedTinklas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tinklas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PPlusSum = table.Column<float>(type: "real", nullable: true),
                    PMinusSum = table.Column<float>(type: "real", nullable: true),
                    Month = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupedTinklas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupedTinklas");
        }
    }
}

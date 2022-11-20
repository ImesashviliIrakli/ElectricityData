using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectricityData.Migrations
{
    /// <inheritdoc />
    public partial class Main : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElectricityData",
                columns: table => new
                {
                    TINKLAS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OBTPAVADINIMAS = table.Column<string>(name: "OBT_PAVADINIMAS", type: "nvarchar(max)", nullable: true),
                    OBJGVTIPAS = table.Column<string>(name: "OBJ_GV_TIPAS", type: "nvarchar(max)", nullable: true),
                    OBJNUMERIS = table.Column<int>(name: "OBJ_NUMERIS", type: "int", nullable: false),
                    PPlus = table.Column<float>(type: "real", nullable: true),
                    PLT = table.Column<DateTime>(name: "PL_T", type: "datetime2", nullable: false),
                    PMINUS = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GroupedTinklas",
                columns: table => new
                {
                    Tinklas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PPlusSum = table.Column<float>(type: "real", nullable: false),
                    PMinusSum = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectricityData");

            migrationBuilder.DropTable(
                name: "GroupedTinklas");
        }
    }
}

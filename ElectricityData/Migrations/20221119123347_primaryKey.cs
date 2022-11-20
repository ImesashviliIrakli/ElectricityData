using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectricityData.Migrations
{
    /// <inheritdoc />
    public partial class primaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ElectricityData",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElectricityData",
                table: "ElectricityData",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ElectricityData",
                table: "ElectricityData");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ElectricityData");
        }
    }
}

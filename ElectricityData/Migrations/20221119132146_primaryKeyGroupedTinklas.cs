using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectricityData.Migrations
{
    /// <inheritdoc />
    public partial class primaryKeyGroupedTinklas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PPlusSum",
                table: "GroupedTinklas",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "PMinusSum",
                table: "GroupedTinklas",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GroupedTinklas",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupedTinklas",
                table: "GroupedTinklas",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupedTinklas",
                table: "GroupedTinklas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GroupedTinklas");

            migrationBuilder.AlterColumn<float>(
                name: "PPlusSum",
                table: "GroupedTinklas",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "PMinusSum",
                table: "GroupedTinklas",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dummy1.Migrations
{
    /// <inheritdoc />
    public partial class modify_districtTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProvinceCode",
                table: "Districts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DistrictCode",
                table: "Communes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProvinceCode",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "DistrictCode",
                table: "Communes");
        }
    }
}

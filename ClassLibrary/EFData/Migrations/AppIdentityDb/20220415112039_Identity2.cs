using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestWebAppl.Migrations.AppIdentityDb
{
    public partial class Identity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalPhone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalPhone",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.IdentityServer.Migrations
{
    /// <inheritdoc />
    public partial class AddAdhocChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdHoc",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdHoc",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinterest.Migrations
{
    /// <inheritdoc />
    public partial class saved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Saveds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Saveds_PostId",
                table: "Saveds",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Saveds_Posts_PostId",
                table: "Saveds",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Saveds_Posts_PostId",
                table: "Saveds");

            migrationBuilder.DropIndex(
                name: "IX_Saveds_PostId",
                table: "Saveds");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Saveds");
        }
    }
}

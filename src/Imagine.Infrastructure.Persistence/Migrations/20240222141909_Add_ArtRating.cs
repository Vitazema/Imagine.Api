using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imagine.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ArtRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Arts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Arts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Arts");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Arts");
        }
    }
}

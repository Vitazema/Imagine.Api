using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imagine.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Addurls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Arts");

            migrationBuilder.AddColumn<List<string>>(
                name: "Urls",
                table: "Arts",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Urls",
                table: "Arts");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Arts",
                type: "text",
                nullable: true);
        }
    }
}

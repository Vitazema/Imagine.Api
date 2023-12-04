using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imagine.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Addworkerid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Arts");

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Arts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Arts");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "Arts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

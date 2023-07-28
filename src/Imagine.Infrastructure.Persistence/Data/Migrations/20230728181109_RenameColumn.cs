using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imagine.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Users",
                newName: "Role");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 18, 11, 8, 988, DateTimeKind.Utc).AddTicks(2086));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 18, 11, 8, 988, DateTimeKind.Utc).AddTicks(2297));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 18, 11, 8, 988, DateTimeKind.Utc).AddTicks(2314));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 18, 11, 8, 988, DateTimeKind.Utc).AddTicks(2335));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 18, 11, 8, 988, DateTimeKind.Utc).AddTicks(2348));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "UserType");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 17, 56, 35, 687, DateTimeKind.Utc).AddTicks(3649));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 17, 56, 35, 687, DateTimeKind.Utc).AddTicks(3843));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 17, 56, 35, 687, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 17, 56, 35, 687, DateTimeKind.Utc).AddTicks(3884));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 17, 56, 35, 687, DateTimeKind.Utc).AddTicks(3899));
        }
    }
}

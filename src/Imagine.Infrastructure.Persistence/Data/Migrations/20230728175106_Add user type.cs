using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Imagine.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addusertype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserType" },
                values: new object[] { new DateTime(2023, 7, 28, 17, 51, 6, 707, DateTimeKind.Utc).AddTicks(9750), 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "FullName", "UserType" },
                values: new object[] { new DateTime(2023, 7, 28, 17, 51, 6, 707, DateTimeKind.Utc).AddTicks(9855), "Guest", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "FullName", "UserType" },
                values: new object[,]
                {
                    { 3, new DateTime(2023, 7, 28, 17, 51, 6, 707, DateTimeKind.Utc).AddTicks(9868), "UserName", 0 },
                    { 4, new DateTime(2023, 7, 28, 17, 51, 6, 707, DateTimeKind.Utc).AddTicks(9886), "TrialUser", 0 },
                    { 5, new DateTime(2023, 7, 28, 17, 51, 6, 707, DateTimeKind.Utc).AddTicks(9896), "PaidUser", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 25, 20, 30, 50, 969, DateTimeKind.Utc).AddTicks(731));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "FullName" },
                values: new object[] { new DateTime(2023, 7, 25, 20, 30, 50, 969, DateTimeKind.Utc).AddTicks(926), "UserName" });
        }
    }
}

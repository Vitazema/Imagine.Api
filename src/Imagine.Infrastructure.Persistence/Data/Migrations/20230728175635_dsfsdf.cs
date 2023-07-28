using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imagine.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class dsfsdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "CreatedAt", "UserType" },
                values: new object[] { new DateTime(2023, 7, 28, 17, 56, 35, 687, DateTimeKind.Utc).AddTicks(3843), 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UserType" },
                values: new object[] { new DateTime(2023, 7, 28, 17, 56, 35, 687, DateTimeKind.Utc).AddTicks(3870), 2 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UserType" },
                values: new object[] { new DateTime(2023, 7, 28, 17, 56, 35, 687, DateTimeKind.Utc).AddTicks(3884), 3 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UserType" },
                values: new object[] { new DateTime(2023, 7, 28, 17, 56, 35, 687, DateTimeKind.Utc).AddTicks(3899), 4 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 7, 28, 17, 51, 6, 707, DateTimeKind.Utc).AddTicks(9750));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UserType" },
                values: new object[] { new DateTime(2023, 7, 28, 17, 51, 6, 707, DateTimeKind.Utc).AddTicks(9855), 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UserType" },
                values: new object[] { new DateTime(2023, 7, 28, 17, 51, 6, 707, DateTimeKind.Utc).AddTicks(9868), 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UserType" },
                values: new object[] { new DateTime(2023, 7, 28, 17, 51, 6, 707, DateTimeKind.Utc).AddTicks(9886), 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UserType" },
                values: new object[] { new DateTime(2023, 7, 28, 17, 51, 6, 707, DateTimeKind.Utc).AddTicks(9896), 0 });
        }
    }
}

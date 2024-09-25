using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UFSBankingSystemWebsite.Migrations
{
    /// <inheritdoc />
    public partial class ModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "030aaff2-5b15-45d1-b076-78fccc1f57e6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "404fd9e5-52e5-4ea1-ae5c-158e1bbf9ab7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ff8b77a-f71d-4832-83c6-1e0ee72e5b22");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f21cb557-f460-43fb-bb71-c8f5033ac073");

            migrationBuilder.AlterColumn<long>(
                name: "StudentStaffNumber",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "IDnumber",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3069082f-ee80-40cd-816b-0f0d550ec2f6", null, "Consultant", "CONSULTANT" },
                    { "59c9b973-c29e-40ab-95cc-39c95e7b3680", null, "Admin", "ADMIN" },
                    { "9d862794-b798-497e-a680-0f14a76ffd8f", null, "User", "USER" },
                    { "d981a7fb-292e-4ee5-94d4-c3627a988006", null, "FinancialAdvisor", "FINANCIALADVISOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3069082f-ee80-40cd-816b-0f0d550ec2f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59c9b973-c29e-40ab-95cc-39c95e7b3680");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d862794-b798-497e-a680-0f14a76ffd8f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d981a7fb-292e-4ee5-94d4-c3627a988006");

            migrationBuilder.AlterColumn<string>(
                name: "StudentStaffNumber",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "IDnumber",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "030aaff2-5b15-45d1-b076-78fccc1f57e6", null, "FinancialAdvisor", "FINANCIALADVISOR" },
                    { "404fd9e5-52e5-4ea1-ae5c-158e1bbf9ab7", null, "Admin", "ADMIN" },
                    { "4ff8b77a-f71d-4832-83c6-1e0ee72e5b22", null, "User", "USER" },
                    { "f21cb557-f460-43fb-bb71-c8f5033ac073", null, "Consultant", "CONSULTANT" }
                });
        }
    }
}

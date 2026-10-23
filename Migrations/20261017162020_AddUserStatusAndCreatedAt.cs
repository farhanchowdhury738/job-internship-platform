using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireLink.Migrations
{
    /// <summary>
    /// Adds the account status and join date columns used by the admin
    /// user-management screen. The Users and Jobs tables already exist in the
    /// database (they were created with a SQL script), so this migration only
    /// adds the missing columns and never recreates or drops existing tables.
    /// </summary>
    public partial class AddUserStatusAndCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");
        }
    }
}

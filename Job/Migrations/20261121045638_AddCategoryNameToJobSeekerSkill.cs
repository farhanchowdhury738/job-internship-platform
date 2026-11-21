
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Migrations
{
    public partial class AddCategoryNameToJobSeekerSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "job_seeker_skills",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "job_seeker_skills");
        }
    }
}

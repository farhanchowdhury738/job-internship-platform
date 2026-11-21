using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job_categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyLogo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_companies_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_seeker_certificates",
                columns: table => new
                {
                    CertificateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CertificateName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CertificateUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_seeker_certificates", x => x.CertificateId);
                    table.ForeignKey(
                        name: "FK_job_seeker_certificates_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_seeker_educations",
                columns: table => new
                {
                    EducationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InstitutionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Gpa = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_seeker_educations", x => x.EducationId);
                    table.ForeignKey(
                        name: "FK_job_seeker_educations_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_seeker_projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProjectTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiveUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_seeker_projects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_job_seeker_projects_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_seeker_skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SkillName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_seeker_skills", x => x.SkillId);
                    table.ForeignKey(
                        name: "FK_job_seeker_skills_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_job_posts",
                columns: table => new
                {
                    JobPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Vacancy = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Salary = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExperienceRequired = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApplicationDeadline = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_job_posts", x => x.JobPostId);
                    table.ForeignKey(
                        name: "FK_company_job_posts_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_company_job_posts_job_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "job_categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobPostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ApplicationStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId1 = table.Column<int>(type: "int", nullable: true),
                    UserId2 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_applications", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_job_applications_company_job_posts_JobPostId",
                        column: x => x.JobPostId,
                        principalTable: "company_job_posts",
                        principalColumn: "JobPostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_applications_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_job_applications_users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_job_applications_users_UserId2",
                        column: x => x.UserId2,
                        principalTable: "users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "job_seeker_saved_jobs",
                columns: table => new
                {
                    SavedJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JobPostId = table.Column<int>(type: "int", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_seeker_saved_jobs", x => x.SavedJobId);
                    table.ForeignKey(
                        name: "FK_job_seeker_saved_jobs_company_job_posts_JobPostId",
                        column: x => x.JobPostId,
                        principalTable: "company_job_posts",
                        principalColumn: "JobPostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_seeker_saved_jobs_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_job_seeker_saved_jobs_users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_companies_UserId",
                table: "companies",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_company_job_posts_CategoryId",
                table: "company_job_posts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_company_job_posts_CompanyId",
                table: "company_job_posts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_job_applications_JobPostId",
                table: "job_applications",
                column: "JobPostId");

            migrationBuilder.CreateIndex(
                name: "IX_job_applications_UserId",
                table: "job_applications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_job_applications_UserId1",
                table: "job_applications",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_job_applications_UserId2",
                table: "job_applications",
                column: "UserId2");

            migrationBuilder.CreateIndex(
                name: "IX_job_seeker_certificates_UserId",
                table: "job_seeker_certificates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_job_seeker_educations_UserId",
                table: "job_seeker_educations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_job_seeker_projects_UserId",
                table: "job_seeker_projects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_job_seeker_saved_jobs_JobPostId",
                table: "job_seeker_saved_jobs",
                column: "JobPostId");

            migrationBuilder.CreateIndex(
                name: "IX_job_seeker_saved_jobs_UserId",
                table: "job_seeker_saved_jobs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_job_seeker_saved_jobs_UserId1",
                table: "job_seeker_saved_jobs",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_job_seeker_skills_UserId",
                table: "job_seeker_skills",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_applications");

            migrationBuilder.DropTable(
                name: "job_seeker_certificates");

            migrationBuilder.DropTable(
                name: "job_seeker_educations");

            migrationBuilder.DropTable(
                name: "job_seeker_projects");

            migrationBuilder.DropTable(
                name: "job_seeker_saved_jobs");

            migrationBuilder.DropTable(
                name: "job_seeker_skills");

            migrationBuilder.DropTable(
                name: "company_job_posts");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "job_categories");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

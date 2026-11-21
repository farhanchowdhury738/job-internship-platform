
using Job.Models;
using Microsoft.EntityFrameworkCore;

namespace Job.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<JobSeekerSkill> JobSeekerSkills { get; set; }
        public DbSet<JobSeekerProject> JobSeekerProjects { get; set; }
        public DbSet<JobSeekerEducation> JobSeekerEducations { get; set; }
        public DbSet<JobSeekerCertificate> JobSeekerCertificates { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<CompanyJobPost> CompanyJobPosts { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobSeekerSavedJob> JobSeekerSavedJobs { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistory { get; set; }
        // Table Names and Relationships

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table Names
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<JobSeekerSkill>().ToTable("job_seeker_skills");
            modelBuilder.Entity<JobSeekerProject>().ToTable("job_seeker_projects");
            modelBuilder.Entity<JobSeekerEducation>().ToTable("job_seeker_educations");
            modelBuilder.Entity<JobSeekerCertificate>().ToTable("job_seeker_certificates");
            modelBuilder.Entity<Company>().ToTable("companies");
            modelBuilder.Entity<JobCategory>().ToTable("job_categories");
            modelBuilder.Entity<CompanyJobPost>().ToTable("company_job_posts");
            modelBuilder.Entity<JobApplication>().ToTable("job_applications");
            modelBuilder.Entity<JobSeekerSavedJob>().ToTable("job_seeker_saved_jobs");

            // GPA Precision
            modelBuilder.Entity<JobSeekerEducation>().Property(e => e.Gpa).HasPrecision(4, 2);

            // Job Application Relationships
            modelBuilder.Entity<JobApplication>().HasOne(a => a.User).WithMany(u => u.Applications).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<JobApplication>().HasOne(a => a.CompanyJobPost).WithMany(j => j.Applications).HasForeignKey(a => a.JobPostId).OnDelete(DeleteBehavior.NoAction);

            // Saved Job Relationship
            modelBuilder.Entity<JobSeekerSavedJob>().HasOne(s => s.User).WithMany(u => u.SavedJobs).HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.NoAction);

            // Prevent Duplicate Applications
            modelBuilder.Entity<JobApplication>().HasIndex(a => new { a.UserId, a.JobPostId }).IsUnique();
        }

    }
}

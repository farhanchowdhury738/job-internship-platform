using JobInternshipPlatform.Models;

namespace JobInternshipPlatform.Data
{
    /// <summary>
    /// Static demo data used while the back end is not built yet.
    /// Replace the calls to this class in the controllers with real services / repositories later.
    /// </summary>
    public static class MockData
    {
        public static readonly List<Company> Companies = new()
        {
            new Company { Id = 1, Name = "Nexora Tech", Industry = "Software", Location = "Dhaka, Bangladesh", Size = "201-500", Website = "nexora.example", Founded = 2015, Color = "#4f46e5", Status = "Verified", OpenJobs = 4, About = "Nexora Tech builds cloud-native products for fintech and logistics companies across South Asia. We care about clean engineering, fast feedback and growing people." },
            new Company { Id = 2, Name = "BlueWave Studio", Industry = "Design & Creative", Location = "Remote", Size = "51-200", Website = "bluewave.example", Founded = 2018, Color = "#0ea5e9", Status = "Verified", OpenJobs = 3, About = "A distributed product design studio crafting brands, apps and websites for startups around the world." },
            new Company { Id = 3, Name = "GreenLeaf Energy", Industry = "Energy", Location = "Chattogram, Bangladesh", Size = "501-1000", Website = "greenleaf.example", Founded = 2009, Color = "#16a34a", Status = "Verified", OpenJobs = 2, About = "GreenLeaf delivers solar and renewable energy solutions to homes and industry." },
            new Company { Id = 4, Name = "Finlytics", Industry = "Fintech", Location = "Singapore", Size = "51-200", Website = "finlytics.example", Founded = 2019, Color = "#f59e0b", Status = "Verified", OpenJobs = 3, About = "Finlytics turns financial data into actionable insight with machine learning and elegant dashboards." },
            new Company { Id = 5, Name = "MediCare Plus", Industry = "Healthcare", Location = "Dhaka, Bangladesh", Size = "1000+", Website = "medicareplus.example", Founded = 2001, Color = "#e11d48", Status = "Verified", OpenJobs = 2, About = "One of the leading healthcare networks, running hospitals, labs and telemedicine services." },
            new Company { Id = 6, Name = "Orbit Logistics", Industry = "Logistics", Location = "Sylhet, Bangladesh", Size = "201-500", Website = "orbitlog.example", Founded = 2012, Color = "#7c3aed", Status = "Pending", OpenJobs = 1, About = "Orbit moves goods faster with smart routing, last-mile delivery and warehouse automation." },
            new Company { Id = 7, Name = "Pixel Forge Games", Industry = "Gaming", Location = "Remote", Size = "11-50", Website = "pixelforge.example", Founded = 2021, Color = "#ec4899", Status = "Verified", OpenJobs = 1, About = "An indie game studio making cozy, story-rich mobile games." },
            new Company { Id = 8, Name = "EduSpark", Industry = "Education", Location = "Dhaka, Bangladesh", Size = "51-200", Website = "eduspark.example", Founded = 2017, Color = "#14b8a6", Status = "Suspended", OpenJobs = 0, About = "EduSpark is an online learning platform for school and university students." },
        };

        private static Company C(int id) => Companies.First(c => c.Id == id);

        private static Job J(int id, string title, int cid, string loc, string type, string cat, string level, bool remote,
            int min, int max, int postedDaysAgo, int deadlineInDays, string[] skills, int applicants, int views, string status = "Active", bool saved = false)
        {
            var c = C(cid);
            return new Job
            {
                Id = id, Title = title, CompanyId = cid, CompanyName = c.Name, CompanyColor = c.Color,
                Location = loc, Type = type, Category = cat, Level = level, IsRemote = remote,
                SalaryMin = min, SalaryMax = max,
                PostedDate = DateTime.Today.AddDays(-postedDaysAgo), Deadline = DateTime.Today.AddDays(deadlineInDays),
                Skills = skills.ToList(), ApplicantCount = applicants, Views = views, Status = status, IsSaved = saved,
                Description = $"We are looking for a motivated {title} to join {c.Name}. You will work with a friendly cross-functional team, own meaningful work from day one and grow quickly in a supportive environment.",
                Responsibilities = new() {
                    "Collaborate with product, design and engineering teams to deliver great user experiences",
                    "Take ownership of tasks from planning to delivery and share progress regularly",
                    "Write clean, maintainable work and take part in reviews",
                    "Continuously learn new tools and suggest improvements" },
                Requirements = new() {
                    level == "Entry" ? "0-1 years of relevant experience or a strong portfolio / projects" : level == "Mid" ? "2-4 years of relevant professional experience" : "5+ years of relevant professional experience",
                    "Strong communication and problem solving skills",
                    "Ability to work in a team and meet deadlines",
                    "Degree in a related field or equivalent practical experience" },
                Benefits = new() { "Flexible working hours", "Health insurance", "Learning budget", "Festival bonuses", "Friendly team culture" }
            };
        }

        public static readonly List<Job> Jobs = new()
        {
            J(1, "Senior Frontend Engineer", 1, "Dhaka, Bangladesh", "Full-time", "Engineering", "Senior", false, 60000, 85000, 1, 25, new[]{"React","TypeScript","CSS","Testing"}, 42, 860, "Active", true),
            J(2, "UI/UX Design Intern", 2, "Remote", "Internship", "Design", "Entry", true, 400, 600, 2, 18, new[]{"Figma","Prototyping","Research"}, 88, 1320),
            J(3, ".NET Backend Developer", 1, "Dhaka, Bangladesh", "Full-time", "Engineering", "Mid", false, 40000, 60000, 3, 30, new[]{"C#","ASP.NET Core","SQL Server","REST"}, 31, 640, "Active", true),
            J(4, "Data Analyst", 4, "Singapore", "Full-time", "Data", "Mid", false, 55000, 75000, 4, 21, new[]{"SQL","Python","Power BI"}, 27, 590),
            J(5, "Marketing Intern", 2, "Remote", "Internship", "Marketing", "Entry", true, 300, 500, 1, 14, new[]{"Social Media","Copywriting","Canva"}, 64, 970),
            J(6, "Solar Project Engineer", 3, "Chattogram, Bangladesh", "Full-time", "Engineering", "Mid", false, 35000, 50000, 6, 40, new[]{"AutoCAD","Project Management","Electrical"}, 12, 310),
            J(7, "Machine Learning Engineer", 4, "Singapore", "Full-time", "Data", "Senior", true, 90000, 130000, 2, 35, new[]{"Python","PyTorch","MLOps","AWS"}, 54, 1480, "Active", false),
            J(8, "Registered Nurse", 5, "Dhaka, Bangladesh", "Full-time", "Healthcare", "Entry", false, 22000, 30000, 8, 20, new[]{"Patient Care","Communication"}, 19, 270),
            J(9, "Software Engineering Intern", 1, "Dhaka, Bangladesh", "Internship", "Engineering", "Entry", false, 350, 550, 1, 16, new[]{"JavaScript","Git","Problem Solving"}, 126, 2100, "Active", true),
            J(10, "Operations Coordinator", 6, "Sylhet, Bangladesh", "Part-time", "Operations", "Entry", false, 15000, 22000, 5, 12, new[]{"Excel","Logistics","Communication"}, 9, 180, "Pending"),
            J(11, "Game Artist (2D)", 7, "Remote", "Contract", "Design", "Mid", true, 30000, 45000, 7, 28, new[]{"Photoshop","Spine","Illustration"}, 23, 520),
            J(12, "Product Manager", 4, "Singapore", "Full-time", "Product", "Senior", false, 85000, 115000, 3, 33, new[]{"Roadmapping","Analytics","Stakeholders"}, 38, 910),
            J(13, "Data Science Intern", 4, "Remote", "Internship", "Data", "Entry", true, 450, 700, 4, 19, new[]{"Python","Statistics","Pandas"}, 97, 1740),
            J(14, "Customer Support Specialist", 5, "Dhaka, Bangladesh", "Part-time", "Support", "Entry", false, 14000, 20000, 9, 15, new[]{"Empathy","English","CRM"}, 15, 240, "Flagged"),
        };

        private static JobApplication A(int id, int jobId, string name, string title, string loc, int exp, int match, int daysAgo, ApplicationStatus st, params string[] skills)
        {
            var j = Jobs.First(x => x.Id == jobId);
            return new JobApplication
            {
                Id = id, JobId = jobId, JobTitle = j.Title, CompanyName = j.CompanyName, CompanyColor = j.CompanyColor,
                ApplicantName = name, ApplicantTitle = title, Location = loc, ExperienceYears = exp, MatchScore = match,
                Email = name.ToLower().Replace(' ', '.') + "@mail.example", AppliedDate = DateTime.Today.AddDays(-daysAgo),
                Status = st, Skills = skills.ToList()
            };
        }

        // Everything applied to Nexora Tech's jobs (recruiter view)
        public static readonly List<JobApplication> Applicants = new()
        {
            A(1, 1, "Ayesha Rahman", "Frontend Developer", "Dhaka", 6, 94, 1, ApplicationStatus.Shortlisted, "React", "TypeScript", "Next.js"),
            A(2, 1, "Tanvir Ahmed", "UI Engineer", "Chattogram", 5, 88, 2, ApplicationStatus.Interview, "React", "CSS", "Testing"),
            A(3, 3, "Nusrat Jahan", "Software Engineer", "Dhaka", 3, 91, 2, ApplicationStatus.Reviewed, "C#", "ASP.NET Core", "SQL"),
            A(4, 9, "Rafiul Islam", "CSE Student", "Dhaka", 0, 76, 1, ApplicationStatus.Pending, "JavaScript", "Git", "Python"),
            A(5, 9, "Maliha Chowdhury", "CSE Student", "Rajshahi", 0, 82, 3, ApplicationStatus.Shortlisted, "Java", "Problem Solving"),
            A(6, 3, "Imran Hossain", "Backend Developer", "Sylhet", 4, 79, 4, ApplicationStatus.Pending, "C#", "Docker", "Azure"),
            A(7, 1, "Sadia Karim", "Web Developer", "Dhaka", 4, 72, 5, ApplicationStatus.Rejected, "JavaScript", "Vue"),
            A(8, 3, "Fahim Reza", "Full-stack Developer", "Khulna", 5, 85, 6, ApplicationStatus.Accepted, "C#", "React", "SQL"),
            A(9, 9, "Tasnim Akter", "CSE Student", "Dhaka", 0, 69, 6, ApplicationStatus.Pending, "HTML", "CSS", "JavaScript"),
        };

        // Applications made by the demo job seeker
        public static readonly List<JobApplication> MyApplications = new()
        {
            Mine(101, 1, 1, ApplicationStatus.Interview),
            Mine(102, 3, 3, ApplicationStatus.Shortlisted),
            Mine(103, 2, 5, ApplicationStatus.Reviewed),
            Mine(104, 7, 6, ApplicationStatus.Pending),
            Mine(105, 12, 8, ApplicationStatus.Rejected),
            Mine(106, 9, 10, ApplicationStatus.Accepted),
        };

        private static JobApplication Mine(int id, int jobId, int daysAgo, ApplicationStatus st)
        {
            var j = Jobs.First(x => x.Id == jobId);
            return new JobApplication
            {
                Id = id, JobId = jobId, JobTitle = j.Title, CompanyName = j.CompanyName, CompanyColor = j.CompanyColor,
                ApplicantName = "Sabbir Hasan", AppliedDate = DateTime.Today.AddDays(-daysAgo), Status = st, Location = j.Location
            };
        }

        public static readonly UserProfile Profile = new()
        {
            FullName = "Sabbir Hasan", Headline = "Junior Full-stack Developer", Email = "sabbir.hasan@mail.example", Phone = "+880 1700 000000",
            Location = "Dhaka, Bangladesh", Website = "sabbir.dev", Education = "B.Sc. in Computer Science & Engineering, 2025",
            Experience = "1 year", CvFileName = "Sabbir_Hasan_CV.pdf", CvSize = "284 KB", CvUpdated = DateTime.Today.AddDays(-12),
            About = "Curious developer who enjoys building clean web apps with C#, ASP.NET Core and modern JavaScript. Looking for a team where I can learn fast and ship real products.",
            Skills = new() { "C#", "ASP.NET Core", "JavaScript", "React", "SQL Server", "Git", "Bootstrap" }, Completion = 85
        };

        public static readonly List<Notification> SeekerNotifications = new()
        {
            new Notification { Id = 1, Title = "Interview invitation", Message = "Nexora Tech invited you to an interview for Senior Frontend Engineer.", Icon = "bi-camera-video", Tone = "success", TimeAgo = "2 hours ago" },
            new Notification { Id = 2, Title = "Application shortlisted", Message = "Your application for .NET Backend Developer was shortlisted.", Icon = "bi-star", Tone = "primary", TimeAgo = "Yesterday" },
            new Notification { Id = 3, Title = "New jobs match your profile", Message = "5 new jobs match your skills: C#, ASP.NET Core.", Icon = "bi-briefcase", Tone = "info", TimeAgo = "Yesterday", IsRead = true },
            new Notification { Id = 4, Title = "Deadline approaching", Message = "Saved job 'Software Engineering Intern' closes in 3 days.", Icon = "bi-alarm", Tone = "warning", TimeAgo = "2 days ago", IsRead = true },
            new Notification { Id = 5, Title = "Application update", Message = "Finlytics has moved on with other candidates for Product Manager.", Icon = "bi-x-circle", Tone = "danger", TimeAgo = "5 days ago", IsRead = true },
        };

        public static readonly List<Notification> RecruiterNotifications = new()
        {
            new Notification { Id = 1, Title = "New applicant", Message = "Ayesha Rahman applied for Senior Frontend Engineer.", Icon = "bi-person-plus", Tone = "primary", TimeAgo = "30 minutes ago" },
            new Notification { Id = 2, Title = "126 applications", Message = "Software Engineering Intern reached 100+ applications.", Icon = "bi-graph-up-arrow", Tone = "success", TimeAgo = "3 hours ago" },
            new Notification { Id = 3, Title = "Job expiring soon", Message = ".NET Backend Developer closes in 5 days.", Icon = "bi-alarm", Tone = "warning", TimeAgo = "Yesterday", IsRead = true },
            new Notification { Id = 4, Title = "Company verified", Message = "Your company profile has been verified by the admin team.", Icon = "bi-patch-check", Tone = "info", TimeAgo = "3 days ago", IsRead = true },
        };

        public static readonly List<AppUser> Users = new()
        {
            new AppUser { Id = 1, Name = "Sabbir Hasan", Email = "sabbir.hasan@mail.example", Role = "JobSeeker", Status = "Active", Joined = DateTime.Today.AddDays(-90) },
            new AppUser { Id = 2, Name = "Ayesha Rahman", Email = "ayesha.rahman@mail.example", Role = "JobSeeker", Status = "Active", Joined = DateTime.Today.AddDays(-60) },
            new AppUser { Id = 3, Name = "Karim Uddin", Email = "karim@nexora.example", Role = "Recruiter", Status = "Active", Joined = DateTime.Today.AddDays(-200) },
            new AppUser { Id = 4, Name = "Lina Sarker", Email = "lina@bluewave.example", Role = "Recruiter", Status = "Active", Joined = DateTime.Today.AddDays(-150) },
            new AppUser { Id = 5, Name = "Rakib Hasan", Email = "rakib@orbitlog.example", Role = "Recruiter", Status = "Pending", Joined = DateTime.Today.AddDays(-2) },
            new AppUser { Id = 6, Name = "Tanvir Ahmed", Email = "tanvir.ahmed@mail.example", Role = "JobSeeker", Status = "Active", Joined = DateTime.Today.AddDays(-30) },
            new AppUser { Id = 7, Name = "Spam Account", Email = "spam123@mail.example", Role = "JobSeeker", Status = "Suspended", Joined = DateTime.Today.AddDays(-14) },
            new AppUser { Id = 8, Name = "Admin Root", Email = "admin@jobhub.example", Role = "Admin", Status = "Active", Joined = DateTime.Today.AddDays(-400) },
            new AppUser { Id = 9, Name = "Nusrat Jahan", Email = "nusrat.jahan@mail.example", Role = "JobSeeker", Status = "Active", Joined = DateTime.Today.AddDays(-7) },
        };

        public static readonly List<ReportItem> Reports = new()
        {
            new ReportItem { Id = 1, Type = "Job", Target = "Customer Support Specialist", ReportedBy = "Tanvir Ahmed", Reason = "Misleading salary information", Date = DateTime.Today.AddDays(-1), Status = "Open" },
            new ReportItem { Id = 2, Type = "Company", Target = "EduSpark", ReportedBy = "Nusrat Jahan", Reason = "Asked for payment before interview", Date = DateTime.Today.AddDays(-3), Status = "Reviewing" },
            new ReportItem { Id = 3, Type = "User", Target = "Spam Account", ReportedBy = "Karim Uddin", Reason = "Spam applications to many jobs", Date = DateTime.Today.AddDays(-5), Status = "Resolved" },
            new ReportItem { Id = 4, Type = "Job", Target = "Operations Coordinator", ReportedBy = "Ayesha Rahman", Reason = "Duplicate job post", Date = DateTime.Today.AddDays(-6), Status = "Open" },
            new ReportItem { Id = 5, Type = "Job", Target = "Marketing Intern", ReportedBy = "Maliha Chowdhury", Reason = "Not a real internship", Date = DateTime.Today.AddDays(-9), Status = "Dismissed" },
        };

        public static readonly List<ActivityItem> Activity = new()
        {
            new ActivityItem { Actor = "Rakib Hasan", Text = "registered a new recruiter account", Time = "10 min ago", Icon = "bi-person-plus", Tone = "primary" },
            new ActivityItem { Actor = "Nexora Tech", Text = "posted 'Senior Frontend Engineer'", Time = "1 hour ago", Icon = "bi-briefcase", Tone = "success" },
            new ActivityItem { Actor = "Tanvir Ahmed", Text = "reported the job 'Customer Support Specialist'", Time = "2 hours ago", Icon = "bi-flag", Tone = "danger" },
            new ActivityItem { Actor = "Admin Root", Text = "verified company 'Finlytics'", Time = "5 hours ago", Icon = "bi-patch-check", Tone = "info" },
            new ActivityItem { Actor = "Admin Root", Text = "suspended user 'Spam Account'", Time = "Yesterday", Icon = "bi-slash-circle", Tone = "warning" },
            new ActivityItem { Actor = "Sabbir Hasan", Text = "applied for '.NET Backend Developer'", Time = "Yesterday", Icon = "bi-send", Tone = "primary" },
            new ActivityItem { Actor = "BlueWave Studio", Text = "closed 'Brand Designer'", Time = "2 days ago", Icon = "bi-lock", Tone = "secondary" },
            new ActivityItem { Actor = "System", Text = "daily backup completed successfully", Time = "2 days ago", Icon = "bi-cloud-check", Tone = "success" },
        };

        public static readonly List<(string Name, string Icon)> CategoryIcons = new()
        {
            ("Engineering", "bi-code-slash"), ("Design", "bi-palette"), ("Data", "bi-bar-chart-line"), ("Marketing", "bi-megaphone"),
            ("Product", "bi-kanban"), ("Healthcare", "bi-heart-pulse"), ("Operations", "bi-gear"), ("Support", "bi-headset"),
        };

        // The recruiter demo account owns Nexora Tech
        public const int RecruiterCompanyId = 1;
    }
}

# HireLink – Job & Internship Platform

**HireLink** is a web-based job and internship platform developed using ASP.NET Core MVC. It connects job seekers with companies through a centralized platform where users can explore opportunities, apply for jobs, and manage recruitment activities.

The platform provides separate dashboards for Job Seekers, Companies, and Administrators to ensure role-based access and efficient management.

## User Roles

| Role | Responsibilities |
|---|---|
| Job Seeker | Browse jobs and manage applications |
| Company | Manage job posts and applicants |
| Admin | Monitor users and manage platform activities |

## Key Features

### Job Seeker
- Apply for jobs and track application status.

### Company
- Create and manage job postings.
- Review applicants and update application status.

### Admin
- Monitor active users, sessions, and login history.

## Technology Stack

**Backend**
- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server

**Frontend**
- HTML5 & CSS3
- Bootstrap & Bootstrap Icons
- JavaScript & jQuery

**Development Tools**
- Visual Studio
- SQL Server Management Studio (SSMS)

## Project Structure

```text
HireLink/
│
├── Controllers/
├── Models/
├── Views/
│   ├── Account/
│   ├── Admin/
│   ├── Company/
│   ├── Home/
│   └── JobSeeker/
│
├── Data/
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
│
├── appsettings.json
├── Program.cs
└── Job.csproj
```
## Project Architecture
- **Controllers:** Handle requests and application logic for Job Seekers, Companies, and Admins.
- **Models:** Define application entities such as Users, Job Posts, Job Applications.
- **Views:** Provide separate interfaces and dashboards for each user role.
- **EF:** Uses Entity Framework Core to manage database operations.
- **Database:** Microsoft SQL Server stores user, job, application, and login history data.

### Application Flow
**User → Controller → Model / Entity Framework Core → SQL Server → View → User**

### Prerequisites for this project
- Visual Studio
- SQL Server Management Studio (SSMS)

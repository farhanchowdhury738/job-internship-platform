using JobInternshipPlatform.Models;

namespace JobInternshipPlatform.Helpers
{
    public static class UiHelper
    {
        public static string StatusClass(ApplicationStatus s) => s switch
        {
            ApplicationStatus.Pending => "st-pending",
            ApplicationStatus.Reviewed => "st-reviewed",
            ApplicationStatus.Shortlisted => "st-shortlisted",
            ApplicationStatus.Interview => "st-interview",
            ApplicationStatus.Accepted => "st-accepted",
            ApplicationStatus.Rejected => "st-rejected",
            _ => "st-pending"
        };

        public static string StatusIcon(ApplicationStatus s) => s switch
        {
            ApplicationStatus.Pending => "bi-hourglass-split",
            ApplicationStatus.Reviewed => "bi-eye",
            ApplicationStatus.Shortlisted => "bi-star",
            ApplicationStatus.Interview => "bi-camera-video",
            ApplicationStatus.Accepted => "bi-check-circle",
            ApplicationStatus.Rejected => "bi-x-circle",
            _ => "bi-circle"
        };

        public static string GenericStatusClass(string s) => s switch
        {
            "Active" or "Verified" or "Resolved" => "st-accepted",
            "Pending" or "Open" or "Draft" => "st-pending",
            "Reviewing" => "st-interview",
            "Closed" or "Dismissed" => "st-reviewed",
            "Suspended" or "Flagged" => "st-rejected",
            _ => "st-reviewed"
        };
    }
}

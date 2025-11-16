namespace RIMS.Models.ViewModels
{
    public class IDRequestViewModel
    {
        public int ResidentId { get; set; }
        public string IDType { get; set; } = "Resident ID";
        public string Purpose { get; set; } = string.Empty;
    }

    public class IDApplicationViewModel
    {
        public int Id { get; set; }
        public string ResidentName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public string IDType { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
        public string IDNumber { get; set; } = string.Empty;
    }

    public class IDStatusViewModel
    {
        public bool HasID { get; set; }
        public string Status { get; set; } = string.Empty;
        public string IDNumber { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
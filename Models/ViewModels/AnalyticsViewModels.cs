using System;
using System.Collections.Generic;

namespace RIMS.Models.ViewModels
{
    public class AnalyticsViewModel
    {
        public int TotalResidents { get; set; }
        public int TotalDocuments { get; set; }
        public int PendingApplications { get; set; }
        public int CompletedApplications { get; set; }
        public int NewResidentsThisMonth { get; set; }
        public int DocumentsIssuedThisMonth { get; set; }
        public decimal CompletionRate { get; set; }

        // Distributions
        public Dictionary<string, int> AgeGroupDistribution { get; set; } = new();
        public Dictionary<string, int> GenderDistribution { get; set; } = new();
        public Dictionary<string, int> DocumentTypeDistribution { get; set; } = new();
        public Dictionary<string, int> ApplicationStatusDistribution { get; set; } = new();

        // Trends
        public List<MonthlyTrend> MonthlyResidentTrend { get; set; } = new();
        public List<MonthlyTrend> MonthlyDocumentTrend { get; set; } = new();
        public List<WeeklyTrend> WeeklyApplicationTrend { get; set; } = new();

        // Top data
        public List<PopularDocument> PopularDocuments { get; set; } = new();
        public List<ActiveResident> ActiveResidents { get; set; } = new();
    }

    public class MonthlyTrend
    {
        public string Month { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public int ResidentCount { get; set; }
        public int DocumentCount { get; set; }
    }

    public class WeeklyTrend
    {
        public string Week { get; set; } = string.Empty;
        public int ApplicationCount { get; set; }
        public int CompletedCount { get; set; }
    }

    public class PopularDocument
    {
        public string DocumentName { get; set; } = string.Empty;
        public int ApplicationCount { get; set; }
        public decimal CompletionRate { get; set; }
    }

    public class ActiveResident
    {
        public string ResidentName { get; set; } = string.Empty;
        public int ApplicationCount { get; set; }
        public DateTime LastApplicationDate { get; set; }
    }
}
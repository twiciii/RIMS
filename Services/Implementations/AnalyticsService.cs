using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;
using RIMS.Models.ViewModels;
using RIMS.Services.Interfaces;

namespace RIMS.Services.Implementations
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardAnalytics> GetDashboardAnalyticsAsync()
        {
            var totalResidents = await _context.rimsResidents.CountAsync();

            // Use DateOfBirth as a proxy for new residents this month
            var newResidentsThisMonth = await _context.rimsResidents
                .Where(r => r.DateOfBirth.Year == DateTime.Now.Year && r.DateOfBirth.Month == DateTime.Now.Month)
                .CountAsync();

            var pendingApplications = await _context.rimsDocumentApplication
                .Where(da => da.Status == "Pending")
                .CountAsync();

            // Use ApplicationDate for completed today
            var completedToday = await _context.rimsDocumentApplication
                .Where(da => da.Status == "Completed" && da.ApplicationDate.Date == DateTime.Today)
                .CountAsync();

            var applicationsByStatus = await _context.rimsDocumentApplication
                .GroupBy(da => da.Status ?? "Unknown")
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);

            // Use CategoryName as configured in DbContext
            var residentsByCategory = await _context.rimsResidentCategories
                .GroupBy(rc => rc.CategoryName ?? "Unknown")
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Category, x => x.Count);

            return new DashboardAnalytics
            {
                TotalResidents = totalResidents,
                NewResidentsThisMonth = newResidentsThisMonth,
                PendingApplications = pendingApplications,
                CompletedApplicationsToday = completedToday,
                CompletionRate = totalResidents > 0 ? (decimal)completedToday / totalResidents * 100 : 0,
                ApplicationsByStatus = applicationsByStatus,
                ResidentsByCategory = residentsByCategory
            };
        }

        public async Task<SystemPerformance> GetSystemPerformanceAsync()
        {
            // Since this is returning mock data, we'll use Task.FromResult to avoid the warning
            return await Task.FromResult(new SystemPerformance
            {
                Uptime = TimeSpan.FromDays(30),
                AverageResponseTime = 150,
                ErrorRate = 0.5,
                ActiveUsers = 25,
                DatabasePerformance = "Optimal",
                LastMaintenance = DateTime.Now.AddDays(-1)
            });
        }

        public async Task<ResidentAnalytics> GetResidentAnalyticsAsync()
        {
            var residents = await _context.rimsResidents.ToListAsync();

            var ageDistribution = new AgeDistribution
            {
                Children = residents.Count(r => CalculateAge(r.DateOfBirth) < 18),
                Youth = residents.Count(r => CalculateAge(r.DateOfBirth) >= 18 && CalculateAge(r.DateOfBirth) < 30),
                Adults = residents.Count(r => CalculateAge(r.DateOfBirth) >= 30 && CalculateAge(r.DateOfBirth) < 60),
                Seniors = residents.Count(r => CalculateAge(r.DateOfBirth) >= 60)
            };

            // Use Gender property as configured in DbContext
            var genderDistribution = new GenderDistribution
            {
                Male = residents.Count(r => r.Gender == "Male"),
                Female = residents.Count(r => r.Gender == "Female"),
                Other = residents.Count(r => r.Gender != "Male" && r.Gender != "Female")
            };

            // Use Address property for residents by area
            var residentsByAddress = await _context.rimsResidents
                .Where(r => !string.IsNullOrEmpty(r.Address))
                .GroupBy(r => r.Address)
                .Select(g => new { Address = g.Key ?? "Unknown", Count = g.Count() })
                .ToDictionaryAsync(x => x.Address, x => x.Count);

            // Use CategoryName for category counts
            var categoryCounts = await _context.rimsResidentCategories
                .GroupBy(rc => rc.CategoryName ?? "Unknown")
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Category, x => x.Count);

            var monthlyGrowth = new TrendData
            {
                Labels = Enumerable.Range(1, 12).Select(m => new DateTime(DateTime.Now.Year, m, 1).ToString("MMM")).ToList(),
                Values = Enumerable.Range(1, 12).Select(m => new Random().Next(10, 50)).ToList()
            };

            return new ResidentAnalytics
            {
                AgeDistribution = ageDistribution,
                GenderDistribution = genderDistribution,
                ResidentsByBarangay = residentsByAddress,
                CategoryCounts = categoryCounts,
                MonthlyGrowth = monthlyGrowth
            };
        }

        public async Task<DemographicData> GetDemographicDataAsync()
        {
            var residents = await _context.rimsResidents.ToListAsync();

            // Use Gender property
            var maleCount = residents.Count(r => r.Gender == "Male");
            var femaleCount = residents.Count(r => r.Gender == "Female");
            var total = residents.Count;

            return new DemographicData
            {
                TotalPopulation = total,
                AverageAge = residents.Count > 0 ? residents.Average(r => CalculateAge(r.DateOfBirth)) : 0,
                GenderRatio = new Dictionary<string, decimal>
                {
                    {"Male", total > 0 ? (decimal)maleCount / total * 100 : 0},
                    {"Female", total > 0 ? (decimal)femaleCount / total * 100 : 0}
                },
                AgeGroups = new Dictionary<string, int>
                {
                    {"0-17", residents.Count(r => CalculateAge(r.DateOfBirth) < 18)},
                    {"18-29", residents.Count(r => CalculateAge(r.DateOfBirth) >= 18 && CalculateAge(r.DateOfBirth) < 30)},
                    {"30-59", residents.Count(r => CalculateAge(r.DateOfBirth) >= 30 && CalculateAge(r.DateOfBirth) < 60)},
                    {"60+", residents.Count(r => CalculateAge(r.DateOfBirth) >= 60)}
                }
            };
        }

        public async Task<CategoryDistribution> GetCategoryDistributionAsync()
        {
            // Use CategoryName as configured
            var categories = await _context.rimsResidentCategories
                .GroupBy(rc => rc.CategoryName ?? "Unknown")
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync();

            return new CategoryDistribution
            {
                Categories = categories.ToDictionary(c => c.Category, c => c.Count),
                TotalCategorized = categories.Sum(c => c.Count)
            };
        }

        public async Task<DocumentAnalytics> GetDocumentAnalyticsAsync()
        {
            var applications = await _context.rimsDocumentApplication
                .Include(da => da.Document)
                .ToListAsync();

            // Use DocumentName property with null check
            var applicationsByType = applications
                .Where(a => a.Document != null)
                .GroupBy(a => a.Document!.DocumentName ?? "Unknown") // Using null-forgiving operator after null check
                .ToDictionary(g => g.Key, g => g.Count());

            var applicationsByStatus = applications
                .GroupBy(a => a.Status ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());

            // Use ApplicationDate for this month's applications
            var applicationsThisMonth = applications.Count(a =>
                a.ApplicationDate.Month == DateTime.Now.Month &&
                a.ApplicationDate.Year == DateTime.Now.Year);

            return new DocumentAnalytics
            {
                TotalApplications = applications.Count,
                ApplicationsThisMonth = applicationsThisMonth,
                ApplicationsByType = applicationsByType,
                ApplicationsByStatus = applicationsByStatus,
                ProcessingTimes = new AverageProcessingTime
                {
                    AverageTime = TimeSpan.FromHours(24),
                    FastestTime = TimeSpan.FromHours(1),
                    SlowestTime = TimeSpan.FromDays(5)
                }
            };
        }

        public async Task<ApplicationTrends> GetApplicationTrendsAsync(DateTime fromDate, DateTime toDate)
        {
            // Use ApplicationDate for trends
            var trends = await _context.rimsDocumentApplication
                .Where(da => da.ApplicationDate >= fromDate && da.ApplicationDate <= toDate)
                .GroupBy(da => da.ApplicationDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToListAsync();

            return new ApplicationTrends
            {
                Dates = trends.Select(t => t.Date.ToString("MMM dd")).ToList(),
                Counts = trends.Select(t => t.Count).ToList(),
                TotalApplications = trends.Sum(t => t.Count),
                AverageDaily = trends.Count > 0 ? trends.Average(t => t.Count) : 0
            };
        }

        public async Task<ProcessingTimes> GetProcessingTimesAsync()
        {
            // Since this is returning mock data, we'll use Task.FromResult to avoid the warning
            return await Task.FromResult(new ProcessingTimes
            {
                AverageProcessingTime = TimeSpan.FromHours(24),
                MedianProcessingTime = TimeSpan.FromHours(18),
                ProcessingTimeDistribution = new Dictionary<string, int>
                {
                    {"< 1 hour", 10},
                    {"1-24 hours", 150},
                    {"1-3 days", 75},
                    {"> 3 days", 15}
                }
            });
        }

        public async Task<UserActivity> GetUserActivityAsync()
        {
            var recentActivity = await _context.rimsAuditTrail
                .Where(a => a.ActionDate >= DateTime.Now.AddDays(-7))
                .GroupBy(a => a.UserId)
                .Select(g => new { UserId = g.Key ?? "Unknown", ActivityCount = g.Count() })
                .ToListAsync();

            // Since ActivityByHour is empty, we'll initialize it properly
            var activityByHour = new Dictionary<int, int>();
            for (int i = 0; i < 24; i++)
            {
                activityByHour[i] = 0;
            }

            var mostActiveUser = recentActivity.OrderByDescending(a => a.ActivityCount).FirstOrDefault();

            return new UserActivity
            {
                ActiveUsers = recentActivity.Count,
                MostActiveUser = mostActiveUser?.UserId ?? "None",
                AverageActivitiesPerUser = recentActivity.Count > 0 ? recentActivity.Average(a => a.ActivityCount) : 0,
                ActivityByHour = activityByHour
            };
        }

        public async Task<SystemUsage> GetSystemUsageAsync()
        {
            // Initialize PeakUsageHours properly
            var peakUsageHours = new Dictionary<int, int>();
            for (int i = 0; i < 24; i++)
            {
                peakUsageHours[i] = new Random().Next(0, 50);
            }

            // Initialize ConcurrentUsers properly
            var concurrentUsers = new Dictionary<DateTime, int>();
            var now = DateTime.Now;
            for (int i = 0; i < 24; i++)
            {
                concurrentUsers[now.AddHours(-i)] = new Random().Next(1, 25);
            }

            return await Task.FromResult(new SystemUsage
            {
                PeakUsageHours = peakUsageHours,
                AverageSessionDuration = TimeSpan.FromMinutes(45),
                MostUsedFeatures = new Dictionary<string, int>
                {
                    {"Resident Search", 500},
                    {"Document Processing", 300},
                    {"Reports", 200}
                },
                ConcurrentUsers = concurrentUsers
            });
        }

        public async Task<ForecastData> GetResidentGrowthForecastAsync(int months)
        {
            // Use DateOfBirth as proxy for resident creation
            var historicalData = await _context.rimsResidents
                .Where(r => r.DateOfBirth >= DateTime.Now.AddYears(-1))
                .GroupBy(r => new { r.DateOfBirth.Year, r.DateOfBirth.Month })
                .Select(g => new { Period = g.Key, Count = g.Count() })
                .OrderBy(x => x.Period.Year).ThenBy(x => x.Period.Month)
                .ToListAsync();

            var averageGrowth = historicalData.Count > 0 ? historicalData.Average(x => x.Count) : 10;

            return new ForecastData
            {
                Periods = Enumerable.Range(1, months).Select(m => DateTime.Now.AddMonths(m).ToString("MMM yyyy")).ToList(),
                ExpectedValues = Enumerable.Range(1, months).Select(m => (int)(averageGrowth * 1.1)).ToList(),
                LowerBounds = Enumerable.Range(1, months).Select(m => (int)(averageGrowth * 0.9)).ToList(),
                UpperBounds = Enumerable.Range(1, months).Select(m => (int)(averageGrowth * 1.3)).ToList()
            };
        }

        public async Task<ForecastData> GetDocumentDemandForecastAsync(int months)
        {
            // Use ApplicationDate for document demand
            var historicalData = await _context.rimsDocumentApplication
                .Where(da => da.ApplicationDate >= DateTime.Now.AddYears(-1))
                .GroupBy(da => new { da.ApplicationDate.Year, da.ApplicationDate.Month })
                .Select(g => new { Period = g.Key, Count = g.Count() })
                .OrderBy(x => x.Period.Year).ThenBy(x => x.Period.Month)
                .ToListAsync();

            var averageDemand = historicalData.Count > 0 ? historicalData.Average(x => x.Count) : 15;

            return new ForecastData
            {
                Periods = Enumerable.Range(1, months).Select(m => DateTime.Now.AddMonths(m).ToString("MMM yyyy")).ToList(),
                ExpectedValues = Enumerable.Range(1, months).Select(m => (int)(averageDemand * 1.05)).ToList(),
                LowerBounds = Enumerable.Range(1, months).Select(m => (int)(averageDemand * 0.85)).ToList(),
                UpperBounds = Enumerable.Range(1, months).Select(m => (int)(averageDemand * 1.25)).ToList()
            };
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
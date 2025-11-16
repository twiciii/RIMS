using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;
using RIMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace RIMS.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateResidentListReportAsync(ReportParameters parameters)
        {
            var residents = await _context.rimsResidents.ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .AlignCenter()
                        .Text("Resident List Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken3);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            // Report Info
                            column.Item().Text($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                            column.Item().Text($"Total Residents: {residents.Count}");

                            // Table Header
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1); // ID
                                    columns.RelativeColumn(2); // First Name
                                    columns.RelativeColumn(2); // Last Name
                                    columns.RelativeColumn(2); // Date of Birth
                                    columns.RelativeColumn(2); // Gender
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("First Name").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Last Name").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Date of Birth").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Gender").SemiBold();
                                });

                                foreach (var resident in residents)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.Id.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.FirstName ?? string.Empty);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.LastName ?? string.Empty);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.DateOfBirth.ToString("yyyy-MM-dd"));
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.Gender ?? string.Empty);
                                }
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateResidentDemographicsReportAsync(ReportParameters parameters)
        {
            var residents = await _context.rimsResidents.ToListAsync();

            var ageGroups = new Dictionary<string, int>
            {
                {"0-17", residents.Count(r => CalculateAge(r.DateOfBirth) < 18)},
                {"18-29", residents.Count(r => CalculateAge(r.DateOfBirth) >= 18 && CalculateAge(r.DateOfBirth) < 30)},
                {"30-59", residents.Count(r => CalculateAge(r.DateOfBirth) >= 30 && CalculateAge(r.DateOfBirth) < 60)},
                {"60+", residents.Count(r => CalculateAge(r.DateOfBirth) >= 60)}
            };

            var genderGroups = residents
                .Where(r => !string.IsNullOrEmpty(r.Gender))
                .GroupBy(r => r.Gender!)
                .ToDictionary(g => g.Key, g => g.Count());

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);

                    page.Header()
                        .AlignCenter()
                        .Text("Resident Demographics Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken3);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(15);

                            // Age Distribution
                            column.Item().Text("Age Distribution").SemiBold().FontSize(16);
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Age Group").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Count").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Percentage").SemiBold();
                                });

                                foreach (var group in ageGroups)
                                {
                                    var percentage = residents.Count > 0 ? (double)group.Value / residents.Count * 100 : 0;
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(group.Key);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(group.Value.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{percentage:F1}%");
                                }
                            });

                            // Gender Distribution
                            column.Item().Text("Gender Distribution").SemiBold().FontSize(16);
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Gender").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Count").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Percentage").SemiBold();
                                });

                                foreach (var group in genderGroups)
                                {
                                    var percentage = residents.Count > 0 ? (double)group.Value / residents.Count * 100 : 0;
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(group.Key);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(group.Value.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{percentage:F1}%");
                                }
                            });
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateResidentCategoryReportAsync(ReportParameters parameters)
        {
            var categories = await _context.rimsResidentCategories
                .GroupBy(rc => rc.CategoryName ?? "Unknown")
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.Header()
                        .AlignCenter()
                        .Text("Resident Categories Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken3);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Category").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Count").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Percentage").SemiBold();
                                });

                                int total = categories.Sum(c => c.Count);
                                foreach (var category in categories)
                                {
                                    var percentage = total > 0 ? (double)category.Count / total * 100 : 0;
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(category.Category);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(category.Count.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{percentage:F1}%");
                                }
                            });
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateDocumentIssuanceReportAsync(ReportParameters parameters)
        {
            var applications = await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da => da.Status == "Completed")
                .ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);

                    page.Header()
                        .AlignCenter()
                        .Text("Document Issuance Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken3);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1); // ID
                                    columns.RelativeColumn(3); // Resident Name
                                    columns.RelativeColumn(2); // Document Type
                                    columns.RelativeColumn(2); // Date
                                    columns.RelativeColumn(2); // Purpose
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("App ID").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Resident Name").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Document Type").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Issue Date").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Purpose").SemiBold();
                                });

                                foreach (var app in applications)
                                {
                                    var residentName = $"{app.Resident?.FirstName ?? "Unknown"} {app.Resident?.LastName ?? ""}";
                                    var documentName = app.Document?.DocumentName ?? "Unknown";
                                    var purpose = app.Purpose ?? string.Empty;

                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(app.Id.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(residentName.Trim());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(documentName);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(app.CreatedDate.ToString("yyyy-MM-dd"));
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(purpose);
                                }
                            });
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateApplicationStatusReportAsync(ReportParameters parameters)
        {
            var statusCounts = await _context.rimsDocumentApplication
                .GroupBy(da => da.Status ?? "Unknown")
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.Header()
                        .AlignCenter()
                        .Text("Application Status Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken3);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Status").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Count").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Percentage").SemiBold();
                                });

                                int total = statusCounts.Sum(s => s.Count);
                                foreach (var status in statusCounts)
                                {
                                    var percentage = total > 0 ? (double)status.Count / total * 100 : 0;
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(status.Status);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(status.Count.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{percentage:F1}%");
                                }
                            });
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateCertificateReportAsync(ReportParameters parameters)
        {
            var certificates = await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da => da.Document != null && da.Document.DocumentName != null && da.Document.DocumentName.Contains("Certificate"))
                .Where(da => da.Status == "Completed")
                .ToListAsync();

            // Reuse the document issuance report logic but with certificate data
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);

                    page.Header()
                        .AlignCenter()
                        .Text("Certificate Issuance Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken3);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1); // ID
                                    columns.RelativeColumn(3); // Resident Name
                                    columns.RelativeColumn(2); // Certificate Type
                                    columns.RelativeColumn(2); // Date
                                    columns.RelativeColumn(2); // Purpose
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("App ID").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Resident Name").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Certificate Type").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Issue Date").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Purpose").SemiBold();
                                });

                                foreach (var app in certificates)
                                {
                                    var residentName = $"{app.Resident?.FirstName ?? "Unknown"} {app.Resident?.LastName ?? ""}";
                                    var documentName = app.Document?.DocumentName ?? "Unknown Certificate";
                                    var purpose = app.Purpose ?? string.Empty;

                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(app.Id.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(residentName.Trim());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(documentName);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(app.CreatedDate.ToString("yyyy-MM-dd"));
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(purpose);
                                }
                            });
                        });
                });
            });

            return document.GeneratePdf();
        }

        public Task<byte[]> GenerateMonthlyStatisticsReportAsync(DateTime month)
        {
            // Implementation for monthly statistics
            // For now, return an empty byte array as placeholder
            return Task.FromResult(new byte[0]);
        }

        public Task<byte[]> GenerateYearlyReportAsync(int year)
        {
            // Implementation for yearly report
            // For now, return an empty byte array as placeholder
            return Task.FromResult(new byte[0]);
        }

        public Task<byte[]> GenerateAuditReportAsync(ReportParameters parameters)
        {
            // Implementation for audit report
            // For now, return an empty byte array as placeholder
            return Task.FromResult(new byte[0]);
        }

        public Task<byte[]> GenerateCustomReportAsync(CustomReportRequest request)
        {
            throw new NotImplementedException("Custom report generation not implemented");
        }

        public Task<IEnumerable<ReportTemplate>> GetReportTemplatesAsync()
        {
            var templates = new List<ReportTemplate>
            {
                new ReportTemplate
                {
                    Id = 1,
                    Name = "Resident List",
                    Description = "Complete list of all residents",
                    TemplateType = "Standard",
                    CreatedDate = DateTime.Now.AddDays(-30),
                    CreatedBy = "system"
                },
                new ReportTemplate
                {
                    Id = 2,
                    Name = "Document Statistics",
                    Description = "Monthly document issuance statistics",
                    TemplateType = "Standard",
                    CreatedDate = DateTime.Now.AddDays(-20),
                    CreatedBy = "system"
                }
            };

            return Task.FromResult<IEnumerable<ReportTemplate>>(templates);
        }

        public Task SaveReportTemplateAsync(ReportTemplate template)
        {
            // Implementation for saving template
            return Task.CompletedTask;
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    // Model classes with proper null safety
    public class ReportParameters
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? DocumentType { get; set; }
        public string? Status { get; set; }
        public string? ResidentCategory { get; set; }
    }

    public class CustomReportRequest
    {
        public string Query { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new();
    }

    public class ReportTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TemplateType { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    public interface IReportService
    {
        Task<byte[]> GenerateResidentListReportAsync(ReportParameters parameters);
        Task<byte[]> GenerateResidentDemographicsReportAsync(ReportParameters parameters);
        Task<byte[]> GenerateResidentCategoryReportAsync(ReportParameters parameters);
        Task<byte[]> GenerateDocumentIssuanceReportAsync(ReportParameters parameters);
        Task<byte[]> GenerateApplicationStatusReportAsync(ReportParameters parameters);
        Task<byte[]> GenerateCertificateReportAsync(ReportParameters parameters);
        Task<byte[]> GenerateMonthlyStatisticsReportAsync(DateTime month);
        Task<byte[]> GenerateYearlyReportAsync(int year);
        Task<byte[]> GenerateAuditReportAsync(ReportParameters parameters);
        Task<byte[]> GenerateCustomReportAsync(CustomReportRequest request);
        Task<IEnumerable<ReportTemplate>> GetReportTemplatesAsync();
        Task SaveReportTemplateAsync(ReportTemplate template);
    }
}
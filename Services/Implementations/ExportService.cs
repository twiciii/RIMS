using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RIMS.Data;
using RIMS.Models;
using RIMS.Models.Entities;
using System.Text;
using System.Text.Json;
using RIMS.Services.Interfaces;

namespace RIMS.Services.Implementations
{
    public class ExportService : IExportService
    {
        private readonly ApplicationDbContext _context;

        public ExportService(ApplicationDbContext context)
        {
            _context = context;
            // Set QuestPDF license if needed (Community license is free)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> ExportResidentsToExcelAsync(ExportParameters parameters)
        {
            var residents = await GetResidentsForExportAsync(parameters);

            // Generate PDF document (QuestPDF generates synchronously)
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .AlignCenter()
                        .Text("Residents Report")
                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50); // ID
                                columns.ConstantColumn(80); // First Name
                                columns.ConstantColumn(80); // Middle Name
                                columns.ConstantColumn(80); // Last Name
                                columns.ConstantColumn(80); // Date of Birth
                                columns.ConstantColumn(60); // Gender
                                columns.RelativeColumn();   // Address
                                columns.ConstantColumn(100); // Email
                                columns.ConstantColumn(80); // Phone
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("ID").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("First Name").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Middle Name").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Last Name").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Date of Birth").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Gender").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Address").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Email").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Phone").SemiBold();
                            });

                            foreach (var resident in residents)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.FirstName ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.MiddleName ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.LastName ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.DateOfBirth.ToString("yyyy-MM-dd"));
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.Sex ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.Address ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(resident.Email ?? "");
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                            x.Span($" | Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                        });
                });
            });

            // Run PDF generation on background thread to avoid blocking
            return await Task.Run(() => document.GeneratePdf());
        }

        public async Task<byte[]> ExportResidentsToPdfAsync(ExportParameters parameters)
        {
            var residents = await GetResidentsForExportAsync(parameters);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .AlignCenter()
                        .Text("Residents Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            foreach (var resident in residents)
                            {
                                column.Item().Background(Colors.Grey.Lighten4).Padding(10).Column(residentColumn =>
                                {
                                    residentColumn.Item().Text($"{resident.FirstName} {resident.MiddleName} {resident.LastName}").SemiBold();
                                    residentColumn.Item().Text($"Date of Birth: {resident.DateOfBirth:yyyy-MM-dd}");
                                    residentColumn.Item().Text($"Gender: {resident.Sex}");
                                    residentColumn.Item().Text($"Address: {resident.Address}");
                                    residentColumn.Item().Text($"Email: {resident.Email}");
                                });

                                column.Item().Height(10);
                            }
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

            return await Task.Run(() => document.GeneratePdf());
        }

        public async Task<byte[]> ExportDocumentsToExcelAsync(ExportParameters parameters)
        {
            var documents = await _context.rimsDocuments.ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .AlignCenter()
                        .Text("Documents Report")
                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50); // ID
                                columns.RelativeColumn(2);  // Document Name
                                columns.RelativeColumn(3);  // File Path
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("ID").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Document Name").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("File Path").SemiBold();
                            });

                            foreach (var doc in documents)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(doc.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(doc.DocumentName ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(doc.FilePath ?? "");
                            }
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

            return await Task.Run(() => document.GeneratePdf());
        }

        public async Task<byte[]> ExportApplicationsToExcelAsync(ExportParameters parameters)
        {
            var applications = await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .AlignCenter()
                        .Text("Document Applications Report")
                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(80); // Application ID
                                columns.RelativeColumn(2);  // Resident Name
                                columns.RelativeColumn(2);  // Document Type
                                columns.ConstantColumn(80); // Application Date
                                columns.ConstantColumn(60); // Status
                                columns.RelativeColumn(3);  // Purpose
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Application ID").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Resident Name").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Document Type").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Application Date").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Status").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Purpose").SemiBold();
                            });

                            foreach (var app in applications)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(app.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{app.Resident?.FirstName} {app.Resident?.LastName}");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(app.Document?.DocumentName ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(app.ApplicationDate.ToString("yyyy-MM-dd"));
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(app.Status ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(app.Purpose ?? "");
                            }
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

            return await Task.Run(() => document.GeneratePdf());
        }

        public async Task<byte[]> ExportAuditLogsToExcelAsync(ExportParameters parameters)
        {
            var auditLogs = await _context.rimsAuditTrail
                .OrderByDescending(a => a.ActionDate)
                .Take(1000)
                .ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header()
                        .AlignCenter()
                        .Text("Audit Logs Report")
                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(60); // Audit ID
                                columns.ConstantColumn(80); // Action Type
                                columns.ConstantColumn(80); // User ID
                                columns.ConstantColumn(100); // Action Date
                                columns.ConstantColumn(80); // IP Address
                                columns.ConstantColumn(60); // Status
                                columns.RelativeColumn(2);  // Remarks
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Audit ID").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Action Type").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("User ID").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Action Date").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("IP Address").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Status").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Remarks").SemiBold();
                            });

                            foreach (var log in auditLogs)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(log.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(log.ActionType ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(log.UserId ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(log.ActionDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(log.IpAddress ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(log.ActionStatus ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(log.Remarks ?? "");
                            }
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

            return await Task.Run(() => document.GeneratePdf());
        }

        public async Task<byte[]> ExportToCsvAsync<T>(IEnumerable<T> data, string fileName)
        {
            // Run CSV generation on background thread
            return await Task.Run(() =>
            {
                var csv = new StringBuilder();

                var properties = typeof(T).GetProperties();
                var header = string.Join(",", properties.Select(p => p.Name));
                csv.AppendLine(header);

                foreach (var item in data)
                {
                    var row = string.Join(",", properties.Select(p =>
                    {
                        var value = p.GetValue(item);
                        return value?.ToString()?.Replace(",", ";") ?? "";
                    }));
                    csv.AppendLine(row);
                }

                return Encoding.UTF8.GetBytes(csv.ToString());
            });
        }

        public async Task<byte[]> ExportToJsonAsync<T>(IEnumerable<T> data, string fileName)
        {
            // Run JSON serialization on background thread
            return await Task.Run(() =>
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(data, options);
                return Encoding.UTF8.GetBytes(json);
            });
        }

        public Task<string> GenerateExportTemplateAsync(string templateType)
        {
            // This method doesn't do any async work, so remove async and return Task.FromResult
            var result = templateType switch
            {
                "ResidentList" => "Resident List Template",
                "DocumentReport" => "Document Report Template",
                "AuditLogs" => "Audit Logs Template",
                _ => "Default Template"
            };
            return Task.FromResult(result);
        }

        public Task<ExportHistory> LogExportAsync(ExportLog log)
        {
            // This method doesn't do any async work, so remove async and return Task.FromResult
            var result = new ExportHistory
            {
                TotalExports = 1,
                RecentExports = new List<ExportLog> { log },
                ExportsByType = new Dictionary<string, int> { { log.ExportType, 1 } }
            };
            return Task.FromResult(result);
        }

        public Task<IEnumerable<ExportLog>> GetExportHistoryAsync()
        {
            // This method doesn't do any async work, so remove async and return Task.FromResult
            var result = new List<ExportLog>
            {
                new ExportLog
                {
                    Id = 1,
                    ExportType = "Residents",
                    Format = "PDF",
                    RecordCount = 150,
                    ExportedBy = "admin",
                    ExportDate = DateTime.Now.AddHours(-1),
                    FileName = "residents_export.pdf"
                }
            };
            return Task.FromResult<IEnumerable<ExportLog>>(result);
        }

        private async Task<List<RIMSResident>> GetResidentsForExportAsync(ExportParameters parameters)
        {
            var query = _context.rimsResidents.AsQueryable();

            if (parameters.Ids?.Any() == true)
            {
                query = query.Where(r => parameters.Ids.Contains(r.Id));
            }

            if (parameters.StartDate.HasValue)
            {
                query = query.Where(r => r.DateOfBirth >= parameters.StartDate.Value);
            }

            if (parameters.EndDate.HasValue)
            {
                query = query.Where(r => r.DateOfBirth <= parameters.EndDate.Value);
            }

            return await query.ToListAsync();
        }
    }
}
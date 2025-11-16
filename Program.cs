using Microsoft.EntityFrameworkCore;
using RIMS.Configurations;
using RIMS.Data;
using RIMS.Middleware;
using RIMS.Repositories; // Add this using
using RIMS.Services;
using RIMS.Services.Implementations;
using RIMS.Services.Interfaces;
using RIMS.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Configurations
builder.Services.Configure<ApplicationConfig>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.Configure<CacheConfig>(builder.Configuration.GetSection("CacheSettings"));
builder.Services.Configure<PdfConfig>(builder.Configuration.GetSection("PdfSettings"));
builder.Services.Configure<FileStorageConfig>(builder.Configuration.GetSection("FileStorageSettings"));

// MVC
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Repositories - ADD THESE
builder.Services.AddScoped<IResidentRepository, ResidentRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
// Add other repositories as needed...

// Services
builder.Services.AddScoped<IResidentService, ResidentService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ISystemService, SystemService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Utilities
builder.Services.AddScoped<IPdfGenerator, PdfGenerator>();
builder.Services.AddScoped<IFileStorage, FileStorage>();

// Memory Cache
builder.Services.AddMemoryCache();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "BarangayRIMS.Session";
});

// Middleware
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseMiddleware<NotificationMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
using System;
using Microsoft.Extensions.Options;
using RIMS.Configurations;
using RIMS.Models.Entities;

namespace RIMS.Utilities
{
    // NOTE:
    // This implementation is a fallback placeholder that does not depend on QuestPDF.
    // It returns an empty PDF placeholder byte[] so project builds without adding external NuGet packages.
    // For real PDF generation install QuestPDF (NuGet) and replace this implementation accordingly.
    public class PdfGenerator : IPdfGenerator
    {
        private readonly PdfConfig _pdfConfig;
        private readonly ApplicationConfig _appConfig;

        public PdfGenerator(IOptions<PdfConfig> pdfConfig, IOptions<ApplicationConfig> appConfig)
        {
            _pdfConfig = pdfConfig?.Value ?? new PdfConfig();
            _appConfig = appConfig?.Value ?? new ApplicationConfig();
        }

        public byte[] GenerateResidentCertificate(RIMSResident resident, string certificateType)
        {
            if (resident is null) throw new ArgumentNullException(nameof(resident));

            // Minimal placeholder PDF bytes (empty array). Replace with a proper PDF generator.
            // If you want real PDFs, add QuestPDF via NuGet and restore the previous implementation.
            return Array.Empty<byte>();
        }
    }
}
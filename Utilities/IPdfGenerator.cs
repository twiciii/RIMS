using RIMS.Models.Entities;

namespace RIMS.Utilities
{
    public interface IPdfGenerator
    {
        // Produce a PDF byte array for the given resident and certificate type.
        // Implementation can use QuestPDF or any other PDF library.
        byte[] GenerateResidentCertificate(RIMSResident resident, string certificateType);
    }
}
namespace RIMS.Utilities
{
    public interface IFileStorage
    {
        Task<string> SaveResidentPhotoAsync(Stream fileStream, string fileName, int residentId);
        Task<string> SaveDocumentAttachmentAsync(Stream fileStream, string fileName, int applicationId);
        Task<Stream> GetFileAsync(string filePath);
        Task<bool> DeleteFileAsync(string filePath);
        Task<List<string>> GetResidentFilesAsync(int residentId);
        Task<string> CreateBackupAsync();
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace RIMS.Utilities
{
    public class FileStorage : IFileStorage
    {
        private readonly string _basePath;
        private readonly ILogger<FileStorage> _logger;

        public FileStorage(IWebHostEnvironment environment, ILogger<FileStorage> logger)
        {
            _basePath = Path.Combine(environment.WebRootPath, "uploads");
            _logger = logger;

            // Ensure directories exist
            Directory.CreateDirectory(Path.Combine(_basePath, "photos"));
            Directory.CreateDirectory(Path.Combine(_basePath, "documents"));
            Directory.CreateDirectory(Path.Combine(_basePath, "backups"));
        }

        public async Task<string> SaveResidentPhotoAsync(Stream fileStream, string fileName, int residentId)
        {
            var safeFileName = GetSafeFileName(fileName);
            var directory = Path.Combine(_basePath, "photos", residentId.ToString());
            Directory.CreateDirectory(directory);

            var filePath = Path.Combine(directory, safeFileName);

            using var file = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(file);

            _logger.LogInformation("Saved resident photo: {FilePath}", filePath);
            return filePath;
        }

        public async Task<string> SaveDocumentAttachmentAsync(Stream fileStream, string fileName, int applicationId)
        {
            var safeFileName = GetSafeFileName(fileName);
            var directory = Path.Combine(_basePath, "documents", applicationId.ToString());
            Directory.CreateDirectory(directory);

            var filePath = Path.Combine(directory, safeFileName);

            using var file = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(file);

            _logger.LogInformation("Saved document attachment: {FilePath}", filePath);
            return filePath;
        }

        public async Task<Stream> GetFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            // Use async file opening to avoid blocking
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
            await Task.Yield(); // Yield to prevent synchronous completion
            return fileStream;
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
                _logger.LogInformation("Deleted file: {FilePath}", filePath);
                return true;
            }

            return false;
        }

        public async Task<List<string>> GetResidentFilesAsync(int residentId)
        {
            var directory = Path.Combine(_basePath, "photos", residentId.ToString());
            if (!Directory.Exists(directory))
                return new List<string>();

            // Use Task.Run to move file system operations to background thread
            var files = await Task.Run(() =>
                Directory.GetFiles(directory)
                    .Select(Path.GetFileName)
                    .Where(name => name != null)
                    .Select(name => name!)
                    .ToList()
            );

            return files;
        }

        public async Task<string> CreateBackupAsync()
        {
            var backupFileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
            var backupPath = Path.Combine(_basePath, "backups", backupFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(backupPath)!);

            using (var zip = new FileStream(backupPath, FileMode.Create))
            using (var archive = new ZipArchive(zip, ZipArchiveMode.Create))
            {
                await AddDirectoryToZip(archive, Path.Combine(_basePath, "photos"), "photos");
                await AddDirectoryToZip(archive, Path.Combine(_basePath, "documents"), "documents");
            }

            _logger.LogInformation("Created backup: {BackupPath}", backupPath);
            return backupPath;
        }

        private async Task AddDirectoryToZip(ZipArchive archive, string directory, string entryName)
        {
            if (!Directory.Exists(directory)) return;

            var files = await Task.Run(() => Directory.GetFiles(directory, "*", SearchOption.AllDirectories));

            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(directory, file);
                var entry = archive.CreateEntry(Path.Combine(entryName, relativePath));

                using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                using var entryStream = entry.Open();
                await fileStream.CopyToAsync(entryStream);
            }
        }

        private static string GetSafeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var safeName = string.Join("_", fileName.Split(invalidChars));
            return $"{DateTime.Now:yyyyMMddHHmmss}_{safeName}";
        }
    }
}
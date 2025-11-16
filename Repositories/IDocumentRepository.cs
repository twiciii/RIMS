using RIMS.Models.Entities;

namespace RIMS.Repositories
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<RIMSDocumentApplication>> GetAllAsync();
        Task<RIMSDocumentApplication?> GetByIdAsync(int id);
        Task<RIMSDocumentApplication> AddAsync(RIMSDocumentApplication document);
        Task<RIMSDocumentApplication> UpdateAsync(RIMSDocumentApplication document);
        Task<bool> DeleteAsync(int id);
    }
}
using RIMS.Models.Entities;

namespace RIMS.Repositories
{
    public interface IResidentRepository
    {
        Task<IEnumerable<RIMSResident>> GetAllAsync();
        Task<RIMSResident?> GetByIdAsync(int id);
        Task<RIMSResident> AddAsync(RIMSResident resident);
        Task<RIMSResident> UpdateAsync(RIMSResident resident);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<RIMSResident>> SearchAsync(string searchTerm);
        Task<bool> ExistsAsync(int id);

        // Add the missing methods
        Task<IEnumerable<RIMSResident>> GetByCategoryAsync(int categoryId);
        Task<int> GetCountAsync();
        Task<RIMSResident?> GetWithDetailsAsync(int id);
    }
}
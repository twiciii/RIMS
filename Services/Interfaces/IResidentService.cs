using RIMS.Models.Entities;
using RIMS.Models.ViewModels;

namespace RIMS.Services.Interfaces
{
    public interface IResidentService
    {
        // Basic CRUD operations
        Task<RIMSResident?> GetResidentByIdAsync(int id);
        Task<IEnumerable<RIMSResident>> GetAllResidentsAsync();
        Task<IEnumerable<RIMSResident>> GetResidentsByCategoryAsync(int categoryId);
        Task<RIMSResident> CreateResidentAsync(RIMSResident resident);
        Task<RIMSResident> UpdateResidentAsync(RIMSResident resident);
        Task<bool> DeleteResidentAsync(int id);

        // Search and analytics
        Task<IEnumerable<RIMSResident>> SearchResidentsAsync(string searchTerm);
        Task<int> GetResidentCountAsync();

        // Detailed operations
        Task<RIMSResident?> GetResidentWithDetailsAsync(int id);
        Task<IEnumerable<RIMSResidentCategory>> GetResidentCategoriesAsync(int residentId);
        Task<bool> AddResidentCategoryAsync(RIMSResidentCategory category);
        Task<bool> RemoveResidentCategoryAsync(int categoryId);

        // Advanced operations with DTOs
        Task<RIMSResident> CreateResidentWithDetailsAsync(ResidentCreateDto residentDto);
        Task<ResidentFullDto> GetResidentFullDetailsAsync(int residentId);

        // DTO methods for backward compatibility
        Task<RIMSResident> CreateResidentWithHouseholdAsync(ResidentCreateDto residentDto);
        Task<ResidentFullDto> GetResidentWithHouseholdAsync(int residentId);
    }
}
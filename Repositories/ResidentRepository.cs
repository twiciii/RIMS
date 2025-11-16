using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;

namespace RIMS.Repositories
{
    public class ResidentRepository : IResidentRepository
    {
        private readonly ApplicationDbContext _context;

        public ResidentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RIMSResident>> GetAllAsync()
        {
            return await _context.rimsResidents.ToListAsync();
        }

        public async Task<RIMSResident?> GetByIdAsync(int id)
        {
            return await _context.rimsResidents.FindAsync(id);
        }

        public async Task<RIMSResident> AddAsync(RIMSResident resident)
        {
            _context.rimsResidents.Add(resident);
            await _context.SaveChangesAsync();
            return resident;
        }

        public async Task<RIMSResident> UpdateAsync(RIMSResident resident)
        {
            _context.rimsResidents.Update(resident);
            await _context.SaveChangesAsync();
            return resident;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var resident = await GetByIdAsync(id);
            if (resident != null)
            {
                _context.rimsResidents.Remove(resident);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<RIMSResident>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _context.rimsResidents
                .Where(r => r.FirstName.Contains(searchTerm) ||
                           r.LastName.Contains(searchTerm) ||
                           (r.MiddleName != null && r.MiddleName.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.rimsResidents.AnyAsync(r => r.Id == id);
        }

        // Implement the missing methods
        public async Task<IEnumerable<RIMSResident>> GetByCategoryAsync(int categoryId)
        {
            return await _context.rimsResidents
                .Where(r => r.ResidentCategories.Any(rc => rc.CategoryId == categoryId))
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.rimsResidents.CountAsync();
        }

        public async Task<RIMSResident?> GetWithDetailsAsync(int id)
        {
            return await _context.rimsResidents
                .Include(r => r.DocumentApplications)
                .Include(r => r.ResidentCategories)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
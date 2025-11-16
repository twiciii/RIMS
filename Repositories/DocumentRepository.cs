using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;

namespace RIMS.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> GetAllAsync()
        {
            return await _context.rimsDocumentApplication.ToListAsync();
        }

        public async Task<RIMSDocumentApplication?> GetByIdAsync(int id)
        {
            return await _context.rimsDocumentApplication.FindAsync(id);
        }

        public async Task<RIMSDocumentApplication> AddAsync(RIMSDocumentApplication document)
        {
            _context.rimsDocumentApplication.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<RIMSDocumentApplication> UpdateAsync(RIMSDocumentApplication document)
        {
            _context.rimsDocumentApplication.Update(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var document = await GetByIdAsync(id);
            if (document != null)
            {
                _context.rimsDocumentApplication.Remove(document);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
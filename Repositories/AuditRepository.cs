using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;

namespace RIMS.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RIMSAuditTrail>> GetAllAsync()
        {
            return await _context.rimsAuditTrail
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();
        }

        public async Task<RIMSAuditTrail> AddAsync(RIMSAuditTrail auditTrail)
        {
            _context.rimsAuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();
            return auditTrail;
        }

        public async Task<IEnumerable<RIMSAuditTrail>> GetByUserIdAsync(string userId)
        {
            return await _context.rimsAuditTrail
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSAuditTrail>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.rimsAuditTrail
                .Where(a => a.ActionDate >= startDate && a.ActionDate <= endDate)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();
        }
    }
}
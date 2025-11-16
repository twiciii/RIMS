using RIMS.Models.Entities;

namespace RIMS.Repositories
{
    public interface IAuditRepository
    {
        Task<IEnumerable<RIMSAuditTrail>> GetAllAsync();
        Task<RIMSAuditTrail> AddAsync(RIMSAuditTrail auditTrail);
        Task<IEnumerable<RIMSAuditTrail>> GetByUserIdAsync(string userId);
        Task<IEnumerable<RIMSAuditTrail>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
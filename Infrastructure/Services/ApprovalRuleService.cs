using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ApprovalRuleService : IApprovalRuleService
    {
        private readonly AppDbContext _context;

        public ApprovalRuleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApprovalRule>> GetAllAsync()
        {
            return await _context.ApprovalRule
                .Include(r => r.AreaNavigation)
                .Include(r => r.TypeNavigation)
                .Include(r => r.ApproverRoleNavigation)
                .ToListAsync();
        }
    }
}

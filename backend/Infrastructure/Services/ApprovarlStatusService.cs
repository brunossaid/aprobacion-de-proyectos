using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ApprovalStatusService : IApprovalStatusService
    {
        private readonly AppDbContext _context;

        public ApprovalStatusService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApprovalStatus>> GetAllAsync()
        {
            return await _context.ApprovalStatus.ToListAsync();
        }
    }
}

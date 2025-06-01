using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ApproverRoleService : IApproverRoleService
    {
        private readonly AppDbContext _context;

        public ApproverRoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApproverRole>> GetAllAsync()
        {
            return await _context.ApproverRole.ToListAsync();
        }
    }
}

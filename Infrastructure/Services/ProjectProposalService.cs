using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProjectProposalService : IProjectProposalService
    {
        private readonly AppDbContext _context;

        public ProjectProposalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectProposal>> GetAllAsync()
        {
            return await _context.ProjectProposal
                .Include(p => p.AreaNavigation)
                .Include(p => p.TypeNavigation)
                .Include(p => p.StatusNavigation)
                .Include(p => p.CreateByNavigation)
                    .ThenInclude(u => u.RoleNavigation)
                .ToListAsync();
        }

        public async Task<ProjectProposal?> GetByIdAsync(Guid id)
        {
            return await _context.ProjectProposal
                .Include(p => p.AreaNavigation)
                .Include(p => p.TypeNavigation)
                .Include(p => p.StatusNavigation)
                .Include(p => p.CreateByNavigation)
                    .ThenInclude(u => u.RoleNavigation)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(ProjectProposal proposal)
        {
            _context.ProjectProposal.Add(proposal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectProposal proposal)
        {
            _context.ProjectProposal.Update(proposal);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TitleExistsAsync(string title, Guid? excludeId = null)
        {
            var query = _context.ProjectProposal.AsQueryable();

            query = query.Where(p => p.Title.ToLower() == title.ToLower());

            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}

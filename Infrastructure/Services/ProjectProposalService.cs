using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;

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
                .ToListAsync();
        }

        public async Task<ProjectProposal?> GetByIdAsync(Guid id)
        {
            return await _context.ProjectProposal
                .Include(p => p.AreaNavigation)
                .Include(p => p.TypeNavigation)
                .Include(p => p.StatusNavigation)
                .Include(p => p.CreateByNavigation)
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

        public async Task<List<ProjectProposal>> GetFilteredAsync(ProjectProposalFilterDto filters)
        {
            var query = _context.ProjectProposal
                .Include(p => p.AreaNavigation)
                .Include(p => p.TypeNavigation)
                .Include(p => p.StatusNavigation)
                .Include(p => p.CreateByNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.Title))
            {
                query = query.Where(p => p.Title.Contains(filters.Title));
            }

            if (filters.Status.HasValue)
            {
                query = query.Where(p => p.Status == filters.Status.Value);
            }

            if (filters.CreatedBy.HasValue)
            {
                query = query.Where(p => p.CreateBy == filters.CreatedBy.Value);
            }

            if (filters.ApproverUserId.HasValue)
            {
                query = query.Where(p =>
                    _context.ProjectApprovalStep.Any(s =>
                        s.ProjectProposalId == p.Id && s.ApproverUserId == filters.ApproverUserId));
            }

            return await query.ToListAsync();
        }

    }
}

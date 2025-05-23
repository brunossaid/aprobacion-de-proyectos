using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProjectApprovalStepService : IProjectApprovalStepService
    {
        private readonly AppDbContext _context;

        public ProjectApprovalStepService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectApprovalStep>> GetAllAsync()
        {
            return await _context.ProjectApprovalStep
                .Include(s => s.ProjectProposalNavigation)
                .Include(s => s.ApproverRoleNavigation)
                .Include(s => s.StatusNavigation)
                .Include(s => s.ApproverUserNavigation)
                .ToListAsync();
        }

        public async Task<List<ProjectApprovalStep>> GetStepsByProjectIdAsync(Guid projectId)
        {
            return await _context.ProjectApprovalStep
                .Include(s => s.ProjectProposalNavigation)
                .Include(s => s.ApproverRoleNavigation)
                .Include(s => s.StatusNavigation)
                .Include(s => s.ApproverUserNavigation)
                .Where(s => s.ProjectProposalId == projectId)
                .OrderBy(s => s.StepOrder)
                .ToListAsync();
        }

        public async Task<ProjectApprovalStep?> GetByIdAsync(long id)
        {
            return await _context.ProjectApprovalStep
                .Include(s => s.ProjectProposalNavigation)
                .Include(s => s.ApproverRoleNavigation)
                .Include(s => s.StatusNavigation)
                .Include(s => s.ApproverUserNavigation)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(ProjectApprovalStep step)
        {
            _context.ProjectApprovalStep.Add(step);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectApprovalStep step)
        {
            _context.ProjectApprovalStep.Update(step);
            await _context.SaveChangesAsync();
        }
    }
}

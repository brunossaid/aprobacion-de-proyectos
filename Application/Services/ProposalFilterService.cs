using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ProposalFilterService
    {
        private readonly IProjectProposalService _proposalService;
        private readonly IProjectApprovalStepService _stepService;

        public ProposalFilterService(IProjectProposalService proposalService, IProjectApprovalStepService stepService)
        {
            _proposalService = proposalService;
            _stepService = stepService;
        }

        public async Task<List<ProjectProposal>> GetFilteredAsync(ProjectProposalFilterDto filters)
        {
            var proposals = await _proposalService.GetAllAsync();
            
            var query = proposals.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.Title))
            {
                query = query.Where(p => p.Title.Contains(filters.Title, StringComparison.OrdinalIgnoreCase));
            }

            if (filters.Status.HasValue)
            {
                query = query.Where(p => p.Status == filters.Status.Value);
            }

            if (filters.CreateBy.HasValue)
            {
                query = query.Where(p => p.CreateBy == filters.CreateBy.Value);
            }

            if (filters.ApproverUserId.HasValue)
            {
                var steps = await _stepService.GetAllAsync();
                var filteredIds = steps
                    .Where(s => s.ApproverUserId == filters.ApproverUserId)
                    .Select(s => s.ProjectProposalId)
                    .Distinct()
                    .ToHashSet();

                query = query.Where(p => filteredIds.Contains(p.Id));
            }

            return query.ToList();
        }
    }
}

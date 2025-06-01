using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ProposalFilterService
    {
        private readonly IProjectProposalReader _proposalReader;
        private readonly IProjectApprovalStepReader _stepReader;

        public ProposalFilterService(IProjectProposalReader proposalReader, IProjectApprovalStepReader stepReader)
        {
            _proposalReader = proposalReader;
            _stepReader = stepReader;
        }

        public async Task<List<ProjectProposal>> GetFilteredAsync(ProjectProposalFilterDto filters)
        {
            var proposals = await _proposalReader.GetAllAsync();
            
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
                var steps = await _stepReader.GetAllAsync();
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

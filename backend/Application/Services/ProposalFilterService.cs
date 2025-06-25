using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ProposalFilterService
    {
        private readonly IProjectProposalReader _proposalReader;
        private readonly ApprovalStepManager _approvalStepManager;

        public ProposalFilterService(
            IProjectProposalReader proposalReader,
            ApprovalStepManager approvalStepManager 
        )
        {
            _proposalReader = proposalReader;
            _approvalStepManager = approvalStepManager;
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

            if (filters.Applicant.HasValue)
            {
                query = query.Where(p => p.CreateBy == filters.Applicant.Value);
            }

            if (filters.ApprovalUser.HasValue)
            {
                var pendingSteps = await _approvalStepManager.GetPendingStepsForUserAsync(filters.ApprovalUser.Value);

                var approvableProposalIds = pendingSteps
                    .Select(step => step.ProjectProposalId)
                    .Distinct()
                    .ToHashSet();

                query = query.Where(p => approvableProposalIds.Contains(p.Id));
            }

            return query.ToList();
        }
    }
}

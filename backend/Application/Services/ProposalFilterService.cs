using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;

namespace Application.Services
{
    public class ProposalFilterService
    {
        private readonly IProjectProposalReader _proposalReader;
        private readonly ApprovalStepManager _approvalStepManager;
        private readonly IMapper _mapper;

        public ProposalFilterService(
            IProjectProposalReader proposalReader,
            ApprovalStepManager approvalStepManager,
            IMapper mapper)
        {
            _proposalReader = proposalReader;
            _approvalStepManager = approvalStepManager;
            _mapper = mapper;
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
        
        public async Task<List<ProjectProposalListDto>> GetFilteredDtoAsync(ProjectProposalFilterDto filters)
        {
            var proposals = await GetFilteredAsync(filters);
            return _mapper.Map<List<ProjectProposalListDto>>(proposals);
        }
    }
}

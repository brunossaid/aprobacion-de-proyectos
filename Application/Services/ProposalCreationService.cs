using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ProposalCreationService : IProposalCreationService
    {
        private readonly IProjectProposalService _proposalService;
        private readonly IApprovalRuleService _approvalRuleService;
        private readonly IProjectApprovalStepService _stepService;

        public ProposalCreationService(
            IProjectProposalService proposalService,
            IApprovalRuleService approvalRuleService,
            IProjectApprovalStepService stepService)
        {
            _proposalService = proposalService;
            _approvalRuleService = approvalRuleService;
            _stepService = stepService;
        }

        public async Task CreateProposalWithStepsAsync(ProjectProposal proposal)
        {
            proposal.Id = Guid.NewGuid();
            await _proposalService.CreateAsync(proposal);

            var allRules = await _approvalRuleService.GetAllAsync();
            var applicableRules = allRules
                .Where(r =>
                    proposal.EstimatedAmount >= r.MinAmount &&
                    (r.MaxAmount == 0 || proposal.EstimatedAmount <= r.MaxAmount))
                .ToList();

            var selectedRules = applicableRules
                .GroupBy(r => r.StepOrder)
                .Select(group => group
                    .OrderByDescending(r => r.Area.HasValue && r.Area == proposal.Area)
                    .ThenByDescending(r => r.Type.HasValue && r.Type == proposal.Type)
                    .First())
                .ToList();

            foreach (var rule in selectedRules)
            {
                var step = new ProjectApprovalStep
                {
                    ProjectProposalId = proposal.Id,
                    ApproverRoleId = rule.ApproverRoleId,
                    Status = 1, // pending
                    StepOrder = rule.StepOrder,
                    ApproverUserId = null,
                    DecisionDate = null,
                    Observations = null
                };

                await _stepService.CreateAsync(step);
            }
        }

        public async Task<ProjectProposal> CreateProposalFromDtoAsync(CreateProjectProposalDto dto)
        {
            var proposal = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                EstimatedAmount = dto.EstimatedAmount,
                EstimatedDuration = dto.EstimatedDuration,
                Area = dto.Area,
                Type = dto.ProjectType,
                CreateBy = dto.CreatedBy,
                Status = 1, // pending
            };

            await CreateProposalWithStepsAsync(proposal);
            var createdProposal = await _proposalService.GetByIdAsync(proposal.Id);

            return createdProposal!;
        }
    }
}

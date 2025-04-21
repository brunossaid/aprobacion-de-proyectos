using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ProposalCreationService
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
    }
}

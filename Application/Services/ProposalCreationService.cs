using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ProposalCreationService : IProposalCreationService
    {
        private readonly IProjectProposalWriter _proposalWriter;
        private readonly IProjectProposalReader _proposalReader;
        private readonly IApprovalRuleService _approvalRuleService;
        private readonly IProjectApprovalStepWriter _stepWriter;

        public ProposalCreationService(
            IProjectProposalWriter proposalWriter,
            IProjectProposalReader proposalReader,
            IApprovalRuleService approvalRuleService,
            IProjectApprovalStepWriter stepWriter)
        {
            _proposalWriter = proposalWriter;
            _proposalReader = proposalReader;
            _approvalRuleService = approvalRuleService;
            _stepWriter = stepWriter;
        }

        public async Task<ProjectProposal> CreateProposalAsync(CreateProjectProposalDto dto)
        {
            var existsWithSameTitle = await _proposalReader.TitleExistsAsync(dto.Title);

            if (existsWithSameTitle)
            {
                throw new InvalidOperationException("Ya existe un proyecto con el mismo titulo.");
            }

            var proposal = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                EstimatedAmount = dto.EstimatedAmount,
                EstimatedDuration = dto.EstimatedDuration,
                Area = dto.Area,
                Type = dto.ProjectType,
                CreateBy = dto.CreateBy,
                Status = 1,
            };

            await _proposalWriter.CreateAsync(proposal);

            var allRules = await _approvalRuleService.GetAllAsync();

            var applicableRules = allRules
                .Where(r =>
                    proposal.EstimatedAmount >= r.MinAmount &&
                    (r.MaxAmount == 0 || proposal.EstimatedAmount <= r.MaxAmount))
                .ToList();

            var selectedRules = applicableRules
                .GroupBy(r => r.StepOrder)
                .Select(group =>
                {
                    // coinciden area y tipe
                    var matchBoth = group.FirstOrDefault(r =>
                        r.Area.HasValue && r.Area == proposal.Area &&
                        r.Type.HasValue && r.Type == proposal.Type);

                    if (matchBoth != null) return matchBoth;

                    // coinciden area o tipo
                    var matchOne = group.FirstOrDefault(r =>
                        (r.Area.HasValue && r.Area == proposal.Area) ||
                        (r.Type.HasValue && r.Type == proposal.Type));

                    // no coincide ninguno
                    return matchOne ?? group.First();
                })
                .ToList();

            foreach (var rule in selectedRules)
            {
                var step = new ProjectApprovalStep
                {
                    ProjectProposalId = proposal.Id,
                    ApproverRoleId = rule.ApproverRoleId,
                    Status = 1,
                    StepOrder = rule.StepOrder,
                    ApproverUserId = null,
                    DecisionDate = null,
                    Observations = null
                };

                await _stepWriter.CreateAsync(step);
            }

            var createdProposal = await _proposalReader.GetByIdAsync(proposal.Id);
            return createdProposal!;
        }
    }
}

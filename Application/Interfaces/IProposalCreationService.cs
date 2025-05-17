using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProposalCreationService
    {
        Task CreateProposalWithStepsAsync(ProjectProposal proposal);
        Task<ProjectProposal> CreateProposalFromDtoAsync(CreateProjectProposalDto dto);
    }
}
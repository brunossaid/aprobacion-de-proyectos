using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProposalCreationService
    {
        Task<ProjectProposal> CreateProposalAsync(CreateProjectProposalDto dto);
    }
}
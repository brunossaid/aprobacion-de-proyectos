using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectProposalService
    {
        Task<List<ProjectProposal>> GetAllAsync();
        Task<ProjectProposal?> GetByIdAsync(Guid id);
        Task CreateAsync(ProjectProposal proposal);
        Task UpdateAsync(ProjectProposal proposal);
        Task<bool> TitleExistsAsync(string title, Guid? excludeId = null);
    }
}

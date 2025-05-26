using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectProposalReader
    {
        Task<List<ProjectProposal>> GetAllAsync();
        Task<ProjectProposal?> GetByIdAsync(Guid id);
        Task<bool> TitleExistsAsync(string title, Guid? excludeId = null);
    }

    public interface IProjectProposalWriter
    {
        Task CreateAsync(ProjectProposal proposal);
        Task UpdateAsync(ProjectProposal proposal);
    }
}

using Domain.Entities;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProjectProposalService
    {
        Task<List<ProjectProposal>> GetAllAsync();
        Task<ProjectProposal?> GetByIdAsync(Guid id);
        Task CreateAsync(ProjectProposal proposal);
        Task UpdateAsync(ProjectProposal proposal);
        Task<List<ProjectProposal>> GetFilteredAsync(ProjectProposalFilterDto filters);
    }
}

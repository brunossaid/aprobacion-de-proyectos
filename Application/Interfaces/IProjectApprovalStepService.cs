using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectApprovalStepService
    {
        Task<List<ProjectApprovalStep>> GetAllAsync();
        Task<ProjectApprovalStep?> GetByIdAsync(long id);
        Task CreateAsync(ProjectApprovalStep step);
        Task UpdateAsync(ProjectApprovalStep step);
    }
}

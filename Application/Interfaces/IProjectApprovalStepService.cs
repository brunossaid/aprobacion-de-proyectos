using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectApprovalStepReader
    {
        Task<List<ProjectApprovalStep>> GetAllAsync();
        Task<ProjectApprovalStep?> GetByIdAsync(long id);
        Task<List<ProjectApprovalStep>> GetStepsByProjectIdAsync(Guid projectId);
    }

    public interface IProjectApprovalStepWriter
    {
        Task CreateAsync(ProjectApprovalStep step);
        Task UpdateAsync(ProjectApprovalStep step);
    }
}

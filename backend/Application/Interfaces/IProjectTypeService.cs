using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectTypeService
    {
        Task<List<ProjectType>> GetAllAsync();
        Task<ProjectType?> GetByIdAsync(int id);
    }
}

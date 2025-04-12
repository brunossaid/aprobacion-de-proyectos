using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectTypeService
    {
        Task<List<ProjectType>> GetAllAsync();
    }
}

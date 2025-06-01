using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAreaService
    {
        Task<List<Area>> GetAllAsync();
    }
}

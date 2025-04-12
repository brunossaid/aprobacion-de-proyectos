using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIndexAsync(int index);
    }
}

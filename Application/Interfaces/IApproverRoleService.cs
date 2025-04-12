using Domain.Entities;

namespace Application.Interfaces
{
    public interface IApproverRoleService
    {
        Task<List<ApproverRole>> GetAllAsync();
    }
}

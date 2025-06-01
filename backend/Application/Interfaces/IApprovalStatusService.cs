using Domain.Entities;

namespace Application.Interfaces
{
    public interface IApprovalStatusService
    {
        Task<List<ApprovalStatus>> GetAllAsync();
    }
}

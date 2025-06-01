using Domain.Entities;

namespace Application.Interfaces
{
    public interface IApprovalRuleService
    {
        Task<List<ApprovalRule>> GetAllAsync();
    }
}

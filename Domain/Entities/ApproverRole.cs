namespace Domain.Entities;

public class ApproverRole
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // relations
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<ApprovalRule> ApprovalRules { get; set; } = new List<ApprovalRule>();
}
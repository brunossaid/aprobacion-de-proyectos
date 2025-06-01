namespace Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class Area
{
    public int Id { get; set; }
    [MaxLength(25)]
    public string Name { get; set; } = null!;

    public ICollection<ProjectProposal> ProjectProposals { get; set; } = new List<ProjectProposal>();
    public ICollection<ApprovalRule> ApprovalRules { get; set; } = new List<ApprovalRule>();
}
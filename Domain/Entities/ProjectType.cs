namespace Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class ProjectType
{
    public int Id { get; set; }
    [MaxLength(25)]
    public string Name { get; set; } = null!;

    // relations
    public ICollection<ProjectProposal> ProjectProposals { get; set; } = new List<ProjectProposal>();
    public ICollection<ApprovalRule> AppovalRules { get; set; } = new List<ApprovalRule>();
}
namespace Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class ApprovalStatus
{
    public int Id { get; set; }
    [MaxLength(25)]
    public string Name { get; set; } = null!;

    public ICollection<ProjectApprovalStep> ProjectApprovalSteps { get; set; } = new List<ProjectApprovalStep>();
    public ICollection<ProjectProposal> ProjectProposals { get; set; } = new List<ProjectProposal>();
}
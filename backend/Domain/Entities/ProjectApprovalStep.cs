namespace Domain.Entities;


public class ProjectApprovalStep
{
    public long Id { get; set; }
    public int StepOrder { get; set; }
    public DateTime? DecisionDate { get; set; }
    public string? Observations { get; set; }

    public Guid ProjectProposalId { get; set; }
    public int? ApproverUserId { get; set; }
    public int ApproverRoleId { get; set; }
    public int Status { get; set; }

    public ProjectProposal ProjectProposalNavigation { get; set; } = null!;
    public User ApproverUserNavigation { get; set; } = null!;
    public ApproverRole ApproverRoleNavigation { get; set; } = null!;
    public ApprovalStatus StatusNavigation { get; set; } = null!;
}
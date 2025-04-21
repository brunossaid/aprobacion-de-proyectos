namespace Domain.Entities;

public class ApprovalRule
{
    public long Id { get; set; }
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public int StepOrder { get; set; }

    public int? Area { get; set; }
    public int? Type { get; set; }
    public int ApproverRoleId { get; set; }

    public Area? AreaNavigation { get; set; }
    public ProjectType? TypeNavigation { get; set; }
    public ApproverRole ApproverRoleNavigation { get; set; } = null!;
}
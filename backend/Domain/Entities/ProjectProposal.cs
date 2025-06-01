namespace Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class ProjectProposal
{
    public Guid Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal EstimatedAmount { get; set; }
    public int EstimatedDuration { get; set; }
    public DateTime CreateAt { get; set; }


    public int Area { get; set; }
    public int Type { get; set; }
    public int Status { get; set; }
    public int CreateBy { get; set; }

    public Area AreaNavigation { get; set; } = null!;
    public ProjectType TypeNavigation { get; set; } = null!;
    public ApprovalStatus StatusNavigation { get; set; } = null!;
    public User CreateByNavigation { get; set; } = null!;
}

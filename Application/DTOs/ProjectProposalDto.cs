namespace Application.DTOs
{
    public class ProjectProposalDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal EstimatedAmount { get; set; }
        public int EstimatedDuration { get; set; }
        public DateTime CreateAt { get; set; }
        public AreaDto Area { get; set; } = null!;
        public ProjectTypeDto Type { get; set; } = null!;
        public ApprovalStatusDto Status { get; set; } = null!;
        public UserDto CreateBy { get; set; } = null!;
        public List<ProjectApprovalStepDto> ApprovalSteps { get; set; } = new();
    }
}

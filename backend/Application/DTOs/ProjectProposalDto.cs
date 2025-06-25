namespace Application.DTOs
{
    public class ProjectProposalDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public int Duration { get; set; }
        public AreaDto Area { get; set; } = null!;
        public ApprovalStatusDto Status { get; set; } = null!;
        public ProjectTypeDto Type { get; set; } = null!;
        public UserDto User { get; set; } = null!;
        public List<ProjectApprovalStepDto> Steps { get; set; } = new();
    }
}

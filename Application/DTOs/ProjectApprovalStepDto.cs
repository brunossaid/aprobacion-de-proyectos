namespace Application.DTOs
{
    public class ProjectApprovalStepDto
    {
        public long Id { get; set; }
        public int StepOrder { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }

        public UserDto ApproverUser { get; set; } = null!;
        public ApproverRoleDto ApproverRole { get; set; } = null!;
        public ApprovalStatusDto Status { get; set; } = null!;
    }
}

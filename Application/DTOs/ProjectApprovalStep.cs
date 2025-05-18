namespace Application.DTOs
{
    public class ProjectApprovalStepDto
    {
        public int StepOrder { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observation { get; set; }

        public ApproverRoleDto ApproverRole { get; set; } = null!;
        public ApprovalStatusDto ApprovalStatus { get; set; } = null!;
    }
}

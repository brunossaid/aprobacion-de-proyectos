namespace Application.DTOs
{
    public class ProjectProposalFilterDto
    {
        public string? Title { get; set; }
        public int? Status { get; set; } 
        public int? CreatedBy { get; set; } 
        public int? ApproverUserId { get; set; } 
    }
}

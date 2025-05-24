namespace Application.DTOs
{
    public class ProjectProposalFilterDto
    {
        public string? Title { get; set; }
        public int? Status { get; set; } 
        public int? CreateBy { get; set; } 
        public int? ApproverUserId { get; set; } 
    }
}

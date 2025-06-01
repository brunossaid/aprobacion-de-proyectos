namespace Application.DTOs
{
    public class ProjectProposalListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public int Duration { get; set; }
        public string Area { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}

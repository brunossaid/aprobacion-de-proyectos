public class CreateProjectProposalDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal EstimatedAmount { get; set; }
    public int EstimatedDuration { get; set; }
    public int Area { get; set; }
    public int ProjectType { get; set; }
    public int CreatedBy { get; set; }
    
}

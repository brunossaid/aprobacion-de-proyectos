namespace Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }
    [MaxLength(25)]
    public string Name { get; set; } = null!;
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    public int Role { get; set; }

    public ApproverRole RoleNavigation { get; set; } = null!;
}
namespace GM.ProjectManagement.Domain.Entities;

public class ProjectTask
{
    public Guid Id { get; set; } 
    public string Title { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty; 
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public DateTime? DueDate { get; set; }

 
    public Guid ProjectId { get; set; } 
    public Project Project { get; set; } 
    public Guid? AssignedToUserId { get; set; } 
    public User? AssignedToUser { get; set; } 
}

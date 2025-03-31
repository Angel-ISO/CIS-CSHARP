

namespace CisAPI.Dtos.Ideas;
public class IdeaDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Username { get; set; }
    public Guid UserId { get; set; }
    public Guid TopicId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}



namespace CisAPI.Dtos.Ideas;
public class IdeaDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? TopicTitle { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}

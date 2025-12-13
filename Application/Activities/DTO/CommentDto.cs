using System;

namespace Application.Activities.DTO;

public class CommentDto
{
    public required string id { get; set; }
    public required string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string UserId { get; set; }
    public required string DisplayName { get; set; }

    public string? ImageUrl { get; set; }

}

using System;

namespace Domain;

public class ActivityAttendee
{
    public string? UserId { get; set; }
    public User User { get; set; } = null!;
    public string? ActivityId { get; set; }
    public Activity activity { get; set; } = null!;
    public bool IsHost { get; set; }
    public DateTime DateJoined { get; set; } = DateTime.UtcNow;

}

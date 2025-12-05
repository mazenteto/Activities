using System;
using Application.Profiles.DTOs;
using Domain;

namespace Application.Activities.DTO;

public class ActivityDTO:EditActivityDto
{
    public required bool IsCancelled { get; set; }
    public required string HostDisplayName { get; set; }
    public required string HostId { get; set; }
    public ICollection<UserProfile> Attendees { get; set; } = [];
}

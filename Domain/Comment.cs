using System;

namespace Domain;

public class Comment
{
    public string id { get; set; }=Guid.NewGuid().ToString();
    public required string Body { get; set; }
    public DateTime CreatedAt { get; set; }=DateTime.UtcNow;

    //Nav Properties
    public required string UserId { get; set; }
    public  User user { get; set; }=null!;

    public required string ActivityId { get; set; }
    public  Activity activity { get; set; }=null!;

}

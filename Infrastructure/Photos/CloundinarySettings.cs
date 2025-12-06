using System;

namespace Infrastructure.Photos;

public class CloudinarySettings
{
    public required string CloudName { get; set; }
    public required string APIKey { get; set; }
    public required string APISecret { get; set; }

}

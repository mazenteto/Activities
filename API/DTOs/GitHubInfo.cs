using System;
using System.Text.Json.Serialization;

namespace API.DTOs;

public class GitHubInfo
{
    public class GitHubAuthRequest
    {
        public required string Code { get; set; }
        [JsonPropertyName("client_id")]
        public required string  ClientId { get; set; }
        [JsonPropertyName("client_secret")]
        public required string  ClientSecret { get; set; }
        [JsonPropertyName("redirect_uri")]
        public required string  RefirectUri { get; set; }
    }

    public class GitHubTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string  AccessToken { get; set; }="";
    }

    public class GitHubUser
    {
        public string Email { get; set; } ="";
        public string name { get; set; } ="";
        [JsonPropertyName("avatar_url")]
        public string? ImagUrl { get; set; }
    }

    public class GitHubEmail
    {
        public string Email { get; set; }="";
        public bool primary { get; set; }
        public bool verified { get; set; }
    }
}

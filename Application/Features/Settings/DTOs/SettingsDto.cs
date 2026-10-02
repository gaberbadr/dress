namespace Application.Features.Settings.DTOs
{
    public class SettingsDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string? TikTokUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
    }
}

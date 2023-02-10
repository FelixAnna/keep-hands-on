namespace HSS.Common
{
    public class IdentityPlatformSettings
    {
        public string Authority { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }
}

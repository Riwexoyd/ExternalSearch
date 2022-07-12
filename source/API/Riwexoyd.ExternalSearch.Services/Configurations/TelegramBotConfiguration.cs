namespace Riwexoyd.ExternalSearch.Services.Configurations
{
    public sealed class TelegramBotConfiguration
    {
        public string Url { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public bool DeleteWebHook { get; set; }
    }
}

namespace Assistant.Contracts.Bot;

public class BotConfiguration
{
    public string ApiKey { get; set; } = string.Empty;

    public List<string> AllowedUsers { get; set; } = new();
}

namespace DataHubIntern.Shared.Messages;

public class EnrichMessage
{
    public string Soc { get; set; } = string.Empty;
    public bool IsForceUpdateRequired { get; set; }
}
namespace DataHubIntern.Db;

public class Identity
{
    public string Id { get; set; } = string.Empty;

    public string Soc { get; set; } = string.Empty;

    public string? OrganizationName { get; set; }

    public string? ZipCode { get; set; }

    public string? Address { get; set; }

    public string? Tel { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FullName { get; set; }

    public string? Mobile { get; set; }

    public string? Url { get; set; }

    public string? Division { get; set; }

    public string? Position { get; set; }

    public string DataCreatedAt { get; set; }

    public string DataUpdatedAt { get; set; }
}
using ProtoBuf;

namespace DataHubIntern.Shared.Identity;

[ProtoContract]
public class Identity
{
    [ProtoMember(1)]
    public string Id { get; set; } = string.Empty;

    [ProtoMember(2)]
    public string Soc { get; set; } = string.Empty;

    [ProtoMember(3)]
    public string? OrganizationName { get; set; }

    [ProtoMember(4)]
    public string? ZipCode { get; set; }

    [ProtoMember(5)]
    public string? Address { get; set; }

    [ProtoMember(6)]
    public string? Tel { get; set; }

    [ProtoMember(7)]
    public string? Fax { get; set; }

    [ProtoMember(8)]
    public string? Email { get; set; }

    [ProtoMember(9)]
    public string? FirstName { get; set; }

    [ProtoMember(10)]
    public string? LastName { get; set; }

    [ProtoMember(11)]
    public string? FullName { get; set; }

    [ProtoMember(12)]
    public string? Mobile { get; set; }

    [ProtoMember(13)]
    public string? Url { get; set; }

    [ProtoMember(14)]
    public string? Division { get; set; }

    [ProtoMember(15)]
    public string? Position { get; set; }

    [ProtoMember(16)]
    public string DataCreatedAt { get; set; }

    [ProtoMember(17)]
    public string DataUpdatedAt { get; set; }
}

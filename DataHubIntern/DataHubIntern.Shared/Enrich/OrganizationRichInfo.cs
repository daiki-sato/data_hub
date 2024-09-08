using ProtoBuf;

namespace DataHubIntern.Shared.Enrich;

[ProtoContract]
public class OrganizationRichInfo
{
    [ProtoMember(1)]
    public string Soc { get; set; } = string.Empty;

    [ProtoMember(2)]
    public string MainIndustrialClassName { get; set; } = string.Empty;

    [ProtoMember(3)]
    public LatestSalesAccountingTermSales LatestSalesAccountingTermSales { get; set; } = new();
}

[ProtoContract]
public record LatestSalesAccountingTermSales
{
    [ProtoMember(1)]
    public int GreaterThan { get; set; }

    [ProtoMember(2)]
    public int LessThan { get; set; }
}

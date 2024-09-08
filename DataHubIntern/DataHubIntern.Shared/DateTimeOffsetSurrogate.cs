using System.Runtime.CompilerServices;
using ProtoBuf.Meta;
using ProtoBuf;

namespace DataHubIntern.Shared;

[ProtoContract]
internal class DateTimeOffsetSurrogate
{
    [ModuleInitializer]
    internal static void SetSurrogate()
    {
        RuntimeTypeModel.Default.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));
    }

    [ProtoMember(1)]
    public long Ticks { get; set; }

    [ProtoMember(2)]
    public short OffsetMinutes { get; set; }

    public static implicit operator DateTimeOffsetSurrogate(DateTimeOffset value)
    {
        return new DateTimeOffsetSurrogate
        {
            Ticks = value.Ticks,
            OffsetMinutes = (short)value.Offset.TotalMinutes
        };
    }

    public static implicit operator DateTimeOffset(DateTimeOffsetSurrogate value)
    {
        return new DateTimeOffset(value.Ticks, TimeSpan.FromMinutes(value.OffsetMinutes));
    }
}
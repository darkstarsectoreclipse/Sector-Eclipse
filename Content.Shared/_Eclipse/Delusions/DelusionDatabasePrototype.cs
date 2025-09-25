using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;

namespace Content.Shared._Eclipse.Delusions;

[Virtual, DataDefinition]
[Serializable, NetSerializable]

public partial class DelusionDatabase
{
    [DataField(required:true), ViewVariables(VVAccess.ReadOnly)]
    public List<ProtoId<DelusionPrototype>> Delusions { get; set; }
}

[Prototype]
public sealed partial class DelusionDatabasePrototype : DelusionDatabase, IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;
}

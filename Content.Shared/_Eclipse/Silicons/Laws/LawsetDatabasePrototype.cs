using Content.Shared.Silicons.Laws;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Eclipse.Silicons.Laws;

[Virtual, DataDefinition]
[Serializable, NetSerializable]
public partial class SiliconLawsetDatabase
{
    [DataField(required: true), ViewVariables(VVAccess.ReadOnly)]
    public List<ProtoId<SiliconLawsetPrototype>> SiliconLawsets;
}

[Prototype]
public sealed partial class SiliconLawsetDatabasePrototype : SiliconLawsetDatabase, IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;
}

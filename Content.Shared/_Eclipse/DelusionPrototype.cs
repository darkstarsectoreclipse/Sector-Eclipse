using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;


namespace Content.Shared._Eclipse.Delusions;

[Virtual, DataDefinition]
[Serializable, NetSerializable]
public partial class Delusion
{
    /// <summary>
    /// The prototype this delusion was created from.
    /// Does not apply to admin-made delusions.
    /// </summary>
    [DataField]
    public ProtoId<DelusionPrototype>? ProtoId;

    /// <summary>
    /// A locale string of the delusion name.
    /// </summary>
    [DataField(required: true)]
    public LocId DelusionName;

    /// <summary>
    /// A locale string of the delusion description.
    /// </summary>
    [DataField(required: true)]
    public LocId DelusionDesc;

    public string GetLocName()
    {
        return Loc.GetString(DelusionName);
    }

    public string GetLocDesc()
    {
        return Loc.GetString(DelusionDesc);
    }

    public Delusion ShallowClone()
    {
        return new Delusion()
        {
            ProtoId = ProtoId,
            DelusionName = DelusionName,
            DelusionDesc = DelusionDesc
        };
    }
}

[Prototype]
[Serializable, NetSerializable]
public sealed partial class DelusionPrototype : Delusion, IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    public DelusionPrototype()
    {
        ProtoId = ID;
    }
}

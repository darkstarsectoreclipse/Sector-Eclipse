using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;


namespace Content.Shared._Eclipse.Delusions;

[Virtual, DataDefinition]
[Serializable, NetSerializable]
public partial class Delusion
{
    /// <summary>
    /// The prototype this Delusion was created for, used to manage conflicts.
    /// Do not apply to manually created Delusions.
    /// </summary>
    [DataField]
    public ProtoId<DelusionPrototype>? ProtoId;

    /// <summary>
    /// A locale string of the delusion name.
    /// </summary>
    [DataField(required: true)]
    public LocId Name;

    /// <summary>
    /// A locale string of the delusion description.
    /// </summary>
    [DataField(required: true)]
    public LocId Description;

    public string GetLocName()
    {
        return Loc.GetString(Name);
    }

    public string GetLocDesc()
    {
        return Loc.GetString(Description);
    }

    public Delusion ShallowClone()
    {
        return new Delusion()
        {
            ProtoId = ProtoId,
            Name = Name,
            Description = Description,
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

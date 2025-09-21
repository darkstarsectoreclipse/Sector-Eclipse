using Content.Shared.Actions;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Eclipse.Delusions;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedDelusionsSystem))]
public sealed partial class DelusionsComponent : Component
{
    /// <summary>
    /// The active delusions of this entity
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<Delusion> Delusions = new();

    /// <summary>
    /// The probability that a Mental Pulse will change current delusions.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Chance = 0.2f;

    /// <summary>
    /// The maximum number of Delusions affecting the entity
    /// </summary>
    [DataField, AutoNetworkedField]
    public int MaxDelusions = 1;

    /// <summary>
    /// Notification sound played if your delusions change
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier? DelusionsChangedSound = null;

    [DataField(serverOnly: true)]
    public EntityUid? Action;
}

public sealed partial class ToggleDelusionsScreenEvent : InstantActionEvent;

[NetSerializable, Serializable]
public enum DelusionsUiKey : byte
{
    Key,
}

/// <summary>
/// BUI state
/// </summary>
[Serializable, NetSerializable]
public sealed class DelusionsBuiState(List<Delusion> delusions) : BoundUserInterfaceState
{
    public List<Delusion> Delusions = delusions;
}

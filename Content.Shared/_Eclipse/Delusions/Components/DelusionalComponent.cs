using Content.Shared.Actions;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Eclipse.Delusions.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DelusionalComponent : Component
{
    /// <summary>
    /// Indicate if the Delusional status can be removed by taking medication
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Treatable = true;

    /// <summary>
    /// Maximum number of delusions this entity can have.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int MaxDelusionsCount = 2;

    /// <summary>
    /// List of currently active delusions
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<Delusion> Delusions = new();

    /// <summary>
    /// Notification sound played when the delusion list change
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier? DelusionsUpdateSound = new SoundPathSpecifier("/Audio/_Eclipse/Delusions/Theremin.ogg");

    [DataField(serverOnly: true)]
    public EntityUid? Action;
}


[ByRefEvent]
public record struct GetDelusionsEvent(EntityUid Entity)
{
    public EntityUid Entity = Entity;
    public List<Delusion> Delusions = new();
    public bool Handled = false;
}

public sealed partial class ToggleDelusionsScreenEvent : InstantActionEvent
{

}


[NetSerializable, Serializable]
public enum DelusionsUiKey : byte
{
    Key,
}

/// <summary>
/// Bound User Interface State
/// </summary>
/// <param name="delusions">list of delusions to be displayed.</param>
[Serializable, NetSerializable]
public sealed class DelusionalBuiState(List<Delusion> delusions) : BoundUserInterfaceState
{
    public List<Delusion> Delusions = delusions;
}

using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._Eclipse.Delusions;

[Serializable, NetSerializable]
public sealed class DelusionsEuiState : EuiStateBase
{
    public List<Delusion> Delusions { get; }
    public NetEntity Target { get; }

    public DelusionsEuiState(List<Delusion> delusions, NetEntity target)
    {
        Delusions = delusions;
        Target = target;
    }
}

[Serializable, NetSerializable]
public sealed class DelusionsSaveMessage : EuiMessageBase
{
    public List<Delusion> Delusions { get; }
    public NetEntity Target { get; }

    public DelusionsSaveMessage(List<Delusion> delusions, NetEntity target)
    {
        Delusions = delusions;
        Target = target;
    }
}

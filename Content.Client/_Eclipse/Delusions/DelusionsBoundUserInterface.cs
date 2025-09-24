using Content.Shared._Eclipse.Delusions.Components;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client._Eclipse.Delusions;

[UsedImplicitly]
public sealed class DelusionsBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IEntityManager _entityManager = default!;

    [ViewVariables]
    private DelusionsMenu? _menu;

    public DelusionsBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _menu = this.CreateWindow<DelusionsMenu>();
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not DelusionalBuiState msg)
            return;

        if (!_entityManager.TryGetComponent<DelusionalComponent>(Owner, out var component))
            return;

        _menu?.Update(component, msg);
    }
}

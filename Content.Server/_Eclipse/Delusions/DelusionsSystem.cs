using System.Diagnostics.CodeAnalysis;
using Content.Server.Chat.Managers;
using Content.Shared._Eclipse.Delusions;
using Content.Shared.Chat;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server._Eclipse.Delusions;

public sealed class DelusionsSystem : SharedDelusionsSystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly UserInterfaceSystem _bui = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    // default delusion

    // list of ProtoId<...>

    // private EntProtoId ActionViewMoods = "ActionViewMoods";

    public void Initialize()
    {
        base.Initialize();
        // subscribe to local events :
        // <DelusionsComponent, MapInitEvent> OnDelusionsInit
        // <DelusionsComponent, ComponentShutdown> OnDelusionsShutdown
        // <DelusionsComponent, ToggleDelusionsScreenEvent> OnToggleDelusionsScreen)
    }

    private void OnToggleDelusionsScreen(Entity<DelusionsComponent> ent, ref ToggleDelusionsScreenEvent args)
    {
        if (args.Handled || !TryComp<ActorComponent>(ent, out var actor))
            return;
        args.Handled = true;
        _bui.TryToggleUi(ent.Owner, DelusionsUiKey.Key, actor.PlayerSession);
    }

    public void NotifyDelusionChange(Entity<DelusionsComponent> ent)
    {
        if (!TryComp<ActorComponent>(ent, out var actor))
            return;

        var session = actor.PlayerSession;
        _audio.PlayGlobal(ent.Comp.DelusionsChangedSound, session);

        var msg = Loc.GetString("delusions-update-notify");
        var wrappedMessage = Loc.GetString("chat-manager-server-wrap-message", ("message", msg));
        _chatManager.ChatMessageToOne(ChatChannel.Server, msg, wrappedMessage, default, false, session.Channel, colorOverride:Color.Orange);
        UpdateBuiState(ent);
    }

    public void UpdateBuiState(Entity<DelusionsComponent> ent)
    {
        _bui.SetUiState(ent.Owner, DelusionsUiKey.Key, null);
    }

    public void AddDelusion(Entity<DelusionsComponent> ent, Delusion delusion, bool notify = true)
    {
        ent.Comp.Delusions.Add(delusion);
        Dirty(ent);

        if (notify)
            NotifyDelusionChange(ent);
        else
            UpdateBuiState(ent);
    }

    // TODO : roll delusion from a delusion prototype
}

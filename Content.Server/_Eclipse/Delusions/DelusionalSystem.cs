using Content.Server.Actions;
using Content.Server.Chat.Managers;
using Robust.Server.GameObjects;

using Content.Shared._Eclipse.Delusions;
using Content.Shared._Eclipse.Delusions.Components;
using Content.Shared.Chat;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server._Eclipse.Delusions;

public sealed class DelusionalSystem : SharedDelusionalSystem
{
    [Dependency] private readonly UserInterfaceSystem _bui = default!;
    [Dependency] private readonly ActionsSystem _actions = default!;    // Server
    [Dependency] private readonly IChatManager _chatManager = default!; // Server
    [Dependency] private readonly SharedAudioSystem _audio = default!;  //

    private readonly EntProtoId _actionViewDelusions = "ActionViewDelusions";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DelusionalComponent, MapInitEvent>(OnDelusionalInit);
        SubscribeLocalEvent<DelusionalComponent, ComponentShutdown>(OnDelusionalShutdown);
        SubscribeLocalEvent<DelusionalComponent, ToggleDelusionsScreenEvent>(OnToggleDelusionsScreen);
        SubscribeLocalEvent<DelusionalComponent, BoundUIOpenedEvent>(OnBoundUIOpened);
        SubscribeLocalEvent<DelusionalComponent, GetDelusionsEvent>(OnDirectedGetDelusions);
    }

    private void OnToggleDelusionsScreen(Entity<DelusionalComponent> ent, ref ToggleDelusionsScreenEvent args)
    {
        if (args.Handled || !TryComp<ActorComponent>(ent, out var actor))
            return;
        args.Handled = true;
        _bui.TryToggleUi(ent.Owner, DelusionsUiKey.Key, actor.PlayerSession);
    }

    private void OnBoundUIOpened(Entity<DelusionalComponent> ent, ref BoundUIOpenedEvent args)
    {
        var state = new DelusionalBuiState(ent.Comp.Delusions);
        _bui.SetUiState(args.Entity, DelusionsUiKey.Key, state);
    }

    private void OnDelusionsShutdown(Entity<DelusionalComponent> ent, ref ComponentShutdown args)
    {
        var target = ent.Comp;

        if (!TryComp<ActorComponent>(ent, out var actor))
            return;

        var session = actor.PlayerSession;
        _audio.PlayGlobal(target.DelusionsUpdateSound, session);

        var msg = Loc.GetString("delusions-update-notify");
        var wrappedMessage = Loc.GetString("chat-manager-server-wrap-message", ("message", msg));
        _chatManager.ChatMessageToOne(ChatChannel.Server, msg, wrappedMessage, default, false, session.Channel, colorOverride:Color.Purple);
        UpdateBuiState(ent);
    }

    /// <summary>
    /// Set the delusions of a delusional entity while notifying the player
    /// </summary>
    public void SetDelusions(Entity<DelusionalComponent> ent, List<Delusion> newDelusions)
    {
        var target = ent.Comp;
        target.Delusions = newDelusions;
        NotifyDelusionsChanged(ent);
    }

    /// <summary>
    /// Remove all delusions of a delusional entity while notifying the player
    /// </summary>
    /// <param name="ent"></param>
    public void RemoveDelusions(Entity<DelusionalComponent> ent)
    {
        NotifyDelusionsEnded(ent);
        RemComp<DelusionalComponent>(ent);
    }

    public void NotifyDelusionsChanged(Entity<DelusionalComponent> ent)
    {
        var target = ent.Comp;

        if (!TryComp<ActorComponent>(ent, out var actor))
            return;

        var session = actor.PlayerSession;
        _audio.PlayGlobal(target.DelusionsUpdateSound, session);

        var msg = Loc.GetString("delusions-update-notify");
        var wrappedMessage = Loc.GetString("chat-manager-server-wrap-message", ("message", msg));
        _chatManager.ChatMessageToOne(ChatChannel.Server, msg, wrappedMessage, default, false, session.Channel, colorOverride:Color.Purple);
        UpdateBuiState(ent);
    }

    public void NotifyDelusionsEnded(Entity<DelusionalComponent> ent)
    {
        if (!TryComp<ActorComponent>(ent, out var actor))
            return;

        var session = actor.PlayerSession;
        _audio.PlayGlobal(ent.Comp.DelusionsUpdateSound, session);

        var msg = Loc.GetString("delusions-update-notify");
        var wrappedMessage = Loc.GetString("chat-manager-server-wrap-message", ("message", msg));
        _chatManager.ChatMessageToOne(ChatChannel.Server, msg, wrappedMessage, default, false, session.Channel, colorOverride:Color.Purple);
    }

    private void UpdateBuiState(Entity<DelusionalComponent> ent)
    {
        var delusions = ent.Comp.Delusions;
        _bui.SetUiState(ent.Owner, DelusionsUiKey.Key, new DelusionalBuiState(delusions));
    }

    private void OnDirectedGetDelusions(EntityUid uid, DelusionalComponent component, ref GetDelusionsEvent args)
    {
        if (args.Handled)
            return;

        args.Delusions = component.Delusions;

        args.Handled = true;
    }

    private void OnDelusionalInit(Entity<DelusionalComponent> ent, ref MapInitEvent args)
    {
        NotifyDelusionsChanged(ent);

        ent.Comp.Action = _actions.AddAction(ent.Owner, _actionViewDelusions);
    }

    private void OnDelusionalShutdown(Entity<DelusionalComponent> ent, ref ComponentShutdown args)
    {
        _actions.RemoveAction(ent.Owner, ent.Comp.Action);
    }
}

using Content.Server.GameTicking;
using Content.Server.Preferences.Managers;
using Content.Shared._Eclipse.Bank;
using Content.Shared._Eclipse.Bank.Components;
using Content.Shared.Preferences;
using Robust.Shared.Player;

namespace Content.Server._Eclipse.Bank;

public sealed class BankAccountSystem : SharedBankAccountSystem
{
    [Dependency] private readonly IServerPreferencesManager _prefsManager = default!;
    [Dependency] private readonly ISharedPlayerManager _playerManager = default!;
    public override void Initialize()
    {
        base.Initialize();
        var _log = Logger.GetSawmill("bank");
        // InitializeATM();
        // InitializeStationATM();

        //SubscribeLocalEvent<BankAccountComponent, PreferenesLoadedEvent>(OnPreferencesLoaded);
        SubscribeLocalEvent<BankAccountComponent, ComponentInit>(OnInit);
        //SubscribeLocalEvent<BankAccountComponent, PlayerAttachedEvent>(OnPlayerAttached);
        //SubscribeLocalEvent<BankAccountComponent, PlayerDetachedEvent>(OnPlayerDetached);
        //SubscribeLocalEvent<PlayerJoinedLobbyEvent>(OnPlayerJoinedLobby);
        //SubscribeLocalEvent<SectorBankComponent, ComponentInit>(OnSectorInit);

    }

    public bool TryGetBalance(Entity<BankAccountComponent> ent, out int balance)
    {
        if (!_playerManager.TryGetSessionByEntity(ent.Owner, out var session) ||
            !_prefsManager.TryGetCachedPreferences(session.UserId, out var prefs))
        {
            balance = 0;
            return false;
        }

        if (prefs.SelectedCharacter is not HumanoidCharacterProfile)
        {
            balance = 0;
            return false;
        }

        balance = ent.Comp.Balance;
        return true;
    }

    public bool TryDeposit(Entity<BankAccountComponent> ent, int amount)
    {
        if (amount <= 0)
            return false;

        if (!_playerManager.TryGetSessionByEntity(ent.Owner, out var session))
            return false;

        if (!_prefsManager.TryGetCachedPreferences(session.UserId, out var prefs))
            return false;

        if (prefs.SelectedCharacter is not HumanoidCharacterProfile)
            return false;

        ent.Comp.Balance += amount;
        return true;
    }

    public bool TryWithdraw(Entity<BankAccountComponent> ent, int amount)
    {
        if (amount <= 0 || ent.Comp.Frozen || ent.Comp.Balance < amount)
            return false;

        ent.Comp.Balance -= amount;
        return true;
    }

    public void SetFrozen(Entity<BankAccountComponent> ent, bool value)
    {
        ent.Comp.Frozen = value;
    }

    private void OnInit(Entity<BankAccountComponent> ent, ref ComponentInit _)
    {
        UpdateBankBalance(ent);
    }


    private void UpdateBankBalance(Entity<BankAccountComponent> ent)
    {
        TryGetBalance(ent,  out var balance);
        ent.Comp.Balance = balance;
    }
}

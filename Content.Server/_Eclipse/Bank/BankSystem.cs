using Content.Server.GameTicking;
using Content.Server.Preferences.Managers;
using Content.Shared._Eclipse.Bank;
using Content.Shared._Eclipse.Bank.Components;
using Content.Shared.Preferences;
using Robust.Shared.Player;

namespace Content.Server._Eclipse.Bank;

public sealed class BankSystem : SharedBankSystem
{
    //[Dependency] private readonly IServerPreferencesManager _prefsManager = default!;
    //[Dependency] private readonly ISharedPlayerManager _playerManager = default!;
    public override void Initialize()
    {
        base.Initialize();
    }

    // not microwavable...
    public bool TryDeposit(Entity<CreditCardComponent> ent, int value)
    {
        if (value < 0)
            return false;

        ent.Comp.Balance += value;
        return true;
    }

    public bool TryWithdraw(Entity<CreditCardComponent> ent, int value)
    {
        if (value < 0 || ent.Comp.Frozen || ent.Comp.Balance < value)
            return false;

        ent.Comp.Balance -= value;
        return true;
    }

    public bool TryGetBalance(Entity<CreditCardComponent> ent, out int value)
    {
        value = ent.Comp.Balance;
        return true;
    }
}

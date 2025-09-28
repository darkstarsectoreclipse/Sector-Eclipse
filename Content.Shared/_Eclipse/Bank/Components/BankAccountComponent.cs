using Robust.Shared.GameStates;

namespace Content.Shared._Eclipse.Bank.Components;

[RegisterComponent, NetworkedComponent]

public sealed partial class BankAccountComponent : Component
{
    // how much money does this entity have on their account
    [DataField]
    public int Balance;

    // is the bank account of this entity frozen ?
    [DataField]
    public bool Frozen;
}

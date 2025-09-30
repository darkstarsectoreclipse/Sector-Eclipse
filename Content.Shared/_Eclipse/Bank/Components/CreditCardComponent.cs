using Robust.Shared.GameStates;

namespace Content.Shared._Eclipse.Bank.Components;

[RegisterComponent, NetworkedComponent]

public sealed partial class CreditCardComponent : Component
{
    // The number assigned to this card
    [DataField]
    public int Number;

    // The owner of the account (todo : not a string, a bank account linked to the card...)
    [DataField]
    public string OwnerName;

    // Balance stored inside the credit card
    [DataField]
    public int Balance;

    // is the card frozen ?
    [DataField]
    public bool Frozen;
}

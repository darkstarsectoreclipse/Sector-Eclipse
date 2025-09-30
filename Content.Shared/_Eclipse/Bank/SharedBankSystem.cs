using Content.Shared._CD.NanoChat;
using Content.Shared._Eclipse.Bank.Components;
using Content.Shared.Examine;

namespace Content.Shared._Eclipse.Bank;

public abstract class SharedBankSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CreditCardComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(Entity<CreditCardComponent> ent, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        args.PushMarkup(Loc.GetString("credit-card-examine-number", ("number", $"{ent.Comp.Number:D4}")));
    }
}

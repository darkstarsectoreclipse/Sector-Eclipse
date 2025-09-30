using Content.Client.UserInterface.Fragments;
using Content.Shared._Eclipse.CartridgeLoader.Cartridges;
using Content.Shared.Damage.Components;
using Robust.Client.UserInterface;

namespace Content.Client._Eclipse.CartridgeLoader.Cartridges;

public sealed partial class CreditCardUi : UIFragment
{
    private CreditCardUiFragment? _fragment;

    public override Control GetUIFragmentRoot()
    {
        return _fragment!;
    }

    public override void Setup(BoundUserInterface userInterface, EntityUid? fragmentOwner)
    {
        _fragment = new CreditCardUiFragment();

        /*
        _fragment.OnMessageSent += (type, number, content, job) =>
        {
            SendNanoChatUiMessage(type, number, content, job, userInterface);
        };
        */
    }

    public override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is CreditCardUiState cast)
            _fragment?.UpdateState(cast);
    }
}

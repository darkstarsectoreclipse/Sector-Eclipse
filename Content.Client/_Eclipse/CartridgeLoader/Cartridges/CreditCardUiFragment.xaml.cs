using Content.Client._CD.CartridgeLoader.Cartridges;
using Content.Shared._Eclipse.CartridgeLoader.Cartridges;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client._Eclipse.CartridgeLoader.Cartridges;

public sealed class CreditCardUiFragment : BoxContainer
{
    public CreditCardUiFragment()
    {
        IoCManager.InjectDependencies(this);
        RobustXamlLoader.Load(this);

        //_newChatPopup = new NewChatPopup();
        SetupEventHandlers();
    }

    public void SetupEventHandlers()
    {

    }

    public void UpdateState(CreditCardUiState cast)
    {

    }
}

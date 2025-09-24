using Content.Client.Eui;
using Content.Shared._Eclipse.Delusions;
using Content.Shared.Eui;

namespace Content.Client._Eclipse.Delusions.Eui;

public sealed class DelusionsEui : BaseEui
{
    private DelusionUi _delusionUi;
    private NetEntity _target;

    public DelusionsEui()
    {
        _delusionUi = new DelusionUi();
        _delusionUi.OnSave += SaveDelusions;
    }

    private void SaveDelusions()
    {
        var newDelusions = _delusionUi.GetDelusions();
        SendMessage(new DelusionsSaveMessage(newDelusions, _target));
        _delusionUi.SetDelusions(newDelusions);
    }

    public override void Opened()
    {
        _delusionUi.OpenCentered();
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is not DelusionsEditEuiState s)
            return;

        _target = s.Target;
        _delusionUi.SetDelusions(s.Delusions);
    }
}

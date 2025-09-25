using Content.Shared._Eclipse.Delusions;
using Content.Shared._Eclipse.Delusions.Components;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Shared._Eclipse.EntityEffects.Effects;

/// <summary>
/// Erase any existing delusions, make you resist any delusion crisis for a limited time.
/// </summary>
public sealed partial class TreatDelusions : EventEntityEffect<TreatDelusions>
{
    /// <summary>
    /// The duration of the resistance to delusions crisis (in seconds)
    /// </summary>
    [DataField]
    public int Delay = 600;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-chem-vomit"); // whoops.

}

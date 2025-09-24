using Content.Shared._Eclipse.Delusions;
using Content.Shared._Eclipse.Delusions.Components;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Shared._Eclipse.EntityEffects.Effects;

/// <summary>
/// Erase any existing delusions, make you resist any delusion crisis for a limited time.
/// </summary>
public sealed partial class ChemResistDelusions : EntityEffect
{
    /// <summary>
    /// The duration of the resistance to delusions crisis (in seconds)
    /// </summary>
    [DataField]
    public float Time = 120f;

    /// <summary>
    /// If true, the effect will persist forever.
    /// </summary>
    [DataField]
    public bool Perpetual = false;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-chem-vomit");

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args.EntityManager.HasComponent<DelusionalComponent>(args.TargetEntity))
            args.EntityManager.RemoveComponent <DelusionalComponent>(args.TargetEntity);

        if (! args.EntityManager.HasComponent<DelusionCrisisTargetComponent>(args.TargetEntity))
            return;

        var target = args.EntityManager.GetComponent<DelusionCrisisTargetComponent>(args.TargetEntity);
        if (Time <= 0 && !Perpetual)
        {
            target.Resistant = false;
        }
        else
        {
            target.Resistant = true; // TODO : see how to make this effect not perpetual ?...

        }

    }
}

using Content.Shared._Eclipse.Delusions;
using Content.Shared._Eclipse.Delusions.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server._Eclipse.Delusions;


// TODO : Ensure no repetition inside delusion selection
// TODO : Implement delusion incompatibilities

public sealed class DelusionCrisisSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly DelusionalSystem _delusionalSystem = default!;

    private readonly ProtoId<DelusionDatabasePrototype> _baseDelusions = "BaseSetDelusion";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DelusionCrisisTargetComponent, MapInitEvent>(OnDelusionCrisisInit);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var timing = IoCManager.Resolve<IGameTiming>();

        var query = EntityQueryEnumerator<DelusionCrisisTargetComponent>();
        while (query.MoveNext(out var entityUid, out var target))
        {
            if (timing.CurTime < target.TimeNextCrisis)
                continue;

            AttemptCrisis((entityUid, target));

            target.TimeNextCrisis = timing.CurTime + target.DelayBetweenCrisis;
            target.TimeNextCrisis += _robustRandom.NextFloat(-0.5f, 0.5f) * target.DelayVariation;
        }

    }

    private void AttemptCrisis(Entity<DelusionCrisisTargetComponent> ent)
    {
        var target = ent.Comp;
        if (target.Resistant)
            return;

        if (!_robustRandom.Prob(target.CrisisSuccessProbability))
            return;

        var delcomp = EnsureComp<DelusionalComponent>(ent);
        var delusions = delcomp.Delusions;

        if (delusions.Count == 0 || (_robustRandom.Prob(target.AggravationProbability) && delusions.Count < delcomp.MaxDelusionsCount))
        {
            delusions.Add(PickRandomDelusion());
        }
        else
        {
            delusions[_robustRandom.Next() % delusions.Count] = PickRandomDelusion();
        }

        _delusionalSystem.SetDelusions((ent.Owner, delcomp), delusions);
    }

    private Delusion PickRandomDelusion()
    {
        var id = _robustRandom.Pick(_proto.Index(_baseDelusions).Delusions);
        return _proto.Index(id);
    }

    private void OnDelusionCrisisInit(Entity<DelusionCrisisTargetComponent> ent, ref MapInitEvent args)
    {
        foreach (var id in _proto.Index(_baseDelusions).Delusions)
        {
            _proto.Resolve<DelusionPrototype>(id, out var prototype);
        }

        var target = ent.Comp;
        var timing =  IoCManager.Resolve<IGameTiming>();

        target.TimeNextCrisis = timing.CurTime + target.DelayBetweenCrisis;
        target.TimeNextCrisis += _robustRandom.NextFloat(-0.5f, 0.5f) * target.DelayVariation;
    }
}

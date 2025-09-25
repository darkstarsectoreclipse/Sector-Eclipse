using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        var now = IoCManager.Resolve<IGameTiming>().CurTime;

        var query = EntityQueryEnumerator<DelusionCrisisTargetComponent>();
        while (query.MoveNext(out var entityUid, out var target))
        {
            if (now < target.TimeNextCrisis)
                continue;

            AttemptCrisis((entityUid, target));

            target.TimeNextCrisis = now + target.DelayBetweenCrisis;
            target.TimeNextCrisis += _robustRandom.NextFloat(-1f, 1f) * target.DelayVariation;
        }

    }

    private void AttemptCrisis(Entity<DelusionCrisisTargetComponent> ent)
    {
        var target = ent.Comp;
        if (target.Resistant)
            return;

        if (!_robustRandom.Prob(target.CrisisSuccessProbability))
            return;

        var alreadyDelusional = TryComp<DelusionalComponent>(ent, out var delComp);

        // ensure the target is now delusional

        if (!alreadyDelusional)
            delComp = AddComp<DelusionalComponent>(ent);

        if (delComp == null)
            return;

        var delusions = delComp.Delusions;

        if (delusions.Count == 0 || (_robustRandom.Prob(target.AggravationProbability) && delusions.Count < delComp.MaxDelusionsCount))
        {
            AddRandomDelusion((ent, delComp), alreadyDelusional);
            //delusions.Add(PickRandomDelusion());
        }
        else
        {
            ReplaceRandomDelusion((ent, delComp), alreadyDelusional);
        }
    }

    private void AddRandomDelusion(Entity<DelusionalComponent> ent, bool notify = false)
    {
        var delusions = ent.Comp.Delusions;
        var excludedIds = delusions.Select(d => d.ProtoId).ToList();

        if (!TryPickRandomDelusion(_baseDelusions, excludedIds, out var prototype))
            return;

        delusions.Add(_proto.Index<DelusionPrototype>(prototype));
        _delusionalSystem.SetDelusions(ent, delusions, notify);
    }

    private void ReplaceRandomDelusion(Entity<DelusionalComponent> ent, bool notify = false)
    {
        var delusions = ent.Comp.Delusions;
        var excludedIds = delusions.Select(d => d.ProtoId).ToList();
        var index = _robustRandom.Next() % delusions.Count;

        if (!TryPickRandomDelusion(_baseDelusions, excludedIds, out var prototype))
            return;

        delusions[index] = _proto.Index<DelusionPrototype>(prototype);
        _delusionalSystem.SetDelusions(ent, delusions, notify);
    }

    private Delusion PickRandomDelusion()
    {
        var id = _robustRandom.Pick(_proto.Index(_baseDelusions).Delusions);
        return _proto.Index(id);
    }

    private bool TryPickRandomDelusion(ProtoId<DelusionDatabasePrototype> database, List<ProtoId<DelusionPrototype>?> excluded, [NotNullWhen(true)] out DelusionPrototype? prototype)
    {
        var choices = _proto.Index(database).Delusions.ToList();
        while (choices.Count > 0)
        {
            var delusionId = _robustRandom.PickAndTake(choices);
            if (excluded.Contains(delusionId))
                continue;

            prototype = _proto.Index(delusionId);
            return true;
        }
        prototype = null;
        return false;
    }

    private void OnDelusionCrisisInit(Entity<DelusionCrisisTargetComponent> ent, ref MapInitEvent args)
    {
        var target = ent.Comp;
        var now =  IoCManager.Resolve<IGameTiming>().CurTime;

        target.TimeNextCrisis = now + target.DelayBetweenCrisis;
        target.TimeNextCrisis += _robustRandom.NextFloat(-1f, 1f) * target.DelayVariation;
    }
}

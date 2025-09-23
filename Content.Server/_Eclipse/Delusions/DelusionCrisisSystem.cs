using System.Linq;
using Content.Shared._Eclipse.Delusions;
using Content.Shared._Eclipse.Delusions.Components;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Dataset;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server._Eclipse.Delusions;


// TODO : Ensure no repetition inside delusion selection
// TODO : Implement delusion incompatibilities

public sealed class DelusionCrisisSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly DelusionalSystem _delusionalSystem = default!;

    private float previousEventTime = 0f;
    private float currentTime = 0f;

    // once again shouldn't be stored inside the system but will do for now.
    private static readonly List<ProtoId<DelusionPrototype>> _delusions = new List<ProtoId<DelusionPrototype>>
    {
        "DelusionKiller",
        "DelusionConspiracy",
        "DelusionClaustro",
        "DelusionAgo",
        "DelusionGlass",
        "DelusionClones",
        "DelusionGhosts",
        "DelusionWhisper",
        "DelusionChoosen",
        "DelusionGermo",
        "DelusionVertigo",
        "DelusionMessiah",
        "DelusionArtist",
        "DelusionAnimals",
        "DelusionRandom",
        "DelusionBenefactor",
        "DelusionPoison",
        "DelusionFashion",
        "DelusionPermanence",
        "DelusionInvincible",
        "DelusionMessage",
        "DelusionLight",
        "DelusionMhysteria",
        "DelusionSilicons",
        "DelusionCommand",
    };


    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        currentTime += frameTime;

        // assuming frameTime is expressed in seconds
        if (currentTime <= previousEventTime + 60)
            return;

        _adminLogger.Add(LogType.Mind, LogImpact.High, $"Delusions Crisis Attempted !");
        var query = EntityQueryEnumerator<DelusionCrisisTargetComponent>();
        while (query.MoveNext(out var entityUid, out var target))
        {
            DelusionCrisisTarget((entityUid, target));
        }
        previousEventTime = currentTime;

    }


    /// <summary>
    /// Randomly alters the delusions suffered by an entity
    /// </summary>
    public void DelusionCrisisTarget(Entity<DelusionCrisisTargetComponent> ent, bool adminlog = true)
    {
        var target = ent.Comp;

        var chance = target.Sensibility;

        if (target.Resistant || ! _robustRandom.Prob(chance))
            return;

        var del = EnsureComp<DelusionalComponent>(ent);
        var delusions = del.Delusions;

        if (del.Delusions.Count == 0 || (_robustRandom.Prob(target.ProbabilityAddDelusion) && del.Delusions.Count != del.MaxDelusionsCount))
        {
            var newDelusion = PickRandomDelusion();
            delusions.Add(newDelusion);
        }
        else
        {
            var newDelusion = PickRandomDelusion();
            delusions[_robustRandom.Next() % del.Delusions.Count] = newDelusion;
        }

        _delusionalSystem.SetDelusions((ent.Owner, del), delusions);

        if (adminlog)
            _adminLogger.Add(LogType.Mind, LogImpact.High, $"{ToPrettyString(ent)} had its delusions changed to TODO");
    }

    private Delusion PickRandomDelusion()
    {
        var ID = _robustRandom.Pick(_delusions);
        return _proto.Index(ID);
    }

    /// <summary>
    /// Pick a random delusion from a Delusion dataset
    /// </summary>
    /// <returns></returns>
    private string Pick(string name)
    {
        var dataset = _proto.Index<DatasetPrototype>(name);
        return _robustRandom.Pick(dataset.Values);
    }
}

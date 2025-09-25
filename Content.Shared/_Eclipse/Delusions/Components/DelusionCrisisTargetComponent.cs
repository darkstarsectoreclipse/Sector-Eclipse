using Robust.Shared.GameStates;

namespace Content.Shared._Eclipse.Delusions;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DelusionCrisisTargetComponent : Component
{
    /// <summary>
    /// Average delay between 2 crisis
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan DelayBetweenCrisis = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Maximum delay added or removed from the DelayBetweenCrisis to compute next crisis time
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan DelayVariation = TimeSpan.FromMinutes(2.5);

    /// <summary>
    /// Probability that a Delusion Crisis have an effect on the target
    /// </summary>
    [DataField, AutoNetworkedField]
    public float CrisisSuccessProbability = 0.5f;

    /// <summary>
    /// Probability that a Delusion Crisis add a new delusion instead of replacing one.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float AggravationProbability = 0.25f;


    /// <summary>
    /// If true, the target is not affected by Delusion Crisis.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Resistant = false;


    [DataField, AutoNetworkedField]
    public TimeSpan TimeNextCrisis = TimeSpan.Zero;
}

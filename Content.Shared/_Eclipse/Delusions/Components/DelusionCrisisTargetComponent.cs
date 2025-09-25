using Robust.Shared.GameStates;

namespace Content.Shared._Eclipse.Delusions;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DelusionCrisisTargetComponent : Component
{
    /// <summary>
    /// Sensibility of the target. Will be hit by (1/sensibility)
    /// </summary>
    //[DataField, AutoNetworkedField]
    //public float Sensibility = 0.2f;

    /// <summary>
    /// Average delay between 2 crisis, in seconds
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan DelayBetweenCrisis = TimeSpan.FromMinutes(5);

    /// <summary>
    /// The time to the next crisis in second will be random inside DelayBetweenCrisis +/- DelayVariation
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan DelayVariation = TimeSpan.FromMinutes(2.5);

    /// <summary>
    /// Probability that a Delusion Crisis have an effect on the target
    /// </summary>
    [DataField, AutoNetworkedField]
    public float CrisisSuccessProbability = 0.25f;

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
    /// <summary>
    /// Probability that a successful delusion crisis add a new delusion
    /// </summary>
    //[DataField, AutoNetworkedField]
    //public float ProbabilityAddDelusion = 0.2f;

    /// <summary>
    /// Probability that a successful crisis replace an existing delusion
    /// </summary>
    //[DataField, AutoNetworkedField]
    //public float ProbabilityReplaceDelusion = 0.2f;
}

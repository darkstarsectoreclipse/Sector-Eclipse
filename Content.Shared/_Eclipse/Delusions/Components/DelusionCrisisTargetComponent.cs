using Robust.Shared.GameStates;

namespace Content.Shared._Eclipse.Delusions;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DelusionCrisisTargetComponent : Component
{
    /// <summary>
    /// Sensibility of the target. Will be hit by (1/sensibility)
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Sensibility = 0.2f;

    /// <summary>
    /// Will cancel every crisis attempt made on the entity
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Resistant = false;

    /// <summary>
    /// Probability that a successful delusion crisis add a new delusion
    /// </summary>
    [DataField, AutoNetworkedField]
    public float ProbabilityAddDelusion = 0.2f;

    /// <summary>
    /// Probability that a successful crisis replace an existing delusion
    /// </summary>
    [DataField, AutoNetworkedField]
    public float ProbabilityReplaceDelusion = 0.2f;
}

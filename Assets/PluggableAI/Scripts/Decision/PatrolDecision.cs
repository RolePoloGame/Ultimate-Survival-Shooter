using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Patrol")]
public class PatrolDecision : Decision
{
    [SerializeField]
    private HealthDecision HealthDecision;

    protected override bool EvaluateResponse(StateController controller)
    {
        return HealthDecision.Decide(controller) && !controller.Flags.Hit && !controller.Flags.Arrived && !controller.Flags.Look;
    }
}

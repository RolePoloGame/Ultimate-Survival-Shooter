using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/HitOrArrived")]
public class HitOrArrivedDecision : Decision
{
    protected override bool EvaluateResponse(StateController controller)
    {
        return controller.Flags.Arrived || controller.Flags.Hit;
    }
}

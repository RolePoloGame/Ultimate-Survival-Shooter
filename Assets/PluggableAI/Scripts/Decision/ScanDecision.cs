using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Scan")]
public class ScanDecision : Decision
{
    [SerializeField]
    private bool RotationBased = true;
    [SerializeField]
    [ShowIf(nameof(RotationBased))]
    private float MaxRotation = 360.0f;
    protected override bool EvaluateResponse(StateController controller)
    {
        if (RotationBased)
            return Rotate(controller);
        return Scan(controller);
    }

    private bool Rotate(StateController controller)
    {
        bool rotationFinished = controller.Data.RotationElapsed >= MaxRotation;
        if (rotationFinished)
            controller.Flags.Arrived = false;
        return rotationFinished;
    }

    private bool Scan(StateController controller)
    {
        controller.NavMeshAgent.isStopped = true;
        controller.transform.Rotate(0, controller.stats.searchingTurnSpeed * Time.deltaTime, 0);
        return controller.CheckIfCountDownElapsed(controller.stats.searchDuration);
    }
}
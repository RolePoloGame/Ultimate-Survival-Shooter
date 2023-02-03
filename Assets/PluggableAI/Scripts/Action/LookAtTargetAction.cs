
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/LookAtTarget")]
public class LookAtTargetAction : Action
{
    public override void OnEnterState(StateController controller)
    {
        base.OnEnterState(controller);
        controller.Data.SavedRotation = controller.Data.RotatingElement.rotation.eulerAngles;

    }

    public override void OnExitState(StateController controller)
    {
        base.OnExitState(controller);
        controller.Data.RotatingElement.rotation = Quaternion.Euler(controller.Data.SavedRotation);
    }

    protected override void PerformActivity(StateController controller)
    {
        controller.Data.RotatingElement.transform.LookAt(controller.Data.Target);
    }
}

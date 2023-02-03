using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/AngularScan")]
public class AngularScan : Action
{
    public override void OnEnterState(StateController controller)
    {
        base.OnEnterState(controller);
        controller.Data.RotationElapsed = controller.stats.scanAngle / 2f;
        controller.Data.RotatingElement.transform.localRotation = Quaternion.identity;
    }
    protected override void PerformActivity(StateController controller)
    {
        float yAngle = GetAngle(controller);
        controller.Data.RotatingElement.transform.Rotate(0, yAngle, 0);
    }

    private float GetAngle(StateController controller)
    {
        float yAngle = Time.deltaTime * controller.stats.searchingTurnSpeed;
        controller.Data.RotationElapsed += yAngle;

        if (controller.Flags.InversedRotation)
            yAngle *= -1;

        if (controller.Data.RotationElapsed >= controller.stats.scanAngle)
        {
            controller.Flags.InversedRotation = !controller.Flags.InversedRotation;
            controller.Data.RotationElapsed = 0.0f;
        }
        return yAngle;
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Look")]
public class LookAction : Action
{
    protected override void PerformActivity(StateController controller)
    {
        float rotateValue = controller.stats.searchingTurnSpeed * Time.deltaTime;
        controller.Data.RotationElapsed += rotateValue;
        controller.transform.Rotate(0, rotateValue, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Health")]
public class HealthDecision : Decision
{
    [Range(0f, 1f)]
    [SerializeField]
    private float DecisionPercent = .25f;
    public override bool Decide(StateController controller)
    {
        bool success = controller.TryGetComponent(out TankHealth health);
        if (success)
            success = health.GetHealthPercentage() <= DecisionPercent;
        return success;
    }
}

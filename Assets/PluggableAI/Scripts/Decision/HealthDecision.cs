using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Health")]
public class HealthDecision : Decision
{
    [Range(0f, 1f)]
    [SerializeField]
    private float DecisionPercent = .25f;

    protected override bool EvaluateResponse(StateController controller)
    {
        float percentage = controller.Data.EnemyHealth.GetHealthPercentage();
        DebugLog($"{percentage}");
        return percentage <= DecisionPercent;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
public class ActiveStateDecision : Decision
{
    protected override bool EvaluateResponse(StateController controller)
    {
        bool chaseTargetIsActive = controller.Data.Target.gameObject.activeSelf;
        return chaseTargetIsActive;
    }
}
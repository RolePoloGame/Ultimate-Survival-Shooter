using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/RunToHeal")]
public class RunToHealAction : Action
{
    protected override void PerformActivity(StateController controller)
    {
        controller.NavMeshAgent.destination = controller.Data.SafeSpace.position;
        DebugLog($"{Vector3.Distance(controller.Data.SafeSpace.position, controller.transform.position)}");
        controller.NavMeshAgent.isStopped = false;
    }
}

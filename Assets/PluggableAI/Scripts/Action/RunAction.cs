using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Run")]
public class RunAction : Action
{
    protected override void PerformActivity(StateController controller)
    {
        Vector3 target = controller.Data.Target.position;
        Vector3 me = controller.transform.position;

        Vector3 direction = -1f * (target - me);

        controller.NavMeshAgent.destination = direction;
        controller.NavMeshAgent.isStopped = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Run")]
public class RunAction : Action
{
    public override void Act(StateController controller)
    {
        Vector3 target = controller.chaseTarget.position;
        Vector3 me = controller.transform.position;

        Vector3 direction = -1f * (target - me);

        controller.navMeshAgent.destination = direction;
        controller.navMeshAgent.isStopped = false;
    }
}

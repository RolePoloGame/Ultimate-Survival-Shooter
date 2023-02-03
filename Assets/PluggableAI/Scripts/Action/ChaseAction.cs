using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    protected override void PerformActivity(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        controller.NavMeshAgent.destination = controller.Data.Target.position;
        controller.NavMeshAgent.isStopped = false;
    }
}
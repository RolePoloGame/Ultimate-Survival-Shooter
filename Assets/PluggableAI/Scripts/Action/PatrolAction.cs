using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    protected override void PerformActivity(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        controller.NavMeshAgent.destination = controller.wayPointList[controller.Data.NextWayPoint].position;
        controller.NavMeshAgent.isStopped = false;

        if (controller.NavMeshAgent.remainingDistance <= controller.NavMeshAgent.stoppingDistance && !controller.NavMeshAgent.pathPending)
        {
            controller.Data.NextWayPoint = (controller.Data.NextWayPoint + 1) % controller.wayPointList.Count;
            controller.Flags.Arrived = true;
        }
    }
}
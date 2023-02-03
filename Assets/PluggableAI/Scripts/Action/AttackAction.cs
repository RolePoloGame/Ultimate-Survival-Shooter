using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    [SerializeField]
    private string[] tags = { "Player", "Zombie" };
    protected override void PerformActivity(StateController controller)
    {
        Attack(controller);
        //ObsoleteAttack(controller);
    }

    private void Attack(StateController controller)
    {
        if (controller.Data.EyeSight.target == null)
            return;

        if(!controller.CheckIfCountDownElapsed(controller.stats.attackRate))
            return;

        controller.Shooting.Target = controller.Data.Target;
        controller.Shooting.Fire(controller.stats.attackForce, controller.stats.attackRate);
        controller.Data.StateTimeElapsed = 0;

    }

    public void ObsoleteAttack(StateController controller)
    {
        if (Debugging) Debug.DrawRay(controller.Eyes.position, controller.Eyes.forward.normalized * controller.stats.attackRange, Color.red);

        bool sphereCastSuccess = Physics.SphereCast(
            controller.Eyes.position,
            controller.stats.lookSphereCastRadius,
            controller.Eyes.forward,
            out RaycastHit hit,
            controller.stats.attackRange);

        if (!sphereCastSuccess)
            return;

        if (!SearchForTag(ref hit))
            return;

        if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Zombie"))
            return;

        if (!controller.CheckIfCountDownElapsed(controller.stats.attackRate))
        {
            return;
        }

        controller.Shooting.Target = hit.collider.transform;
        controller.Shooting.Fire(controller.stats.attackForce, controller.stats.attackRate);
        controller.Data.StateTimeElapsed = 0;
    }

    private bool SearchForTag(ref RaycastHit hit)
    {
        bool tagExist = false;
        for (int i = 0; i < tags.Length; i++)
        {
            if (!hit.collider.CompareTag(tags[i]))
                continue;

            tagExist = true;
            break;
        }

        return tagExist;
    }
}
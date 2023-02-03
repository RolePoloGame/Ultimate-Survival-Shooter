using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/FindTagWithHealth")]
public class LookDecision : Decision
{
    GameObject _gameObject;
    [SerializeField]
    private string[] tags = { "Player" };

    public LayerMask m_TankMask;
    protected override bool EvaluateResponse(StateController controller)
    {
        return Look(controller);
    }

    private bool Look(StateController controller)
    {
        if (controller.Data.EyeSight.target == null)
            return false;

        bool hasHealthSystem = controller.Data.EyeSight.target.TryGetComponent(out EnemyHealth enemyHealth);
        if (!hasHealthSystem)
            return false;

        if (enemyHealth.IsDead)
            return false;

        controller.Data.Target = controller.Data.EyeSight.target;
        controller.Flags.Look = true;
        return true;

        //return ObsoleteMethod(controller);
    }

    private bool ObsoleteMethod(StateController controller)
    {
        Transform eyes = controller.Data.Eyes;

        if (Debugging) Debug.DrawRay(eyes.position, eyes.forward.normalized * controller.stats.lookRange, Color.green);
        controller.Flags.Look = false;

        bool sphereCastSuccess = Physics.SphereCast(
            eyes.position,
            controller.stats.lookSphereCastRadius,
            eyes.forward,
            out RaycastHit hit,
            controller.stats.lookRange,
            m_TankMask
            );
        DebugLog($"{sphereCastSuccess} => {eyes.position}");

        if (!sphereCastSuccess)
            return false;

        if (_gameObject == null)
        {
            _gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _gameObject.transform.SetParent(eyes);

            _gameObject.transform.localScale = controller.stats.lookSphereCastRadius * Vector3.one;
            _gameObject.transform.localPosition = Vector3.zero;
            _gameObject.GetComponent<SphereCollider>().enabled = false;
            Destroy(_gameObject, 2.0f);
        }

        DebugLog($"<b>Comparing... {_gameObject.transform.position} == {eyes.position} == {hit.point}</b>");
        DebugLog($"Checking {hit.collider.name} for Tag");
        if (!SearchForTag(ref hit))
            return false;

        DebugLog($"Checking {hit.collider.name} for health...");
        bool hasHealthSystem = hit.collider.gameObject.TryGetComponent(out EnemyHealth enemyHealth);
        if (!hasHealthSystem)
            return false;
        DebugLog($"Checking {hit.collider.name} if alive...");

        if (enemyHealth.IsDead)
            return false;

        DebugLog($"{hit.collider.name} is alive. Last mistake...");
        controller.Data.Target = hit.transform;
        controller.Flags.Look = true;
        return true;
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
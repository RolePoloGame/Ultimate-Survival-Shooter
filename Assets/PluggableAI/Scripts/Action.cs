using UnityEngine;

public abstract class Action : ScriptableObject
{
    [SerializeField]
    protected bool Debugging = false;
    public void Act(StateController controller)
    {
        if (Debugging) Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.cyan)}>◔</color> {name}");
        PerformActivity(controller);
    }
    protected abstract void PerformActivity(StateController controller);

    protected void DebugLog(string msg)
    {
        if (!Debugging)
            return;
        Debug.Log(msg);
    }

    public virtual void OnEnterState(StateController controller)
    {
        DebugLog($"<b>OnEnterState</b>");
    }

    public virtual void OnExitState(StateController controller)
    {
        DebugLog($"<b>OnExitState</b>");
    }
}
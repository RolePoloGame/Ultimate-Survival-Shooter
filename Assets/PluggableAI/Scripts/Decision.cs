using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : ScriptableObject
{
    [SerializeField]
    protected bool Debugging = false;
    protected bool Response = false;
    public bool Decide(StateController controller)
    {
        Response = EvaluateResponse(controller);
        if (Debugging) Debug.Log($"{name} => {Response}");
        return Response;
    }

    protected abstract bool EvaluateResponse(StateController controller);

    protected void DebugLog(string msg)
    {
        if (!Debugging) return;
        Debug.Log(msg);
    }
}
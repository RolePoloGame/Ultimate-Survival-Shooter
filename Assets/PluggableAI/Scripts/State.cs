using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public class State : ScriptableObject
{
    [SerializeField]
    private bool Debugging = true;
    public Action[] actions;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions(StateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            Decision decision = transitions[i].decision;
            bool decisionSucceeded = decision.Decide(controller);
            Color color = decisionSucceeded ? Color.green : Color.red;
            if (Debugging) Debug.Log($"Deciding: <color=#{ColorUtility.ToHtmlStringRGB(color)}>{decision.name}</color>");

            State nextState = decisionSucceeded ? transitions[i].trueState : transitions[i].falseState;
            controller.TransitionToState(nextState);

            if (!controller.IsRemainInState(nextState))
                break;
        }
    }

    public void FirstAct(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].OnEnterState(controller);
        }
    }

    public void LastAct(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].OnExitState(controller);
        }
    }
}
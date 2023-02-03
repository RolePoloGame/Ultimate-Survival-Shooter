using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using NaughtyAttributes;

public class StateController : MonoBehaviour
{
    [SerializeField]
    private bool Debugging = false;

    [BoxGroup("AI - State")]
    public State startState;

    [BoxGroup("AI - State")]
    [SerializeField]
    [ReadOnly]
    protected State currentState;

    [BoxGroup("AI - State")]
    public EnemyStats stats;

    [BoxGroup("AI - State")]
    public State remainState;
    [BoxGroup("AI - State")]
    public List<Transform> wayPointList;

    public Transform Eyes => Data.Eyes;

    #region Properties

    public NavMeshAgent NavMeshAgent { get => LoadNavMeshAgent(); set => m_navMeshAgent = value; }
    [Header("Components")]
    public NavMeshAgent m_navMeshAgent;

    public EnemyShooting Shooting { get => LoadTankShooting(); set => m_shooting = value; }
    public EnemyShooting m_shooting;

    public AIFlags Flags { get => m_flags; set => m_flags = value; }
    [SerializeField]
    private AIFlags m_flags;

    public AIData Data { get => m_data; set => m_data = value; }
    [SerializeField]
    private AIData m_data;

    #endregion

    void Awake()
    {
        SetupAI(true, wayPointList);
        TransitionToState(startState);
    }

    #region Load
    private NavMeshAgent LoadNavMeshAgent()
    {
        if (m_navMeshAgent == null)
            m_navMeshAgent = GetComponent<NavMeshAgent>();
        return m_navMeshAgent;
    }

    private EnemyShooting LoadTankShooting()
    {
        if (m_shooting == null)
            m_shooting = GetComponent<EnemyShooting>();
        return m_shooting;
    }
    #endregion

    public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
    {
        wayPointList = wayPointsFromTankManager;
        Flags.Active = aiActivationFromTankManager;
        NavMeshAgent.enabled = Flags.Active;
        Shooting.MaxDistance = Data.EyeSight.EyesightSize;
        currentState = remainState;
    }

    void Update()
    {
        if (!Flags.Active)
            return;
        UpdateFlagsFromData();
        if (currentState == null)
            return;
        currentState.UpdateState(this);
    }

    private void UpdateFlagsFromData()
    {
        Flags.Hit = Data.EnemyHealth.WasHit();
    }

    void OnDrawGizmos()
    {
        if (currentState == null || Eyes == null)
            return;
        if (!Debugging)
            return;
        Gizmos.color = currentState.sceneGizmoColor;
        Gizmos.DrawWireSphere(Eyes.position, stats.lookSphereCastRadius);
    }

    public void TransitionToState(State nextState)
    {
        if (nextState == remainState)
            return;

        currentState.LastAct(this);
        currentState = nextState;
        OnExitState();
        currentState.FirstAct(this);

        if (Debugging) Debug.Log($"Running: [<color=#{ColorUtility.ToHtmlStringRGB(nextState.sceneGizmoColor)}>●</color>]{currentState.name}");
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        Data.StateTimeElapsed += Time.deltaTime;
        return (Data.StateTimeElapsed >= duration);
    }

    public bool IsRemainInState(State nextState) => remainState == nextState;

    private void OnExitState()
    {
        Data.StateTimeElapsed = 0;
        Data.RotationElapsed = 0;
    }

    [Serializable]
    public class AIFlags
    {
        public bool Active = true;
        public bool Arrived = false;
        public bool Hit = false;
        public bool Look = false;
        public bool InversedRotation = false;
    }

    [Serializable]
    public class AIData
    {
        public Transform Target = null;
        public int NextWayPoint;
        public float StateTimeElapsed;
        public float RotationElapsed;
        public Transform RotatingElement;
        public Transform Aim;
        public Transform Eyes;
        public EyeSight EyeSight;
        public Vector3 SavedRotation;
        public Transform SafeSpace;

        public EnemyHealth EnemyHealth;

    }
}
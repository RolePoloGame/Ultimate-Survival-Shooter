using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 1;
    [Header("Look")]
    public float lookRange = 40f;
    public float lookSphereCastRadius = 1f;

    [Header("Attack")]
    public float attackRange = 1f;
    public float attackRate = 1f;
    public float attackForce = 15f;
    public int attackDamage = 50;

    [Header("Search")]
    public float searchDuration = 4f;
    public float searchingTurnSpeed = 120f;
    [Range(0.0f, 360.0f)]
    public float scanAngle = 120.0f;
}
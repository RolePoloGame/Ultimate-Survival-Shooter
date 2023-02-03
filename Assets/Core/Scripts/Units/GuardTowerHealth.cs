using Core.Systems.Components.Systems;
using UnityEngine;

public class GuardTowerHealth : EnemyHealth, IHealthSystem
{
    public HealthSystem HealthSystem => new HealthSystem(m_StartingHealth);

    public void Heal(float value, Vector3 point)
    {

    }

    public void Hurt(float value, Vector3 point)
    {

    }
}

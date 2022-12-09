using Core.Systems.Components.Systems;
using UnityEngine;

public interface IHealthSystem
{
    void Hurt(float value, Vector3 point);
    void Heal(float value, Vector3 point);
    HealthSystem HealthSystem { get; }
}

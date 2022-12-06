using UnityEngine;

public abstract class EnemyHealth : MonoBehaviour
{
    public virtual void TakeDamage(float amount, Vector3 hitPoint) { }
}

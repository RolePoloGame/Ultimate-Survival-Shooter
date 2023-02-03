using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyHealth : MonoBehaviour
{
    #region Health
    //To Do: Implement HealthSystem here
    public float m_StartingHealth = 100f;               // The amount of health starts with.
    protected float CurrentHealth { get => m_Health; set => Hit(value); }                      // How much health currently has.
    private float m_Health = 100f; 
    #endregion

    public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
    public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.

    #region Got Hit
    public float GotHitMemory = 10.0f;
    private float m_HitTimeElapsed = 0.0f;
    private bool m_Hit = false; 
    #endregion

    #region Public Methods
    public bool WasHit() => m_Hit;
    public virtual bool IsDead { get => CurrentHealth <= 0.0f; }
    public virtual void TakeDamage(float amount, Vector3 hitPoint) { }
    public float GetHealthPercentage() => CurrentHealth / m_StartingHealth;
    #endregion

    #region Private Methods

    private void Hit(float value)
    {
        if (value < m_Health)
        {
            StopAllCoroutines();
            StartCoroutine(GotHit());
        }

        m_Health = value;
    }
    private IEnumerator GotHit()
    {
        m_HitTimeElapsed = 0.0f;
        m_Hit = true;
        while (m_HitTimeElapsed < GotHitMemory)
        {
            m_HitTimeElapsed += Time.deltaTime;
            yield return null;
        }
        m_Hit = false;
    } 
    #endregion

}

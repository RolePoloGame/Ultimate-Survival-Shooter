using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Core.Systems.Components.Systems;

public class TankHealth : EnemyHealth, IHealthSystem
{
    public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
    public Image m_FillImage;                           // The image component of the slider.
    public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.

    private AudioSource m_ExplosionAudio;               // The audio source to play when the tank explodes.
    private ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroyed.
    private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?

    public HealthSystem HealthSystem => new(m_StartingHealth);

    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        CurrentHealth = m_StartingHealth;
        m_Dead = false;
        SetHealthUI();
    }

    public override void TakeDamage(float amount, Vector3 hitPoint)
    {
        CurrentHealth -= amount;
        SetHealthUI();
        if (CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }
    private void SetHealthUI()
    {
        // Set the slider's value appropriately.
        m_Slider.value = GetHealthPercentage();

        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, CurrentHealth / m_StartingHealth);
    }

    private void OnDeath()
    {
        // Set the flag so that this function is only called once.
        m_Dead = true;

        // Move the instantiated explosion prefab to the tank's position and turn it on.
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        // Play the particle system of the tank exploding.
        m_ExplosionParticles.Play();

        // Play the tank explosion sound effect.
        m_ExplosionAudio.Play();

        // Turn the tank off.
        gameObject.SetActive(false);
    }

    public void Hurt(float value, Vector3 point) => TakeDamage(value, point);

    public void Heal(float value, Vector3 point) => TakeDamage(-value, point);
}
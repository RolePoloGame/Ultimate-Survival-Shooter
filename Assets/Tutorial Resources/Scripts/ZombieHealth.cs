using Core.Systems.Components.Systems;
using Core.World;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieHealth : EnemyHealth, IHealthSystem
{
    #region Public Fields
    public HealthSystem HealthSystem { get => healthSystem ??= new HealthSystem(StartingHealth); set => healthSystem = value; }
    private HealthSystem healthSystem;

    public float StartingHealth = 100.0f;
    [SerializeField]
    private int ScoreValue = 10;
    [SerializeField]
    private int BackupAmmo = 10;
    #endregion

    #region Private Fields
    [SerializeField]
    private float sinkSpeed = 2.5f;
    [SerializeField]
    private float destroyTime = 2.0f;

    [SerializeField]
    private AudioClip deathClip;
    [SerializeField]
    private Slider Slider;
    [SerializeField]
    private Image FillImage;
    [SerializeField]
    private Color FullHealthColor = Color.green;
    [SerializeField]
    private Color ZeroHealthColor = Color.red;
    #endregion

    #region Components
    private Animator anim;
    private AudioSource audioSource;
    private ParticleSystem hitParticles;
    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;
    private NavMeshAgent meshAgent;

    #region Getters
    private Rigidbody GetRigidbody()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        return rb;
    }
    private NavMeshAgent GetNavMeshAgent()
    {
        if (meshAgent == null) meshAgent = GetComponent<NavMeshAgent>();
        return meshAgent;
    }
    private Animator GetAnimator()
    {
        if (anim == null) anim = GetComponent<Animator>();
        return anim;
    }
    private AudioSource GetAudioSource()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        return audioSource;
    }
    private ParticleSystem GetHitParticles()
    {
        if (hitParticles == null) hitParticles = GetComponentInChildren<ParticleSystem>();
        return hitParticles;
    }
    private CapsuleCollider GetCapsuleCollider()
    {
        if (capsuleCollider == null) capsuleCollider = GetComponent<CapsuleCollider>();
        return capsuleCollider;
    }

    #endregion

    #endregion

    #region Tags
    private bool isDeadInitiated;
    private bool isSinking;
    #endregion

    #region Unity Methods

    void Awake()
    {
        Initiate();
    }


    void Update()
    {
        if (!isSinking)
            return;
        Sink();
    }
    #endregion

    #region Private Methods
    private void Initiate()
    {
        HealthSystem = new HealthSystem(StartingHealth);
        UpdateHealthUI();
    }

    private void Sink()
    {
        transform.Translate(sinkSpeed * Time.deltaTime * Vector3.down);
    }

    private void UpdateHealthUI()
    {
        Slider.value = HealthSystem.HealthPercentage;
        FillImage.color = Color.Lerp(ZeroHealthColor, FullHealthColor, HealthSystem.HealthPercentage);
    }
    private void Death()
    {
        isDeadInitiated = true;
        ScoreManager.score += ScoreValue;
        PlayerShooting.AmmoSystem.AddAmmo(BackupAmmo);
        GetCapsuleCollider().isTrigger = true;
        GetAnimator().SetTrigger("Dead");
        GetAudioSource().clip = deathClip;
        GetAudioSource().Play();
    }
    #endregion

    #region Public Methods
    public override void TakeDamage(float amount, Vector3 hitPoint)
    {
        if (isDeadInitiated)
            return;

        GetAudioSource().Play();
        HealthSystem.Hurt(amount);
        GetHitParticles().transform.position = hitPoint;
        GetHitParticles().Play();
        UpdateHealthUI();
        if (HealthSystem.IsDead && !isDeadInitiated)
        {
            Death();
        }
    }

    public void StartSinking()
    {
        GetNavMeshAgent().enabled = false;
        GetRigidbody().isKinematic = true;
        isSinking = true;
        DisableMinimapIcon();
        Destroy(gameObject, destroyTime);
    }

    private void DisableMinimapIcon()
    {
        GetComponentInChildren<MinimapIcon>().gameObject.SetActive(false);
    }

    public void Hurt(float value, Vector3 point) => TakeDamage(value, point);
    public void Heal(float value, Vector3 point) => HealthSystem.Heal(value);
    #endregion
}

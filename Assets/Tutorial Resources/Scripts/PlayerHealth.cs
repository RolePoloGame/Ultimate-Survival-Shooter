using UnityEngine.UI;
using UnityEngine;
using Core.Systems.Components.Systems;
using UnityEngine.AI;

public class PlayerHealth : MonoBehaviour, IHealthSystem
{
    [SerializeField]
    private float StartingHealth = 100;                            // The amount of health the player starts the game with.
    [SerializeField]
    private Slider healthSlider;                                 // Reference to the UI's health bar.
    [SerializeField]
    private Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    [SerializeField]
    private AudioClip deathClip;                                 // The audio clip to play when the player dies.
    [SerializeField]
    private float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    [SerializeField]
    private Color flashColour = new(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
                                                           // Reference to the Animator component.
    #region Components
    private PlayerMovement playerMovement;                              // Reference to the player's movement.
    private PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.                                              // True when the player gets isDamaged.
    private Animator anim;
    private AudioSource audioSource;
    private ParticleSystem hitParticles;
    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;
    public HealthSystem HealthSystem { get => healthSystem; set => healthSystem = value; }
    private HealthSystem healthSystem;
    #region Getters
    private Rigidbody GetRigidbody()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        return rb;
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
        if (hitParticles == null) hitParticles = GetComponent<ParticleSystem>();
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
    private bool isDeathInitated;
    private bool isDamaged;
    #endregion

    #region Unity Methods
    void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        HealthSystem = new HealthSystem(StartingHealth);
    }
    void Update()
    {
        // If the player has just been isDamaged...
        if (isDamaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the isDamaged flag.
        isDamaged = false;
    }
    #endregion

    #region Public Methods
    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        TakeDamage(amount);
    }
    public void TakeDamage(int amount)
    {
        isDamaged = true;
        HealthSystem.Hurt(amount);
        UpdateHealthGUI();
        audioSource.Play();

        if (HealthSystem.IsDead && !isDeathInitated)
        {
            Death();
        }
    }

    public void Heal(int healingValue)
    {
        HealthSystem.Heal(healingValue);
        UpdateHealthGUI();
    }

    #endregion

    #region Private Methods
    void UpdateHealthGUI()
    {
        healthSlider.value = HealthSystem.HealthPercentage;
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDeathInitated = true;

        // Turn off any remaining shooting effects.
        playerShooting.DisableEffects();

        // Tell the animator that the player is dead.
        GetAnimator().SetTrigger("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        GetAudioSource().clip = deathClip;
        GetAudioSource().Play();

        // Turn off the movement and shooting scripts.
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    public void Hurt(float value, Vector3 point) => HealthSystem.Hurt(value);
    #endregion
}

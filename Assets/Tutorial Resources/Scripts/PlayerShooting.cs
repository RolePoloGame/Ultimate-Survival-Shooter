using UnityEngine;
using TMPro;
using Core.Systems.Components.Systems;
using UnityEngine.UI;
using Core.GUI.SegmentRing;

public class PlayerShooting : MonoBehaviour
{
    #region Shooting Settings
    [SerializeField]
    private int damagePerShot = 20;
    [SerializeField]
    private float timeBetweenBullets = 0.15f;
    [SerializeField]
    private float range = 100f;
    [SerializeField]
    private float effectsDisplayTime = 0.2f;

    #endregion
    public static AmmoSystem AmmoSystem { get => ammoSystem ??= new AmmoSystem(30, 60); set => ammoSystem = value; }
    public static AmmoSystem ammoSystem;

    #region UI
    [SerializeField]
    private TextMeshProUGUI currentAmmoTMP;
    [SerializeField]
    private TextMeshProUGUI magazineAmmoTMP;
    [SerializeField]
    private TextMeshProUGUI backupAmmoTMP;
    [SerializeField]
    private Image ammoRingIM;
    [SerializeField]
    private SegmentRing segmentUI;
    #endregion

    private float timer;
    private Ray shootRay;
    private RaycastHit shotHit;
    private int shootableMask;

    #region Components

    private ParticleSystem gunParticles;
    private LineRenderer gunLine;
    private AudioSource audioSource;
    private Light gunLight;
    private ParticleSystem GetGunParticles()
    {
        if (gunParticles == null) gunParticles = GetComponent<ParticleSystem>();
        return gunParticles;
    }
    private LineRenderer GetGunLine()
    {
        if (gunLine == null) gunLine = GetComponent<LineRenderer>();
        return gunLine;
    }
    private AudioSource GetAudioSource()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        return audioSource;
    }
    private Light GetLight()
    {
        if (gunLight == null) gunLight = GetComponent<Light>();
        return gunLight;
    }

    #endregion

    #region Tags

    private bool isFiring;
    #endregion

    #region Unity Methods
    void Awake()
    {
        Initialize();
        UpdateMagazineUI();
        UpdateUI();
    }

    void Update()
    {
        GetTimerAllowShoot();
        UpdateUI();
        ControlShooting();
        ControlReloading();
    }
    #endregion
    #region Private Methods
    private void Initialize()
    {
        shootableMask = LayerMask.GetMask("Shootable") | LayerMask.GetMask("Tank");
    }

    private void ControlReloading()
    {
        if (!Input.GetKey(KeyCode.R))
            return;

        AmmoSystem.Reload();
    }

    private void ControlShooting()
    {
        if (!AmmoSystem.CanShoot)
            return;
        if (isFiring)
            return;
        if (!GetTimerAllowShoot())
            return;
        if (!Input.GetButton("Fire1"))
            return;
        Shoot();
    }

    private bool GetTimerAllowShoot()
    {
        if (timer <= timeBetweenBullets - (timeBetweenBullets * effectsDisplayTime))
            DisableEffects();

        if (timer <= 0.0f)
            return true;
        timer -= Time.deltaTime;
        return false;
    }

    private void UpdateTimer()
    {
        timer = timeBetweenBullets;
    }

    private void UpdateUI()
    {
        currentAmmoTMP.SetText(AmmoSystem.CurrentAmmo.ToString());
        backupAmmoTMP.SetText(AmmoSystem.BackupAmmo.ToString());
        ammoRingIM.fillAmount = AmmoSystem.AmmoPercentage;
        segmentUI.SetVisibleSegments((int)AmmoSystem.CurrentAmmo);
    }

    private void UpdateMagazineUI()
    {
        magazineAmmoTMP.SetText(AmmoSystem.MagazineSize.ToString());
        segmentUI.GenerateRings((int)AmmoSystem.MagazineSize);
    }

    public void DisableEffects()
    {
        GetGunLine().enabled = false;
        GetLight().enabled = false;
    }

    private void Shoot()
    {
        isFiring = true;
        AmmoSystem.Shoot();
        GetAudioSource().Play();
        UpdateTimer();
        GetLight().enabled = true;
        GetGunParticles().Stop();
        GetGunParticles().Play();
        RaycastHit();
        isFiring = false;
    }

    private void RaycastHit()
    {
        GetGunLine().enabled = true;
        GetGunLine().SetPosition(0, transform.position);
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        bool raycastHit = Physics.Raycast(shootRay, out shotHit, range, shootableMask);
        if (raycastHit)
        {
            if (shotHit.collider.TryGetComponent(out EnemyHealth enemyHealth))
                enemyHealth.TakeDamage(damagePerShot, shotHit.point);
            GetGunLine().SetPosition(1, shotHit.point);
            return;
        }
        GetGunLine().SetPosition(1, shootRay.origin + shootRay.direction * range);
    } 
    #endregion
}

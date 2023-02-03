using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using UnityEditor;
using Core.GUI.Healthbar;
using Core.Managers.Animators;
using Core.World;
/**
* ? Wartoœci jednorazowej regeneracji
* ? Ca³kowita iloœæ zregenerowanego HP (lub brak limitu), po której nastêpuje dezaktywacja obiekt regeneruj¹cego.
* ? Limit u¿yæ (lub brak), po którym nastêpuje dezaktywacja obiekt regeneruj¹cego.
* ? Czas do pierwszego uruchomienia
* ? Czas pomiêdzy kolejnymi u¿yciami
* ? Maksymaln¹ iloœæ jednoczeœnie uzdrawianych jednostek
* ? Typy akceptowanych jednostek
**/
namespace Core.Systems.Stations
{
    [SelectionBase]
    public class HealingStation : MonoBehaviour
    {
        #region Settings

        #region General Settings

        [BoxGroup("Settings")]
        [SerializeField]
        private bool ShowSettings;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private float Radius = 5f;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private float HealValue = 5f;


        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private float HealInterval = 5f;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(EConditionOperator.And, nameof(ShowSettings), nameof(DestroyOnDeactivatation))]
        private float DissapearTime = 5f;

        #endregion

        #region Heal Limit
        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private bool HasHealLimit;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(EConditionOperator.And, nameof(ShowSettings), nameof(HasHealLimit))]
        private float HealLimit;
        #endregion

        #region Use Limit
        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private bool HasUseLimit;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(EConditionOperator.And, nameof(ShowSettings), nameof(HasUseLimit))]
        private float UseLimit;
        #endregion

        #region Activation Time
        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private bool HasActivationTimer;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(EConditionOperator.And, nameof(ShowSettings), nameof(HasActivationTimer))]
        private float ActivationTime;
        #endregion

        #region Cooldown Time
        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private bool HasHealCooldown;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(EConditionOperator.And, nameof(ShowSettings), nameof(HasHealCooldown))]
        private float HealCooldown;
        #endregion

        #region Who Can use

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private bool CanPlayerUse;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private bool CanZombieUse;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private bool CanTankUse;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private bool HasMaxUsers = false;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(EConditionOperator.And, nameof(ShowSettings), nameof(HasMaxUsers))]
        private uint MaxUsers;

        #endregion

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private bool DestroyOnDeactivatation;

        #region Visualization
        [BoxGroup("Visualization")]
        [SerializeField]
        private bool VisualizeColider;

        [BoxGroup("Visualization")]
        [SerializeField]
        [ShowIf(EConditionOperator.And, nameof(ShowSettings), nameof(VisualizeColider))]
        [ColorUsage(false)]
        private Color VisualizationColor = Color.white;
        #endregion

        #endregion

        #region Fields

        private SphereCollider _colider;

        private List<IHealthSystem> healable = new();

        private float CooldownTimer = 0.0f;
        private float ActivationTimer = 0.0f;
        private float IntervalTimer = 0.0f;

        private bool Cooldown = false;
        private bool Activated = false;

        private float HealedAmount = 0.0f;
        private float UseAmount = 0.0f;

        private bool Deactivate = false;

        #endregion

        #region Components
        [SerializeField]
        [Required]
        private MinimapIcon minimapIcon;

        private ParticleSystem healParticles;
        private ParticleSystem GetHealParticles()
        {
            if (healParticles == null) healParticles = GetComponentInChildren<ParticleSystem>();
            return healParticles;
        }
        private RingHealthbarControler timerUI;
        private RingHealthbarControler GetTimerUI()
        {
            if (timerUI is null) timerUI = GetComponentInChildren<RingHealthbarControler>();
            return timerUI;
        }
        private SimpleAnimatorControler animatorControler;
        private SimpleAnimatorControler GetAnimatorControler()
        {
            if (animatorControler is null) animatorControler = GetComponent<SimpleAnimatorControler>();
            return animatorControler;
        }
        #endregion

        #region Unity Methods

        private void Update()
        {
            if (IsToBeActivated())
                return;

            if (IsOnCooldown())
                return;

            if (ReachedHealLimit())
                return;

            if (ReachedUseLimit())
                return;

            if (!CheckHealInterval())
                return;

            if (healable.Count == 0)
                return;
            Heal();
        }

        private void OnEnable()
        {
            UpdateRadius();
        }

        private void OnTriggerEnter(Collider other)
        {
            ///ToDo: Better Implementation
            if (!other.TryGetComponent(out IHealthSystem health))
                return;

            bool isPlayer = other.TryGetComponent<PlayerHealth>(out _);
            if (isPlayer && !CanPlayerUse) return;

            bool isZombie = other.TryGetComponent<ZombieHealth>(out _);
            if (isZombie && !CanZombieUse) return;

            bool isTank = other.TryGetComponent<TankHealth>(out _);
            if (isTank && !CanTankUse) return;

            if (!(isPlayer || isZombie || isTank))
                return;

            AddHealable(health);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out IHealthSystem health))
                return;
            RemoveHealable(health);
        }

        private void OnDrawGizmosSelected()
        {
            if (!VisualizeColider) return;
            Gizmos.color = VisualizationColor;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods

        private bool CheckMaxUsers()
        {
            if (!HasMaxUsers)
                return false;
            return healable.Count >= MaxUsers;
        }

        private void AddHealable(IHealthSystem health)
        {
            if (CheckMaxUsers()) return;
            healable.Add(health);
        }

        private void RemoveHealable(IHealthSystem health)
        {
            healable.Remove(health);
        }

        private void UpdateRadius()
        {
            GetCollider().radius = Radius;
        }

        private SphereCollider GetCollider()
        {
            if (_colider == null)
                _colider = GetComponent<SphereCollider>();
            return _colider;
        }
        private bool CheckHealInterval()
        {
            if (IntervalTimer <= 0.0f)
            {

                if (GetAnimatorControler().IsActive)
                    GetAnimatorControler().Deactivate();
                return true;
            }
            IntervalTimer -= Time.deltaTime;
            GetTimerUI().SetValue((IntervalTimer / HealInterval));
            return false;
        }

        private bool ReachedUseLimit()
        {
            if (!HasUseLimit)
                return false;
            bool reached = UseAmount >= UseLimit;
            if (reached)
                Cooldown = true;
            return reached;
        }

        private bool ReachedHealLimit()
        {
            if (!HasHealLimit)
                return false;
            bool reached = HealedAmount >= HealLimit;
            if (reached)
                Cooldown = true;
            return reached;
        }

        private bool IsToBeActivated()
        {
            bool isStartActivated = ManageActivationTimer();
            return isStartActivated;
        }

        private bool ManageActivationTimer()
        {
            if (!HasActivationTimer)
                return false;

            if (Activated)
                return false;

            if (ActivationTimer >= ActivationTime)
            {
                Activated = true;
                ActivationTimer = 0.0f;
                return false;
            }
            ActivationTimer += Time.deltaTime;
            return true;
        }

        private bool IsOnCooldown()
        {
            if (Cooldown && DestroyOnDeactivatation)
            {
                Deactivate = true;
                minimapIcon.gameObject.SetActive(false);
                StartCoroutine(DeactivateSelfAfter(DissapearTime));
            }

            if (!HasHealCooldown)
                return false;

            if (!Cooldown)
                return false;

            if (CooldownTimer >= HealCooldown)
            {
                ResetCounters();
                if (Deactivate)
                {
                    DeactivateSelf();
                }

                Cooldown = false;
                return false;
            }

            CooldownTimer += Time.deltaTime;
            return true;
        }

        private IEnumerator DeactivateSelfAfter(float healCooldown)
        {
            float timer = 0.0f;
            while (timer < healCooldown)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            if (Deactivate)
            {
                DeactivateSelf();
            }
        }

        private void DeactivateSelf()
        {
            gameObject.SetActive(false);
        }

        private void ResetCounters()
        {
            CooldownTimer = 0.0f;
            HealedAmount = 0.0f;
            UseAmount = 0.0f;
        }

        private void Heal()
        {
            if (healable.Count == 0)
                return;

            for (int i = 0; i < healable.Count; i++)
            {
                IHealthSystem healthSystem = healable[i];
                healthSystem.Heal(HealValue, Vector3.one);
            }

            HealedAmount += HealValue * healable.Count;
            UseAmount++;
            RestartInterval();
            PlayHealEffect();
            GetAnimatorControler().Activate();
        }

        private void RestartInterval()
        {
            IntervalTimer = HealInterval;
        }

        private void PlayHealEffect()
        {
            GetHealParticles().gameObject.SetActive(true);
            GetHealParticles().Play();
        }
        #endregion

    }
}
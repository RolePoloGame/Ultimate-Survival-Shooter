using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using UnityEditor;
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
        [ShowIf(EConditionOperator.And, nameof(ShowSettings), nameof(HasHealLimit))]
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
        [ShowIf(nameof(ShowSettings))]
        private float HealCooldown; 
        #endregion

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        private bool DestroyOnDeactivatation;

        #region Visualization
        [BoxGroup("Visualization")]
        [SerializeField]
        private bool VisualizeColider;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf(nameof(ShowSettings))]
        [ColorUsage(false)]
        private Color VisualizationColor = Color.white; 
        #endregion

        #endregion

        #region Fields

        private SphereCollider _colider;

        private List<IHealthSystem> healable = new();

        private float CooldownTimer = 0.0f;
        private float ActivationTimer = 0.0f;

        private bool Cooldown = false;
        private bool Activated = false;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (IsToBeActivated())
                return;

            if (IsOnCooldown())
                return;

            Heal();
        }

        private bool IsToBeActivated()
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
            if (!HasHealCooldown)
                return false;

            if (!Cooldown)
                return false;

            if (CooldownTimer >= HealCooldown)
            {
                CooldownTimer = 0.0f;
                Cooldown = false;
                return false;
            }

            CooldownTimer += Time.deltaTime;
            return true;
        }

        private void Heal()
        {
            if (healable.Count == 0)
                return;

            float healed = 0;
            foreach(IHealthSystem healthSystem in healable)
            {
                //HealthSystem.HealthSystem.HealDamage(HealValue);
            }

        }

        private void OnEnable()
        {
            UpdateRadius();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHealthSystem health))
                return;
            AddHealable(health);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = VisualizationColor;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods

        private void AddHealable(IHealthSystem health) => healable.Add(health);
        private void RemoveHealable(IHealthSystem health) => healable.Remove(health);
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

        #endregion

    }
}
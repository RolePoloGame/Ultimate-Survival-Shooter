using System;
using UnityEngine;

namespace Core.Systems.Components.Systems
{
    [Serializable]
    public class HealthSystem
    {
        #region Constructors
        public HealthSystem(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            TemporaryMaxHealth = 0.0f;
        }

        public HealthSystem(float temporaryMaxHealth, float maxHealth) : this(maxHealth)
        {
            TemporaryMaxHealth = temporaryMaxHealth;
        }
        #endregion

        #region Fields
        public float TemporaryMaxHealth { get; private set; }
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public float MinHealth { get; private set; } = 0.0f;
        public float HealthPercentage => CurrentHealth / (MaxHealth + TemporaryMaxHealth);
        public bool IsDead => CurrentHealth <= MinHealth;
        #endregion

        #region Public Methods
        public void AddTemporary(float value)
        {
            TemporaryMaxHealth += value;
            Heal(value);
        }
        public void AddMax(float value)
        {
            MaxHealth += value;
            Heal(value);
        }
        public void RemoveMax(float value)
        {
            MaxHealth -= value;
            if (MaxHealth <= MinHealth)
                MaxHealth = MinHealth;
            Hurt(value);
        }
        public void SetMax(float value)
        {
            float _diff = value - MaxHealth;
            MaxHealth = value;
            Heal(_diff);
        }
        public void SetTemporary(float value)
        {
            float _diff = value - TemporaryMaxHealth;
            TemporaryMaxHealth = value;
            Heal(_diff);
        }
        public void SetMin(float value) => MinHealth = value;
        public void Heal(float value)
        {
            float _currValue = CurrentHealth;
            _currValue += value;
            _currValue = Mathf.Max(_currValue, MaxHealth + TemporaryMaxHealth);
            CurrentHealth = _currValue;
        }
        public void Hurt(float value)
        {
            float _currValue = CurrentHealth;
            _currValue -= value;
            _currValue = Mathf.Min(_currValue, MinHealth);
            CurrentHealth = _currValue;
        } 
        #endregion
    }
}
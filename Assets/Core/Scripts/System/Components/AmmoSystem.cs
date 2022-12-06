using System;
using UnityEngine;

namespace Core.Systems.Components.Systems
{
    [Serializable]
    public class AmmoSystem
    {
        #region Constructors

        public AmmoSystem(float magazineSize, float backupAmmo)
        {
            MagazineSize = magazineSize;
            BackupAmmo = backupAmmo;
            Reload();
        }
        #endregion

        #region Field
        public float MagazineSize { get; private set; }
        public float BackupAmmo { get; private set; }
        public float CurrentAmmo { get; private set; }
        public float MinAmmo { get; private set; } = 0.0f;
        public float OneShotCount { get; private set; } = 1.0f;
        public float AmmoPercentage => CurrentAmmo / (MagazineSize);
        public bool CanShoot => CurrentAmmo > MinAmmo;
        #endregion

        #region Public Methods
        public void Shoot()
        {
            CurrentAmmo -= OneShotCount;
            CurrentAmmo = Mathf.Max(0.0f, CurrentAmmo);
        }
        public void Reload()
        {
            if (CurrentAmmo >= MagazineSize)
                return;
            int ammoToAdd = ((int)(MagazineSize - CurrentAmmo));
            CurrentAmmo += GetFromBackup(ammoToAdd);
        }

        private float GetFromBackup(int ammoToAdd)
        {
            float limitedToMagazine = Mathf.Min(MagazineSize, ammoToAdd);
            float limitedToAmmoCount = Mathf.Min(limitedToMagazine, BackupAmmo);
            BackupAmmo -= limitedToAmmoCount;
            return limitedToAmmoCount;
        }

        public void AddAmmo(int value)
        {
            BackupAmmo += value;
        }
        #endregion
    }
}
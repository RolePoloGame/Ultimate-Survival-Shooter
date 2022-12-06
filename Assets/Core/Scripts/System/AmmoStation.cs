using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Systems.Stations
{
    public class AmmoStation : MonoBehaviour
    {
        [SerializeField]
        private int Ammunition;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            PlayerShooting.AmmoSystem.AddAmmo(Ammunition);
            gameObject.SetActive(false);
        }
    }
}
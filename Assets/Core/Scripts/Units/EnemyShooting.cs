using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyShooting : MonoBehaviour
{
    public Transform Target { get; internal set; }
    public float MaxDistance = 0.0f;

    public abstract void Fire(float launchForce, float fireRate);
}

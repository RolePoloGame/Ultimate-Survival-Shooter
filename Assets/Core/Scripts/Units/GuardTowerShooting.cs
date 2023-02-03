using System;
using System.Collections;
using UnityEngine;

public class GuardTowerShooting : EnemyShooting
{
    [SerializeField]
    private Rigidbody m_Shell;
    [SerializeField]
    private Transform m_FireTransform;
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private float LaserWidth = 1;

    private Coroutine coroutine;

    public override void Fire(float launchForce, float fireRate)
    {
        coroutine ??= StartCoroutine(SpawnAfterTime(fireRate));
    }

    private void SetLinePositions()
    {
        line.SetPosition(0, m_FireTransform.transform.position);
        line.SetPosition(1, Target.position);
    }

    private IEnumerator SpawnAfterTime(float fireRate)
    {
        float timer = 0.0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > fireRate) break;
            SetLinePositions();
            SetLaserWidth(timer / fireRate);
            if (Vector3.Distance(m_FireTransform.position, Target.position) > MaxDistance)
            {
                SetLaserWidth(0.0f);
                StopCoroutine();
            }

            yield return new WaitForEndOfFrame();
        }

        SpawnExplosion();
        SetLaserWidth(0.0f);
        yield return new WaitForSeconds(1.0f);
        StopCoroutine();
    }

    private void StopCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private void SpawnExplosion()
    {
        Rigidbody shellInstance = Instantiate(m_Shell, Target.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = 0.0f * m_FireTransform.forward;
    }

    private void SetLaserWidth(float readinessFactor)
    {
        float width = LaserWidth * readinessFactor;
        line.startWidth = width;
        line.endWidth = width;
    }
}

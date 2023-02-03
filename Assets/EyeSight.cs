using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EyeSight : MonoBehaviour
{
    [SerializeField]
    private bool Debugging = false;
    public string[] tags = { "Player" };
    public Transform target;
    public float EyesightSize
    {
        get => _eyesightSize;
        set
        {
            _eyesightSize = value;
            Collider.radius = _eyesightSize;
        }
    }
    [SerializeField]
    private float _eyesightSize = 0.5f;
    private SphereCollider Collider
    {
        get
        {
            if (_collider == null)
                _collider = GetComponent<SphereCollider>();
            return _collider;
        }
        set => _collider = value;
    }
    private SphereCollider _collider;

    private void OnDrawGizmos()
    {
        if (!Debugging) return;

        Gizmos.DrawWireSphere(transform.position, EyesightSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (!other.CompareTag(tags[i]))
                continue;

            target = other.transform;
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target == null)
            return;
        if (!other.gameObject.Equals(target.gameObject))
            return;

        target = null;
    }
}

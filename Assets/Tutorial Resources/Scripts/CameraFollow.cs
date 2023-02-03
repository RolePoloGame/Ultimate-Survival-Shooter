using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static Transform Camera;

    [SerializeField]
    private Transform target;            // The position that that camera will be following.
    [SerializeField]
    private float smoothing = 5f;        // The speed with which the camera will be following.
    [SerializeField]
    private Vector3 offset = new Vector3(0, 5, 0);                     // The initial offset from the target.

    private void Awake()
    {
        Camera = transform;
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}

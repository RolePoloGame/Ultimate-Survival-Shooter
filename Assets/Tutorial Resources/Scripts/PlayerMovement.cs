using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float ROTATION_EPSILON = 0.5f;
    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float rotSpeed = 50f;
    private Vector3 movement;
    [SerializeField]
    private Camera MainCamera;
    #region Components

    private Animator anim;
    private Animator GetAnimator()
    {
        if (anim == null) anim = GetComponent<Animator>();
        return anim;
    }
    private Rigidbody rb;
    private Rigidbody GetRigidbody()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        return rb;
    }
    #endregion

    private int floorMask;
    private float camRayLength = 1000f;
    private int backupAmmo = 30;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Move(horizontal, vertical);
        Turning();
        Animating(horizontal, vertical);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Ammonition"))
            return;

        backupAmmo = 30;
        PlayerShooting.AmmoSystem.AddAmmo(backupAmmo);
        Destroy(other.gameObject);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = speed * Time.deltaTime * movement.normalized;
        GetRigidbody().MovePosition(transform.position + movement);
    }

    void Turning()
    {
        if (!Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, camRayLength, floorMask))
            return;

        Vector3 diff = hitInfo.point - transform.position;
        if (diff.magnitude < ROTATION_EPSILON)
            diff = diff.normalized;
        diff = -diff;
        Quaternion newRotation = (Quaternion.LookRotation(diff)) * new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);
        transform.rotation = newRotation;// Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotSpeed);
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        GetAnimator().SetBool("IsWalking", walking);
    }

}

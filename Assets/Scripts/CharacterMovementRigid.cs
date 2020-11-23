using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovementRigid : MonoBehaviour
{
    //To do:
    //Ground check
    //Slope check


    [Header("Essentials")]
    public Camera PlayerCamera;

    [Header("Variables")]
    private float _accelerationSpeed = 100f;
    private float _maxVelocity = 3.5f;
    private float _drag = 8f;
    private float _airDrag = 0.1f;
    private float _jumpStrength = 7.5f;
    private float _extraGravity = 1.5f;

    private Vector3 _velocity;
    private Rigidbody _rigidBody;
    private CapsuleCollider _collider;

    private void Start()
    {
        _rigidBody = this.GetComponent<Rigidbody>();
        _collider = this.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    private void FixedUpdate()
    {
        _velocity = _rigidBody.velocity;

        ApplyMovement();
        LimitVelocity();
        ApplyDrag();

        ApplyExtraGravity();

        _rigidBody.velocity = _velocity;
    }

    private void ApplyMovement()
    {
        Vector3 horizontal = PlayerCamera.transform.right * Input.GetAxis("Horizontal");
        Vector3 vertical = PlayerCamera.transform.forward * Input.GetAxis("Vertical");
        Vector3 totalMovement = horizontal + vertical;
        totalMovement.y = 0;

        _rigidBody.AddForce(totalMovement * _accelerationSpeed, ForceMode.Acceleration);
    }

    private void LimitVelocity()
    {
        float heightVelocity = _velocity.y;
        _velocity.y = 0;

        if (_velocity.sqrMagnitude >= _maxVelocity * _maxVelocity)
            _velocity = _velocity.normalized * _maxVelocity;

        _velocity.y = heightVelocity;
    }

    private void ApplyDrag()
    {
        _velocity.x /= 1 + _drag * Time.fixedDeltaTime;
        _velocity.y /= 1 + _airDrag * Time.fixedDeltaTime;
        _velocity.z /= 1 + _drag * Time.fixedDeltaTime;
    }

    private void ApplyExtraGravity()
    {
        _velocity += _extraGravity * Physics.gravity * Time.fixedDeltaTime;
    }

    private void Jump()
    {
        _rigidBody.AddForce(Vector3.up * _jumpStrength, ForceMode.VelocityChange);
    }
}

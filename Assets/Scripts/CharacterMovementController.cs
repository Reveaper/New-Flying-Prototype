using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Camera _playerCamera;

    private float _accelerationSpeed = 75f;
    private float _maxVelocity = 6.5f;
    private float _jumpStrength = 7.5f;
    private float _drag = 8f;
    private float _airDrag = 0.01f;
    private float _airControl = 0.25f;

    private CharacterController _controller;
    private CapsuleCollider _collider;
    private Vector3 _velocity;
    private bool _isGrounded;

    private Vector3 FeetPosition { get { return this.transform.position - new Vector3(0, _collider.height / 2f, 0); } }


    private void Start()
    {
        _controller = this.GetComponent<CharacterController>();
        _collider = this.GetComponent<CapsuleCollider>();
    }


    private void Update()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
            Jump();

        ApplyGravity();
        ApplyMovement();
        LimitVelocity();

        ApplyDrag();

        _controller.Move(_velocity * Time.deltaTime);
    }
    private void ApplyGravity()
    {



        
        _isGrounded = Physics.CheckSphere(FeetPosition, 0.02f, _groundLayer, QueryTriggerInteraction.Ignore);
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = 0f;
        else
            _velocity.y += (Physics.gravity.y * 2f) * Time.deltaTime;

        /*
        RaycastHit rayHit;

        _isGrounded = Physics.Raycast(FeetPosition + Vector3.up * 0.01f, Vector3.down, out rayHit, 0.02f);
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = 0;*/
    }

    private void ApplyMovement()
    {
        /*
        float airControl = _isGrounded ? 1 : _airControl;
        Vector3 horizontal = _playerCamera.transform.right * (Input.GetAxis("Horizontal") * airControl);
        Vector3 vertical = _playerCamera.transform.forward * (Input.GetAxis("Vertical") * airControl);*/

        if(_isGrounded)
        {
            Vector3 horizontal = _playerCamera.transform.right * Input.GetAxis("Horizontal");
            Vector3 vertical = _playerCamera.transform.forward * Input.GetAxis("Vertical");
            Vector3 totalMovement = horizontal + vertical;
            totalMovement.y = 0;

            _velocity += totalMovement * _accelerationSpeed * Time.deltaTime;
        }

    }

    private void ApplyDrag()
    {
        _velocity.x /= 1 + _drag * Time.deltaTime;
        _velocity.y /= 1 + _airDrag * Time.deltaTime;
        _velocity.z /= 1 + _drag * Time.deltaTime;
    }

    private void LimitVelocity()
    {
        float velocityHeight = _velocity.y;
        _velocity.y = 0;

        if (_velocity.sqrMagnitude > _maxVelocity * _maxVelocity)
            _velocity = _velocity.normalized * _maxVelocity;

        _velocity.y = velocityHeight;
    }

    private void Jump()
    {
        _velocity.y = _jumpStrength;
    }
}

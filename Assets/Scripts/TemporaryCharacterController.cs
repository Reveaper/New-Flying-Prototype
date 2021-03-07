using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public enum PlayerState
{
    Grounded, Flying
}*/

[RequireComponent(typeof(Rigidbody))]
public class TemporaryCharacterController : MonoBehaviour
{
    private Animator _animator;
    private Camera _camera;

    [SerializeField] private CameraSystem _cameraSystem;

    private PlayerState _state;
    private Rigidbody _rigidbody;

    private Vector3 _velocity;
    [SerializeField] private float _movementStrength = 50f;
    private float _rotationLerpSpeed = 0.25f;

    private bool _jump;

    private float _maxVelocity = 4f;

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
        _rigidbody = this.GetComponent<Rigidbody>();
        _camera = _cameraSystem.Camera;


        //Locomotion doesnt work with Root Motion enabled
        //Locomotion without root motion + enable/disable on roll works, but it gives odd AABB errors only if the game starts with root motion disabled
        //Disabling root motion after game has started still give those errors
        
        //On the first roll, root motion has to be already enabled or else it will continuously give those errors
        //so, easy dumb fix.. roll the player on game start
        //_animator.SetTrigger("Roll");

    }

    private void FixedUpdate()
    {
        HandleMovementStates();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
            _animator.SetTrigger("Roll");

        if (Input.GetKeyDown(KeyCode.Space))
            _animator.SetBool("Fly", !_animator.GetBool("Fly"));


        if (Input.GetKeyDown(KeyCode.R))
            _jump = true;



    }

    private void HandleMovementStates()
    {
        /*
        switch (_state)
        {
            case PlayerState.Grounded:
                HandleGroundedState();
                break;
            case PlayerState.Flying:
                HandleFlyingState();
                break;
        }*/
    }

    private void HandleGroundedState()
    {
        _velocity = _rigidbody.velocity;

        
        ApplyMovement();
        LimitVelocity();

        if (_jump)
        {
            _velocity = Vector3.up * 5f;
            _jump = false;
        }

        _rigidbody.velocity = _velocity;

        //RotateToDirection();

        _animator.SetFloat("Forward", Mathf.Min(_rigidbody.velocity.magnitude / _maxVelocity, 1));
    }

    private void ApplyMovement()
    {
        Vector3 movementVertical = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized * Input.GetAxis("Vertical");
        Vector3 movementHorizontal = Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized * Input.GetAxis("Horizontal");
        Vector3 movement = movementVertical + movementHorizontal;
        movement.y = 0;

        _velocity += movement * Time.deltaTime * _movementStrength;
    }

    private void LimitVelocity()
    {
        float velocityHeight = _velocity.y;
        _velocity.y = 0;

        if (_velocity.sqrMagnitude > _maxVelocity * _maxVelocity)
            _velocity = _velocity.normalized * _maxVelocity;

        _velocity.y = velocityHeight;
    }

    private void RotateToDirection()
    {
        if(_velocity.sqrMagnitude >= 0.02f && _animator.applyRootMotion == false)
        {
            Vector3 velocity = Vector3.ProjectOnPlane(_velocity, Vector3.up);
            SmoothRotateTowards(velocity);
        }


    }

    private void SmoothRotateTowards(Vector3 forward)
    {
        Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, _rotationLerpSpeed);
    }

    private void HandleFlyingState()
    {
        this.transform.position += Vector3.up * Time.deltaTime;
    }

    private void OnAnimatorMove()
    {
        if (_rigidbody.velocity.sqrMagnitude >= 0.02f)
        {
            Vector3 velocity = Vector3.ProjectOnPlane(_rigidbody.velocity, Vector3.up);
            SmoothRotateTowards(velocity);
        }
    }
}

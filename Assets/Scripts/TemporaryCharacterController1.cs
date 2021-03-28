using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Idle, Rolling, Flying
}

[RequireComponent(typeof(Rigidbody))]
public class TemporaryCharacterController1 : MonoBehaviour
{
    private Animator _animator;
    private Camera _camera;

    public Transform Mesh;

    [SerializeField] private CameraSystem _cameraSystem;
    private Rigidbody _rigidbody;

    private float _acceleration = 100f;
    private float _rotationLerpSpeed = 10f;

    private float _flyingSpeedMax = 10f;
    private float _flyingAcceleration = 0.45f;
    private float _flyingRotationSpeed = 2f;


    private bool _jump;

    private float _maxVelocity = 4f;

    public PlayerState State { set => _state = value; }
    private PlayerState _state = PlayerState.Idle;

    public Rigidbody RigidBody { get => _rigidbody; }

    [SerializeField] private List<Collider> _defaultColliders;
    [SerializeField] private List<Collider> _flyingColliders;

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
        _rigidbody = this.GetComponent<Rigidbody>();
        _camera = _cameraSystem.Camera;
    }

    private void Update()
    {
        HandleInput();

        Vector3 velocity = _rigidbody.velocity;

        switch (_state)
        {
            case PlayerState.Idle:
                velocity = HandleGroundMovement(_rigidbody.velocity);
                break;
            case PlayerState.Flying:
                velocity = HandleFlyingMovement(_rigidbody.velocity);
                break;
        }

        _rigidbody.velocity = velocity;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
            _animator.SetTrigger("Roll");

        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetBool("Fly", !_animator.GetBool("Fly"));

            foreach (var collider in _defaultColliders)
                collider.enabled = !_animator.GetBool("Fly");

            foreach (var collider in _flyingColliders)
                collider.enabled = _animator.GetBool("Fly");
        }

        if (Input.GetMouseButtonDown(0))
            _animator.SetTrigger("Kick1");

        if (Input.GetKeyDown(KeyCode.Q))
            _animator.SetTrigger("Attack");
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case PlayerState.Flying:
                _rigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
                break;
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = _rigidbody.velocity;

        if(_state == PlayerState.Rolling)
            velocity = HandleRootMovement();

        _rigidbody.velocity = velocity;
    }

    private Vector3 HandleGroundMovement(Vector3 velocity)
    {
        Vector3 movementVertical = Vector3.ProjectOnPlane(_cameraSystem.transform.forward, Vector3.up).normalized * Input.GetAxis("Vertical");
        Vector3 movementHorizontal = Vector3.ProjectOnPlane(_cameraSystem.transform.right, Vector3.up).normalized * Input.GetAxis("Horizontal");
        Vector3 movement = movementVertical + movementHorizontal;
        movement.y = 0;

        if (movement.sqrMagnitude > 0.01f)
        {
            velocity += movement * _acceleration * Time.deltaTime;
            HandleVelocityLimit(ref velocity, _maxVelocity, true);
            SmoothRotateTowards(velocity, true, _rotationLerpSpeed);
        }

        float animF = Mathf.Clamp((velocity.magnitude / _maxVelocity), 0, 1);
        _animator.SetFloat("Forward", animF);

        return velocity;
    }



    private Vector3 HandleFlyingMovement(Vector3 velocity)
    {
        velocity += Mesh.forward * Time.fixedDeltaTime * 75f * _flyingAcceleration;
        HandleVelocityLimit(ref velocity, _flyingSpeedMax, false);
        HandleFlyingRotation(velocity);

        return velocity;
    }

    private void HandleFlyingRotation(Vector3 velocity)
    {
        float dot = Vector3.Dot(Mesh.right, velocity);

        Quaternion targetRotation = Quaternion.LookRotation(_camera.transform.forward, Vector3.up);
        Quaternion bankOffset = Quaternion.Euler(new Vector3(0, 0, dot * 65f));
        Mesh.rotation = Quaternion.Slerp(Mesh.rotation, targetRotation * bankOffset, _flyingRotationSpeed * Time.deltaTime);
    }

    private Vector3 HandleRootMovement()
    {
        Vector3 forwardRoot = Mesh.forward * _animator.deltaPosition.z;
        Vector3 sideRoot = Mesh.right * _animator.deltaPosition.x;
        return (forwardRoot + sideRoot) / Time.deltaTime;
    }


    private void HandleVelocityLimit(ref Vector3 velocity, float maxVelocity, bool ignoreY)
    {
        if(ignoreY)
        {
            float velocityHeight = velocity.y;
            velocity.y = 0;

            velocity = LimitVelocity(velocity, maxVelocity);

            velocity.y = velocityHeight;
        }
        else
        {
            velocity = LimitVelocity(velocity, maxVelocity);
        }
    }

    private Vector3 LimitVelocity(Vector3 velocity, float maxVelocity)
    {
        return velocity.sqrMagnitude > maxVelocity * maxVelocity ? velocity.normalized * maxVelocity : velocity;
    }



    private void SmoothRotateTowards(Vector3 forward, bool ignoreY, float rotationSpeed)
    {
        if(ignoreY)
            forward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);
        Mesh.rotation = Quaternion.Slerp(Mesh.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

}

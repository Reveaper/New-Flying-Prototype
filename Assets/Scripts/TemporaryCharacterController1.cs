using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Idle, Rolling
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
    private float _rotationLerpSpeed = 12.5f;

    private bool _jump;

    private float _maxVelocity = 4f;

    public PlayerState State { set => _state = value; }
    private PlayerState _state = PlayerState.Idle;

    public UnityEngine.UI.Text TextDebug;

    //_rigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
    public Rigidbody RigidBody { get => _rigidbody; }

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
        _rigidbody = this.GetComponent<Rigidbody>();
        _camera = _cameraSystem.Camera;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
            _animator.SetTrigger("Roll");

        if (Input.GetKeyDown(KeyCode.R))
            _animator.SetBool("Fly", !_animator.GetBool("Fly"));

    }



    private void OnAnimatorMove()
    {
        Vector3 velocity = _rigidbody.velocity;

        if (_state == PlayerState.Idle)
        {
            Vector3 movementVertical = Vector3.ProjectOnPlane(_cameraSystem.transform.forward, Vector3.up).normalized * Input.GetAxis("Vertical");
            Vector3 movementHorizontal = Vector3.ProjectOnPlane(_cameraSystem.transform.right, Vector3.up).normalized * Input.GetAxis("Horizontal");
            Vector3 movement = movementVertical + movementHorizontal;
            movement.y = 0;

            if (movement.sqrMagnitude > 0.001f)
            {
                velocity += movement * _acceleration * Time.deltaTime;
                LimitVelocity(ref velocity);
                SmoothRotateTowards(velocity);
            }

        }
        else if (_state == PlayerState.Rolling)
        {
            Vector3 forwardRoot = Mesh.transform.forward * _animator.deltaPosition.z;
            Vector3 sideRoot = Mesh.transform.right * _animator.deltaPosition.x;
            velocity = (forwardRoot + sideRoot) / Time.deltaTime;
        }

        float animF = Mathf.Clamp((velocity.magnitude / _maxVelocity), 0, 1);
        _animator.SetFloat("Forward", animF);
        TextDebug.text = _state.ToString();

        _rigidbody.velocity = velocity;
    }

    private void LimitVelocity(ref Vector3 velocity)
    {
        float velocityHeight = velocity.y;
        velocity.y = 0;

        if (velocity.sqrMagnitude > _maxVelocity * _maxVelocity)
            velocity = velocity.normalized * _maxVelocity;

        velocity.y = velocityHeight;
    }

    private void SmoothRotateTowards(Vector3 forward)
    {
        forward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);
        Mesh.transform.rotation = Quaternion.Slerp(Mesh.transform.rotation, targetRotation, _rotationLerpSpeed * Time.deltaTime);
    }

}

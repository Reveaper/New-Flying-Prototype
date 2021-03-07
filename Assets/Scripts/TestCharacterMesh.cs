using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterMesh : MonoBehaviour
{
    private TemporaryCharacterController1 _controller;

    private void OnAnimatorMove()
    {
        /*
        Vector3 velocity = _controller.RigidBody.velocity;

        if (_controller.State == PlayerState.Idle)
        {
            Vector3 movementVertical = Vector3.ProjectOnPlane(_cameraSystem.transform.forward, Vector3.up).normalized * Input.GetAxis("Vertical");
            Vector3 movementHorizontal = Vector3.ProjectOnPlane(_cameraSystem.transform.right, Vector3.up).normalized * Input.GetAxis("Horizontal");
            Vector3 movement = movementVertical + movementHorizontal;
            movement.y = 0;

            if (movement.sqrMagnitude > 0.001f)
            {
                velocity += movement * _acceleration / Time.deltaTime * 50f;
                LimitVelocity(ref velocity);
                //SmoothRotateTowards(velocity);
            }

        }
        else if (_state == PlayerState.Rolling)
        {
            velocity = _animator.deltaPosition / Time.deltaTime;
        }

        float animF = Mathf.Clamp((velocity.magnitude / _maxVelocity), 0, 1);
        _animator.SetFloat("Forward", animF);
        TextDebug.text = _state.ToString();

        _rigidbody.velocity = velocity;
        //_rigidbody.angularVelocity = _animator.angularVelocity;

        */
    }
}

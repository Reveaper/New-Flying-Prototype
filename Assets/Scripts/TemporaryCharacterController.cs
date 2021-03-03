using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TemporaryCharacterController : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        float movement = Input.GetKey(KeyCode.W) ? Time.deltaTime : -Time.deltaTime;
        _animator.SetFloat("Forward", Mathf.Clamp(_animator.GetFloat("Forward") + movement, 0, 1));
    }
}

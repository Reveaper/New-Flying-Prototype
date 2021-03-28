using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationState : StateMachineBehaviour
{
    [SerializeField] private PlayerState _stateEnter;

    private TemporaryCharacterController1 _characterController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_characterController == null)
            _characterController = animator.GetComponentInParent<TemporaryCharacterController1>();

        _characterController.State = _stateEnter;
    }
}

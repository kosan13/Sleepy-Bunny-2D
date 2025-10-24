using Player;
using Player.Movement;
using UnityEngine;

namespace Animation
{
    public class FallingAnimation : StateMachineBehaviour
    {
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController playerController = animator.gameObject.GetComponentInParent<PlayerController>();
            Rigidbody2D rigidbody2D = playerController.GetComponent<Rigidbody2D>();

            if (Jump.IsGrounded(rigidbody2D)) AnimationsStateMachine.SetState(AnimationsStates.IsLanding);
            else AnimationsStateMachine.SetState(AnimationsStates.IsFalling);
        }
    }
}
using Player;
using UnityEngine;

namespace Animation
{
    public class WalkingAnimation : StateMachineBehaviour
    {
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController playerController = animator.gameObject.GetComponentInParent<PlayerController>();
            Rigidbody2D rigidbody2D = playerController.GetComponent<Rigidbody2D>();
            
            if (rigidbody2D.linearVelocityX == 0) AnimationsStateMachine.SetState(AnimationsStates.IsIdling);
            else if (rigidbody2D.linearVelocityY < 0) AnimationsStateMachine.SetState(AnimationsStates.IsFalling);
            else AnimationsStateMachine.SetState(AnimationsStates.IsWalking);
        }
    }
}
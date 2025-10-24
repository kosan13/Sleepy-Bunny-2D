using Player;
using UnityEngine;

namespace Animation
{
    public class LandingAnimation : StateMachineBehaviour
    {
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController playerController = animator.gameObject.GetComponentInParent<PlayerController>();
            Rigidbody2D rigidbody2D = playerController.GetComponent<Rigidbody2D>();
            
            if (rigidbody2D.linearVelocityX <= 0 || rigidbody2D.linearVelocityX >= 0) AnimationsStateMachine.SetState(AnimationsStates.IsWalking);
            else AnimationsStateMachine.SetState(AnimationsStates.IsIdling);
        }
    }
}
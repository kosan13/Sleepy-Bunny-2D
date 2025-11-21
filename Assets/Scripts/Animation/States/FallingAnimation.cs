using UnityEngine;
using static Animation.AnimationsStateMachine;
using static Player.PlayerController;

namespace Animation
{
    public class FallingAnimation : StateMachineBehaviour
    {
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (TrySetStateIsFalling(GetPlayerController.GetRigidbody2D, GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateIsLanding(GetPlayerController.GetRigidbody2D, GetPlayerController.MainAnimationBone)) return;
            return;
        }
    }
}
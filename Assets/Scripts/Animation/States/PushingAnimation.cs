using UnityEngine;
using static Animation.AnimationsStateMachine;
using static Player.PlayerController;

namespace Animation.States
{
    public class PushingAnimation : StateMachineBehaviour
    {
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (TrySetStateIsPulling(GetPlayerController.GetRigidbody2D, GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateIsFalling(GetPlayerController.GetRigidbody2D, GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateIsIdling(GetPlayerController.GetRigidbody2D, GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateCrouchRun(GetPlayerController.GetRigidbody2D, GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateCrouchWalk(GetPlayerController.GetRigidbody2D, GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateIsRunning(GetPlayerController.GetRigidbody2D, GetPlayerController.MainAnimationBone)) return;
            if(TrySetStateIsWalking(GetPlayerController.GetRigidbody2D, GetPlayerController.MainAnimationBone)) return;
            return;
        }
    }
}
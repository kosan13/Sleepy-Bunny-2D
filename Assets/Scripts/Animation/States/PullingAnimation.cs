using UnityEngine;
using static Animation.AnimationsStateMachine;
using static Player.PlayerController;

namespace Animation.States
{
    public class PullingAnimation : StateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (TrySetStateIsFalling(GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateIsIdling(GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateIsCrouchingIdling(GetPlayerController.MainAnimationBone)) return;
            
            // if (TrySetStateIsPushing(GetPlayerController.MainAnimationBone)) return;
            // if (TrySetStateCrouchRun(GetPlayerController.MainAnimationBone)) return;
            // if (TrySetStateCrouchWalk(GetPlayerController.MainAnimationBone)) return;
            // if (TrySetStateIsRunning(GetPlayerController.MainAnimationBone)) return;
            // if(TrySetStateIsWalking(GetPlayerController.MainAnimationBone)) return;
            return;
        }
    }
}
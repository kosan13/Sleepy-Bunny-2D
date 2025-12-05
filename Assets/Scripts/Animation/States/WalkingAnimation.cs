using UnityEngine;
using static Animation.AnimationsStateMachine;
using static Player.PlayerController;

namespace Animation.States
{
    public class WalkingAnimation : StateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (TrySetStateIsFalling(GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateIsIdling(GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateIsCrouchingIdling(GetPlayerController.MainAnimationBone)) return;
            return;
        }
    }
}
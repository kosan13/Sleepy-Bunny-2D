using UnityEngine;
using static Animation.AnimationsStateMachine;
using static Player.PlayerController;

namespace Animation.States
{
    public class JumpingAnimation : StateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (TrySetStateIsFalling(GetPlayerController.MainAnimationBone)) return;
            if (TrySetStateIsLanding(GetPlayerController.MainAnimationBone)) return;
            return;
        }
    }
}

using UnityEngine;
using static Animation.AnimationsStateMachine;
using static Player.PlayerController;

namespace Animation.States
{
    public class StandAnimation : StateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (TrySetStateIsIdling(GetPlayerController.MainAnimationBone)) return;
            return;
        }
    }
}
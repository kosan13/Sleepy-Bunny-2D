using UnityEngine;
using static Animation.AnimationsStateMachine;

namespace Animation.States
{
    public class PullingAnimation : StateMachineBehaviour
    {
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (TrySetStateIsPushing()) return;
            if (TrySetStateIsFalling()) return;
            if (TrySetStateIsIdling()) return;
            if (TrySetStateCrouchRun()) return;
            if (TrySetStateCrouchWalk()) return;
            if (TrySetStateIsRunning()) return;
            if(TrySetStateIsWalking()) return;
            return;
        }
    }
}
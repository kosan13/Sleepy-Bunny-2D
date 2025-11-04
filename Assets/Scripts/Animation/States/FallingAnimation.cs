using UnityEngine;
using static Animation.AnimationsStateMachine;

namespace Animation
{
    public class FallingAnimation : StateMachineBehaviour
    {
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (TrySetStateIsFalling()) return;
            if (TrySetStateIsLanding()) return;
            return;
        }
    }
}
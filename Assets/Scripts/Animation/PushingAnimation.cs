using UnityEngine;

namespace Animation
{
    public class PushingAnimation : StateMachineBehaviour
    {
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AnimationsStateMachine.SetState(AnimationsStates.IsIdling);
        }
    }
}
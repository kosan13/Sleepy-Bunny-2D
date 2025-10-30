using Animation;
using UnityEngine;

namespace Player.Movement
{
    public static class PushAndPull
    {
        private const int PushAndPullObjectLayer = 6;
        
        private static Rigidbody2D _rigidbody2D;
        private static bool _pushOrPull;
        private static GameObject _pushAndPullObject;

        public static void OnPushAndPullAwake(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
        }

        public static void OnPushAndPullUpdate()
        {
            if (!_pushOrPull) return;
            switch (Move.MoveDirection)
            {
                case > 0: OnPush(); break;
                case < 0: OnPull(); break;
                default: AnimationsStateMachine.SetState(AnimationsStates.IsIdling); break;
            }
        }
        public static void OnPushAndPullFixedUpdate() {}

        public static void OnPushAndPullCollisionEnter2D(Collision2D other) { }
        public static void OnPushAndPullCollisionExit2D(Collision2D other) { }
        
        public static void OnPushAndPullTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PushAndPullObjectLayer) return;
            _pushAndPullObject = other.gameObject;
        }
        public static void OnPushAndPullOnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer != _pushAndPullObject.layer) return;
            _pushAndPullObject = null;
            _pushOrPull = false;
        }

        public static void OnPushAndPull()
        {
            if (_pushAndPullObject is null) return;
            _pushOrPull = true;
        }

        private static void OnPush()
        {
            Debug.Log("PushObject direction is Value: " + Move.MoveDirection);
            AnimationsStateMachine.SetState(AnimationsStates.IsPushing);
        }

        private static void OnPull()
        {
            Debug.Log("PullObject direction is Value: " + Move.MoveDirection);
            AnimationsStateMachine.SetState(AnimationsStates.IsPulling);
        }
    }
}
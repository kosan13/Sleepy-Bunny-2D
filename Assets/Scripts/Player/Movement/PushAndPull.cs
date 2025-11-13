using Animation;
using UnityEngine;
using static Global.GlobalVariablesLibrary;

namespace Player.Movement
{
    public static class PushAndPull
    {
        
        private static Rigidbody2D _rigidbody2D;
        private static GameObject _pushAndPullObject;

        public static bool PushOrPull { get; private set; }

        public static void OnPushAndPullAwake(Rigidbody2D rigidbody2D) => _rigidbody2D = rigidbody2D;
        public static void OnPushAndPullUpdate() {}
        public static void OnPushAndPullFixedUpdate()
        {
            if (!PushOrPull) return;
            switch (Move.GetMoveDirection)
            {
                case > 0: OnPushOrPull(AnimationsStates.IsPushingRight); break;
                case < 0: OnPushOrPull(AnimationsStates.IsPullingLeft); break;
                default: OnPushOrPull(AnimationsStates.IsPullingRight); break;
            }
        }
        public static void OnPushAndPullCollisionEnter2D(Collision2D other) {}
        public static void OnPushAndPullCollisionExit2D(Collision2D other) {}
        public static void OnPushAndPullTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PushAndPullLayer) return;
            _pushAndPullObject = other.gameObject;
        }
        public static void OnPushAndPullOnTriggerExit2D(Collider2D other)
        {
            if (other is null || _pushAndPullObject is null) return;
            if (other.gameObject.layer != _pushAndPullObject.layer) return;
            _pushAndPullObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
            _pushAndPullObject = null;
            PushOrPull = false;
        }

        public static void OnPushAndPull(bool relistButton = false) => PushOrPull = _pushAndPullObject is not null && relistButton == false;
        private static void OnPushOrPull(AnimationsStates animationsStates)
        {
            Rigidbody2D rigidbody2D = _pushAndPullObject.GetComponent<Rigidbody2D>();
            rigidbody2D.linearVelocityX = _rigidbody2D.linearVelocityX;
            AnimationsStateMachine.SetState(animationsStates);
        }
    }
}
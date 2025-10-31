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

        public static void OnPushAndPullAwake(Rigidbody2D rigidbody2D) => _rigidbody2D = rigidbody2D;
        public static void OnPushAndPullUpdate() {}
        public static void OnPushAndPullFixedUpdate()
        {
            if (!_pushOrPull) return;
            switch (Move.MoveDirection)
            {
                case > 0: PushOrPull(AnimationsStates.IsPushing); break;
                case < 0: PushOrPull(AnimationsStates.IsPulling); break;
            }
        }
        public static void OnPushAndPullCollisionEnter2D(Collision2D other) {}
        public static void OnPushAndPullCollisionExit2D(Collision2D other) {}
        public static void OnPushAndPullTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PushAndPullObjectLayer) return;
            _pushAndPullObject = other.gameObject;
        }
        public static void OnPushAndPullOnTriggerExit2D(Collider2D other)
        {
            if (other is null || _pushAndPullObject is null) return;
            if (other.gameObject.layer != _pushAndPullObject.layer) return;
            _pushAndPullObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
            _pushAndPullObject = null;
            _pushOrPull = false;
        }

        public static void OnPushAndPull(bool relistButton = false) => _pushOrPull = _pushAndPullObject is not null && relistButton == false;

        private static void OnPush()
        {
            AnimationsStateMachine.SetState(AnimationsStates.IsPushing);
            Rigidbody2D rigidbody2D = _pushAndPullObject.GetComponent<Rigidbody2D>();
            rigidbody2D.linearVelocityX = _rigidbody2D.linearVelocityX;
        }
        private static void OnPull()
        {
            AnimationsStateMachine.SetState(AnimationsStates.IsPulling);
            Rigidbody2D rigidbody2D = _pushAndPullObject.GetComponent<Rigidbody2D>();
            rigidbody2D.linearVelocityX = _rigidbody2D.linearVelocityX;
        }

        private static void PushOrPull(AnimationsStates animationsStates)
        {
            Rigidbody2D rigidbody2D = _pushAndPullObject.GetComponent<Rigidbody2D>();
            rigidbody2D.linearVelocityX = _rigidbody2D.linearVelocityX;
            AnimationsStateMachine.SetState(animationsStates);
        }
    }
}
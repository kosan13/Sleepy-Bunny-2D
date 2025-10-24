using System;
using Animation;
using PushAndPull;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Player.Movement
{
    public static class PushAndPull
    {
        private const int PushAndPullObjectLayer = 6;
        
        private static Rigidbody2D _rigidbody2D;

        private static GameObject _pushAndPullObject;
        
        public static void OnPushAndPullAwake(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
        }
        public static void OnPushAndPullUpdate() {}
        public static void OnPushAndPullFixedUpdate() {}

        public static void OnPushAndPullCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != PushAndPullObjectLayer) return;
            _pushAndPullObject = other.gameObject;
        }
        public static void OnPushAndPullCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.layer != PushAndPullObjectLayer) return;
            _pushAndPullObject = null;
        }

        public static void OnPushAndPull()
        {
            AnimationsStateMachine.SetState(AnimationsStates.IsPulling);
            // AnimationsStateMachine.SetState(AnimationsStates.IsPushing);
        }
        
        private static void OnPush() {}
        private static void OnPull() {}
    }
}
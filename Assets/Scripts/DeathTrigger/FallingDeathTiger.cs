using System.Runtime.CompilerServices;
using Events;
using UnityEngine;

namespace DeathTrigger
{
    public static class FallingDeathTrigger
    {
        private static Rigidbody2D _rigidbody2D;
        private static float _safeFallVelocity;
        private static IDeathEvent _deathEvent;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnFallingDeathTriggerAwake(Rigidbody2D rigidbody2D, float safeFallVelocity)
        {
            _rigidbody2D = rigidbody2D;
            _safeFallVelocity = safeFallVelocity;

            if (_rigidbody2D.TryGetComponent(out IDeathEvent deathEvent)) _deathEvent = deathEvent;
            else Debug.LogError(_rigidbody2D.name + " dos not hav a 'IDeathEvent' on it");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnFallingDeathTriggerUpdate() {}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnFallingDeathTriggerFixedUpdate()
        {
            if (_rigidbody2D.linearVelocityY < _safeFallVelocity) return;
            _deathEvent.TriggerDeathEvent();
        }
    }
}
using UnityEngine;
using static Global.GlobalVariablesLibrary;

namespace PickUps
{
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class PickUp: MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != PlayerLayer) return;
            OnPickUp(other);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.layer != PlayerLayer) return;
            OnAfterPickUp(other);
            Destroy(this);
        }

        public abstract void OnPickUp(Collision2D collisionObject);
        public abstract void OnAfterPickUp(Collision2D collisionObject);
    }
}
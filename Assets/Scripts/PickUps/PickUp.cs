using UnityEngine;
using static Global.GlobalVariablesLibrary;

namespace PickUps
{
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class PickUp: MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PlayerLayer) return;
            OnPickUp(other);
            GetComponent<SpriteRenderer>().enabled = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer != PlayerLayer) return;
            OnAfterPickUp(other);
            Destroy(this);
        }

        public abstract void OnPickUp(Collider2D collisionObject);
        public abstract void OnAfterPickUp(Collider2D collisionObject);
    }
}
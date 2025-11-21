using Events;
using UnityEngine;

using static Global.GlobalVariablesLibrary;

namespace DeathTrigger
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EnterDeathTrigger : MonoBehaviour
    {
        private void OnValidate() => GetComponent<BoxCollider2D>().isTrigger = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PlayerLayer) return;
            if (other.TryGetComponent(out IDeathEvent deathEvent)) deathEvent.TriggerDeathEvent();
            else Debug.LogError(other.name + " dos not hav a 'IDeathEvent' on it");
        }
    }
}
using UnityEngine;
using static Global.GlobalVariablesLibrary;

namespace LevelFunctionsLibrary
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class NewLevelTrigger : MonoBehaviour
    {
        private BoxCollider2D _boxCollider2D;
        private void OnValidate()
        {
            //Only sets the value if not null
            _boxCollider2D ??= GetComponent<BoxCollider2D>();
            if (!_boxCollider2D.isTrigger) _boxCollider2D.isTrigger = true;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PlayerLayer) return;
            LevelFunctions.LoadNextLevel();
        }
    }
}
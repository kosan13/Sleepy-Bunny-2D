using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class CrouchAreaTrigger : MonoBehaviour
{
#if UNITY_ANDROID
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovementController controller = collision.GetComponent<MovementController>();
        if (controller)
        {
            controller.ToggleCrouch(true);
            controller.LockCrouch = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        MovementController controller = collision.GetComponent<MovementController>();
        if (controller)
        {
            controller.LockCrouch = false;
            controller.ToggleCrouch(false);
        }
    }
#endif
}

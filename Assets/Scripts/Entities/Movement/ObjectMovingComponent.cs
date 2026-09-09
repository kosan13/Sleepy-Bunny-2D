using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(MovementController))]
public class ObjectMovingComponent : MonoBehaviour
{
    // Todo: Rework and create interact class
    // Todo: clean up the code, there are multiple things that need to be optimized
    public bool CanGrab {  get; private set; }

    Rigidbody2D interactableRB;
    Rigidbody2D moveCompRB;
    MovementController movementController;
    float grabDirection;
    bool grabbing;
    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        moveCompRB = movementController.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(interactableRB && grabbing)
        {
            interactableRB.linearVelocity = moveCompRB.linearVelocity;
            if (movementController.CurrentState != MovementState.Walk && movementController.CurrentState != MovementState.Idle && movementController.CurrentState != MovementState.Run)
            {
                ReleaseObject();
            }
            if(movementController.MoveDirection == grabDirection)
            {
                movementController.AnimationController.UpdateAnimationState("IsPushing", true);
                movementController.AnimationController.UpdateAnimationState("IsPulling", false);
            }
            else
            {
                movementController.AnimationController.UpdateAnimationState("IsPushing", false);
                movementController.AnimationController.UpdateAnimationState("IsPulling", true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
       
       if(collision.gameObject.layer == 6 && collision.attachedRigidbody)
       {
            interactableRB = collision.attachedRigidbody;
            if (Global.GlobalFunctionsLibrary.IsGrounded(interactableRB)) interactableRB.constraints = RigidbodyConstraints2D.FreezeAll;
            CanGrab = true;
            grabDirection = movementController.MoveDirection;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger || (collision.gameObject.layer != 6)) return;
       
        movementController.AnimationController.UpdateAnimationState("IsPushing", false);
        movementController.AnimationController.UpdateAnimationState("IsPulling", false);
        movementController.CurrentWalkSpeed = movementController.WalkSpeed;
        interactableRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        interactableRB.transform.parent = null;
        interactableRB = null;
        movementController.LockDirection = false;
        CanGrab = false;
        movementController.SetWalkDirection(movementController.MoveDirection);
    }
   
    public void GrabObject()
    {
        if (interactableRB == null) return;
        movementController.CurrentWalkSpeed = movementController.WalkSpeed * Mathf.Clamp(1 - (0.2f * Mathf.RoundToInt(interactableRB.mass * 0.01f)), 0, 1);
        interactableRB.transform.parent = transform;
        interactableRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        BoxController box = interactableRB.GetComponent<BoxController>();
        if (box)
        {
            box.BottomBox = true;
            box.OnGrab();
        }
       
        grabbing = true;
        movementController.AnimationController.UpdateAnimationState("IsPushing", true);
        movementController.LockDirection = true;
    }
    public void ReleaseObject()
    {
        if (interactableRB == null) return;
        movementController.AnimationController.UpdateAnimationState("IsPulling", false);
        movementController.AnimationController.UpdateAnimationState("IsPushing", false);

        BoxController box = interactableRB.GetComponent<BoxController>();
        if (box) box.OnRelease();

        if (Global.GlobalFunctionsLibrary.IsGrounded(interactableRB)) interactableRB.constraints = RigidbodyConstraints2D.FreezeAll;
        interactableRB.transform.parent = null;
        movementController.CurrentWalkSpeed = movementController.WalkSpeed;
        movementController.LockDirection = false;
        grabbing = false;
        movementController.SetWalkDirection(movementController.MoveDirection);

    }
}

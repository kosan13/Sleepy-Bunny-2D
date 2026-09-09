using Animation;
using Global;
using Player.Movement;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Global.GlobalEnumLibrary;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementAnimationController))]
public class MovementController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float runModifier = 1.5f;

    [Header("Crouch")]
    [SerializeField] private float crouchWalkModifier = .5f;

    [Header("Jump")]
    [SerializeField] private float jumpPower = 15;
    [SerializeField] private float runJumpPower = 20;
    [SerializeField] private float horizontalRunJumpPower = 20;
    [SerializeField] private float horizontalJumpForceCap = 40;

    public bool LockCrouch { get; set; }
    public float WalkSpeed { get { return walkSpeed; } }
    public bool LockDirection { get; set; }
    public bool Climbing { get; set; }
    public MovementState CurrentState { get { return moveState; } }
    public float CurrentWalkSpeed { get { return currentWalkSpeed; } set { if(value > 0) currentWalkSpeed = value; } }
    public MovementAnimationController AnimationController { get { return animationController; } }
    public float MoveDirection {  get { return moveDirection; } }
   
    
    private Rigidbody2D rigidBody;
    private float currentWalkSpeed;
    private MovementState moveState = MovementState.Idle;
    private MovementAnimationController animationController;
    private float moveDirection = 0f;
    private bool isCrouching = false;
    private bool checkForUncrouch;
    private bool isRunning = false;
    private Vector2 originalHitboxSize;
    private Vector2 originalHitboxOffset;
    bool inWater = false;

    CapsuleCollider2D hitBoxCollider;
    private void Awake()
    {
        CurrentWalkSpeed = WalkSpeed;
        rigidBody = GetComponent<Rigidbody2D>();
        animationController = GetComponent<MovementAnimationController>();
        hitBoxCollider = GetComponent<CapsuleCollider2D>();
        originalHitboxSize = hitBoxCollider.size;
        originalHitboxOffset = hitBoxCollider.offset;
    }

    private void FixedUpdate()
    {
        if (checkForUncrouch && !GlobalFunctionsLibrary.IsGrounded(rigidBody, 1, -1)) ToggleCrouch(false);
        if (GlobalVariablesLibrary.PlayerGhostMode && rigidBody.simulated)
        {
            rigidBody.simulated = false;
        }
        else if(!GlobalVariablesLibrary.PlayerGhostMode && !rigidBody.simulated)
        {
            rigidBody.simulated = true;
        }
        float speed = currentWalkSpeed;
        switch (moveState)
        {
            case MovementState.Idle: return;
            case MovementState.Run: speed *= runModifier; break;
            case MovementState.CrouchWalk: speed *= crouchWalkModifier; break;
            default:
                break;
        }
        if(Mathf.Abs(rigidBody.linearVelocityX) < 10)
        {
            transform.Translate(Vector3.right * (moveDirection * speed * Time.fixedDeltaTime));
        }
        
    }
    public void Jump()
    {
        if (GlobalFunctionsLibrary.IsGrounded(rigidBody, 1, -1) && !inWater) return;
        if (Climbing || GlobalFunctionsLibrary.IsGrounded(rigidBody) || inWater)
        {
            if (isRunning)
            {
                rigidBody.linearVelocityY = runJumpPower;
                rigidBody.linearVelocityX += horizontalRunJumpPower * moveDirection;
                rigidBody.linearVelocityX = Mathf.Clamp(rigidBody.linearVelocityX, -horizontalJumpForceCap, horizontalJumpForceCap);
            }
            else rigidBody.linearVelocityY = jumpPower;
            Climbing = false;
            animationController.UpdateAnimationState("Jump");
        }
    }
   public void SetWalkDirection(float newMoveDirection)
    {
#if UNITY_ANDROID
        if (!PlayerInputManager.UsingKeyboardAndMouse && (newMoveDirection > 0.95f || newMoveDirection < -0.95f) && !isRunning) ToggleRunning(true); 
        else if (!PlayerInputManager.UsingKeyboardAndMouse && ((newMoveDirection <= 0.95f && newMoveDirection > 0) || (newMoveDirection >= -0.95f && newMoveDirection < 0)) && isRunning) ToggleRunning(false); 
#endif
        moveDirection = newMoveDirection;
        if (!LockDirection) animationController.UpdateDirectionalFacing(moveDirection);
        animationController.UpdateAnimationState("IsWalking", moveDirection == 0 ? false : true);
        animationController.PauseAnimations(false);
        UpdateMovementState();
    }
    public void ToggleRunning(bool isNowRunning)
    {  
        if (isNowRunning && isCrouching && !ToggleCrouch(false)) return;
        isRunning = isNowRunning;
        UpdateMovementState();
    }
    public bool ToggleCrouch(bool isNowCrouching)
    {
        if(LockCrouch) return false;
        if ((!isNowCrouching && GlobalFunctionsLibrary.IsGrounded(rigidBody, 1, -1))) { checkForUncrouch = true; return false; }
        isCrouching = isNowCrouching;
        checkForUncrouch = false;
        if (isCrouching)
        {
            ToggleRunning(false);
                hitBoxCollider.size = (originalHitboxSize * 0.75f);
                hitBoxCollider.offset = (originalHitboxOffset + Vector2.down * 0.5f);
        }
        else
        {
                hitBoxCollider.size =  originalHitboxSize;
                hitBoxCollider.offset = originalHitboxOffset;
        }
        animationController.UpdateAnimationState("IsCrouching", isCrouching);
        UpdateMovementState();
        return true;
    }
    public void UpdateMovementState()
    {
        moveState = MovementState.Walk;
        if (isRunning) moveState = MovementState.Run;
        if (isCrouching) moveState = MovementState.CrouchWalk;
        if (moveDirection == 0) moveState = MovementState.Idle;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BuoyancyEffector2D buoyancy = collision.GetComponent<BuoyancyEffector2D>();
        if (buoyancy)
        {
            inWater = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BuoyancyEffector2D buoyancy = collision.GetComponent<BuoyancyEffector2D>();
        if (buoyancy)
        {
            inWater = true;
        }
    }

}

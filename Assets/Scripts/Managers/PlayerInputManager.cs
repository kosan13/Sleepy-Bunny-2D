using Events;
using Input;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;
using static Player.Movement.ForceAccumulate;
using static UnityEngine.InputSystem.InputAction;
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(ObjectMovingComponent))]
[RequireComponent(typeof(ClimbingController))]
[RequireComponent(typeof(PlayerHealthComponent))]
[RequireComponent(typeof(PlayerCameraController))]
public class PlayerInputManager : MonoBehaviour
{
    public static UnityEvent<InputDevice> InputDeviceChanged = new UnityEvent<InputDevice>();
    public PlayerInputController.PlayerActions playerMap => inputControls.Player;
    public PlayerInputController.CameraActions cameraMap => inputControls.Camera;
    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject interactKeyPrompt;
    
    MovementController playerMovementController;
    ClimbingController climbingController;
    ObjectMovingComponent objectMovingComponent;
    PlayerInputController inputControls;
    PlayerCameraController cameraController;

    public static bool UsingKeyboardAndMouse;

    private void Awake()
    {
        playerMovementController = GetComponent<MovementController>();
        objectMovingComponent = GetComponent<ObjectMovingComponent>();
        climbingController = GetComponent<ClimbingController>();
        cameraController = GetComponent<PlayerCameraController>();
        inputControls = new PlayerInputController();
        
        playerMap.Enable();
        cameraMap.Enable();

        InputSystem.onActionChange += InputActionChangeCallback;
        SetupInputBindings();
        Debug.Log("penis");
        LevelFunctionsLibrary.LevelFunctions.togglePause.AddListener(PauseInput);
    }
    private void InputActionChangeCallback(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            InputAction receivedInputAction = (InputAction)obj;

            InputDevice lastDevice = receivedInputAction.activeControl.device;
            if (lastDevice.name.Equals("Mouse") || receivedInputAction.name == "Navigate" || receivedInputAction.name == "Look") return;
            UsingKeyboardAndMouse = lastDevice.name.Equals("Keyboard");
            InputDeviceChanged.Invoke(lastDevice);
        }
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= InputActionChangeCallback;
        RemoveInputBindings();
       
        playerMap.Disable();
        cameraMap.Disable();
    }
    private void RemoveInputBindings()
    {
        #region Player Character movement
        playerMap.Walk.performed -= SetWalkDirection;
        playerMap.Walk.canceled -= StopWalk;

        playerMap.Run.performed -= ToggleRunning;
        playerMap.Run.canceled -= StopRunning;

        playerMap.Jump.performed -= DoJump;
        playerMap.Jump.performed -= StopClimb;
        playerMap.Jump.performed -= InteractReleased;

        playerMap.Crouch.performed -= ToggleCrouching;
        playerMap.Crouch.canceled -= StopCrouching;

        playerMap.Interact.performed -= InteractPressed;
        playerMap.Interact.canceled -= InteractReleased;

        playerMap.Climb.performed -= Climb;
        playerMap.Climb.canceled -= StopMidClimb;
        #endregion
        #region Camera controls
        cameraMap.Horizontal.performed -= SetCameraHorizontalDirection;
        cameraMap.Horizontal.canceled -= ResetCameraHorizontalDirection;

        cameraMap.Vertical.performed -= SetCameraVerticalDirection;
        cameraMap.Vertical.canceled -= ResetCameraVerticalDirection;

        playerMap.Walk.performed -= ResetCamera;
        playerMap.Jump.performed -= ResetCamera;
        playerMap.Crouch.performed -= ResetCamera;
        playerMap.Climb.performed -= ResetCamera;
        #endregion
        playerMap.Pause.performed -= TogglePauseMenu;
    }
    private void SetupInputBindings()
    {
        #region Player character movement
        playerMap.Walk.performed += SetWalkDirection;
        playerMap.Walk.canceled += StopWalk;

        playerMap.Run.performed += ToggleRunning;
        playerMap.Run.canceled += StopRunning;

        playerMap.Jump.performed += DoJump;
        playerMap.Jump.performed += StopClimb;
        playerMap.Jump.performed += InteractReleased;

        playerMap.Crouch.performed += ToggleCrouching;
        playerMap.Crouch.canceled += StopCrouching;

        playerMap.Interact.performed += InteractPressed;
        playerMap.Interact.canceled += InteractReleased;


        playerMap.Climb.performed += Climb;
        playerMap.Climb.canceled += StopMidClimb;
        #endregion
        #region Camera controls
        cameraMap.Horizontal.performed += SetCameraHorizontalDirection;
        cameraMap.Horizontal.canceled += ResetCameraHorizontalDirection;
        
        cameraMap.Vertical.performed += SetCameraVerticalDirection;
        cameraMap.Vertical.canceled += ResetCameraVerticalDirection;

        playerMap.Walk.performed += ResetCamera;
        playerMap.Jump.performed += ResetCamera;
        playerMap.Crouch.performed += ResetCamera;
        playerMap.Climb.performed += ResetCamera;
        #endregion
        playerMap.Pause.performed += TogglePauseMenu;
    }
    #region Player movement bindings
    private void SetWalkDirection(CallbackContext callbackContext) => playerMovementController.SetWalkDirection(callbackContext.ReadValue<float>());
    private void StopWalk(CallbackContext callbackContext) => playerMovementController.SetWalkDirection(0);
    private void ToggleRunning(CallbackContext callbackContext) => playerMovementController.ToggleRunning(true);
    private void StopRunning(CallbackContext callbackContext) => playerMovementController.ToggleRunning(false);
    private void DoJump(CallbackContext callbackContext) => playerMovementController.Jump();
    private void ToggleCrouching(CallbackContext callbackContext) => playerMovementController.ToggleCrouch(true);
    private void StopCrouching(CallbackContext callbackContext) => playerMovementController.ToggleCrouch(false);
    private void Climb(CallbackContext callbackContext) => climbingController.ClimbInteraction(callbackContext.ReadValue<float>());
    private void StopMidClimb(CallbackContext callbackContext) => climbingController.ClimbInteraction(0);
    private void StopClimb(CallbackContext callbackContext) => climbingController.StopClimbing();
    #endregion
    #region Camera bindings
    private void ResetCamera(CallbackContext callbackContext) => cameraController.ResetFocusPoint();
    private void SetCameraHorizontalDirection(CallbackContext callbackContext) => cameraController.SetCameraHorizontalDir(callbackContext.ReadValue<float>());
    private void ResetCameraHorizontalDirection(CallbackContext callbackContext) => cameraController.SetCameraHorizontalDir(0);
    private void SetCameraVerticalDirection(CallbackContext callbackContext) => cameraController.SetCameraVerticalDir(callbackContext.ReadValue<float>());
    private void ResetCameraVerticalDirection(CallbackContext callbackContext) => cameraController.SetCameraVerticalDir(0);

    #endregion

    private void InteractPressed(CallbackContext callbackContext)
    {
        if(objectMovingComponent.CanGrab) objectMovingComponent.GrabObject();
    }
    private void InteractReleased(CallbackContext callbackContext)
    {
        objectMovingComponent.ReleaseObject();
    }
    void PauseInput(bool paused)
    {
       
        
        if (paused)
        {
            RemoveInputBindings();
            playerMap.Pause.performed += TogglePauseMenu;
        }
        else
        {
            playerMap.Pause.performed -= TogglePauseMenu;
            SetupInputBindings();
        }
    }
    public void TogglePauseMenu(CallbackContext callbackContext)
    {
        if (pauseMenuCanvas == null) return;
        if(pauseMenuCanvas.activeSelf)
        {
            pauseMenuCanvas.SetActive(false);
            LevelFunctionsLibrary.LevelFunctions.ToggleGamePause(false);
        }
        else
        {
            pauseMenuCanvas.SetActive(true);
            
            LevelFunctionsLibrary.LevelFunctions.ToggleGamePause(true);
        }
    }

    public void DisableInput()
    {
        playerMap.Disable();
    }
    public void EnableInput()
    {
        playerMap.Enable();
    }
   
}

using UnityEngine;
using UnityEngine.Events;
using DeviceType = GlobalType.DeviceType;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

namespace UnityCore
{
    namespace PlayerControl
    {
	    [RequireComponent(typeof(PlayerInput))]
        public class ControllerInputs : Singleton<ControllerInputs>
        {
	        #region Variables

	        // Public
	        public DeviceType CurrentDevice = DeviceType.PC;
	        
	        [Header("Character Input Values")]
	        public Vector2 InputPosition;
	        public Vector2 MoveCoordinate;
	        public Vector2 LookCoordinate;

	        public bool ScreenPressed;
	        public bool SpaceBarPressed;
	        public bool Sprint;

	        [Header("Movement Settings")]
	        public bool analogMovement;

	        [Header("Mouse Cursor Settings")] 
	        public bool UseCursorLock = false;
	        public bool cursorLocked = true;
	        public bool cursorInputForLook = true;
	        
	        // Private
	        private PlayerInput _playerInput;
	        private bool _initialized = false;
	        
	        #endregion Variables

	        #region Unity Methods

	        private void Start()
	        {
		        Initialize();
	        } // End of Unity - Start

	        #endregion Unity Methods

	        #region Callback

	        public UnityEvent OnSpaceBarPressed = new UnityEvent();
	        public UnityEvent OnScreenPressed = new UnityEvent();

	        #endregion Callback

	        #region Get input from Player Input

#if ENABLE_INPUT_SYSTEM
	        public void OnInputPosition(InputValue value) => SetInputPosition(value.Get<Vector2>());
	        public void OnMove(InputValue value) => SetMoveInput(value.Get<Vector2>());
	        public void OnLook(InputValue value)
	        {
		        if(cursorInputForLook)
		        {
			        SetLookInput(value.Get<Vector2>());
		        }
	        }

	        public void OnScreenPress(InputValue value) => SetScreenPressInput(value.isPressed);
	        public void OnSpaceBar(InputValue value) => SetSpaceBarInput(value.isPressed);
	        public void OnSprint(InputValue value) => SetSprintInput(value.isPressed);
	        
#endif


	        #endregion Get input from Player Input
	        
	        #region Set Input Methods
			private void SetInputPosition(Vector2 newInputPosition) => InputPosition = newInputPosition;
	        private void SetMoveInput(Vector2 newMoveDirection) => MoveCoordinate = newMoveDirection;
	        private void SetLookInput(Vector2 newLookDirection) => LookCoordinate = newLookDirection;
	        
	        private void SetScreenPressInput(bool newScreenPressState)
	        {
		        OnScreenPressed.Invoke();
		        ScreenPressed = newScreenPressState;
	        }

	        private void SetSpaceBarInput(bool newSpaceBarState)
	        {
		        OnSpaceBarPressed.Invoke();
		        SpaceBarPressed = newSpaceBarState;
	        }

	        private void SetSprintInput(bool newSprintState) =>  Sprint = newSprintState;
	        
	        #endregion Set Input Methods

	        #region Private Methods

	        private void Initialize()
	        {
		        SetInputType();

		        _initialized = true;
	        } // End of Initialize

	        private void SetInputType()
	        {
		        _playerInput = GetComponent<PlayerInput>();

		        switch (_playerInput.currentControlScheme)
		        {
			        case "KeyboardMouse":
				        CurrentDevice = DeviceType.PC;
				        break;
			        case "Touch":
				        CurrentDevice = DeviceType.Touch;
				        break;
			        case "Gamepad":
				        CurrentDevice = DeviceType.GamePad;
				        break;
			        case "Joystick":
				        CurrentDevice = DeviceType.Joystick;
				        break;
			        case "XR":
				        CurrentDevice = DeviceType.XR;
				        break;
		        }
		        
	        } // End of SetInputType
	        private void OnApplicationFocus(bool hasFocus)
	        {
		        if(!UseCursorLock) return;
		        SetCursorState(cursorLocked);
	        }
	        private void SetCursorState(bool newState)
	        {
		        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	        }

	        #endregion

        } 
    }
}



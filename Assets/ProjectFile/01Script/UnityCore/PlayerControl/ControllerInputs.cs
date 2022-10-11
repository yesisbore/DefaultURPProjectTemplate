using GlobalType;
using UnityEngine;
using DeviceType = GlobalType.DeviceType;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

namespace UnityCore
{
    namespace PlayerControl
    {
        public class ControllerInputs : MonoBehaviour
        {
	        #region Variables

	        // Public
	        public DeviceType CurrentDevice = DeviceType.PC;
	        
	        [Header("Character Input Values")]
	        public Vector2 inputPosition;
	        public Vector2 move;
	        public Vector2 look;
	        public bool screenPressed;
	        public bool jump;
	        public bool sprint;

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
	        public void OnJump(InputValue value) => SetJumpInput(value.isPressed);
	        public void OnSprint(InputValue value) => SetSprintInput(value.isPressed);
	        
#endif


	        #endregion Get input from Player Input
	        
	        #region Set Input Methods
			private void SetInputPosition(Vector2 newInputPosition) => inputPosition = newInputPosition;
	        private void SetMoveInput(Vector2 newMoveDirection) => move = newMoveDirection;
	        private void SetLookInput(Vector2 newLookDirection) => look = newLookDirection;
	        
	        private void SetScreenPressInput(bool newScreenPressState) => screenPressed = newScreenPressState;
	        private void SetJumpInput(bool newJumpState) => jump = newJumpState;
	        private void SetSprintInput(bool newSprintState) =>  sprint = newSprintState;
	        
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



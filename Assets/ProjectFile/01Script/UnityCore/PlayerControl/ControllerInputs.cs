using UnityEngine;
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
	        [Header("Character Input Values")]
	        public Vector2 move;
	        public Vector2 look;
	        public bool jump;
	        public bool sprint;

	        [Header("Movement Settings")]
	        public bool analogMovement;

	        [Header("Mouse Cursor Settings")] 
	        public bool UseCursorLock = false;
	        public bool cursorLocked = true;
	        public bool cursorInputForLook = true;
	        
	        // Private

	        #endregion Variables

	        #region Public Methods

#if ENABLE_INPUT_SYSTEM
	        public void OnMove(InputValue value)
	        {
		        MoveInput(value.Get<Vector2>());
	        }

	        public void OnLook(InputValue value)
	        {
		        if(cursorInputForLook)
		        {
			        LookInput(value.Get<Vector2>());
		        }
	        }

	        public void OnJump(InputValue value)
	        {
		        JumpInput(value.isPressed);
	        }

	        public void OnSprint(InputValue value)
	        {
		        SprintInput(value.isPressed);
	        }
#endif
	        
	        public void MoveInput(Vector2 newMoveDirection)
	        {
		        move = newMoveDirection;
	        } 

	        public void LookInput(Vector2 newLookDirection)
	        {
		        look = newLookDirection;
	        }

	        public void JumpInput(bool newJumpState)
	        {
		        jump = newJumpState;
	        }

	        public void SprintInput(bool newSprintState)
	        {
		        sprint = newSprintState;
	        }

	        private void OnApplicationFocus(bool hasFocus)
	        {
		        if(!UseCursorLock) return;
		        SetCursorState(cursorLocked);
	        }

	        #endregion Public Methods

	        #region Private Methods

	        private void SetCursorState(bool newState)
	        {
		        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	        }

	        #endregion

        } 
    }
}



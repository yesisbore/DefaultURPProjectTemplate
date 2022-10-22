using GlobalType;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityCore
{
    namespace PlayerControl
    {
        namespace Controller
        {
            [RequireComponent(typeof(PlayerInput))]
            [RequireComponent(typeof(ControllerInputs))]
            public class SimpleFirstPersonController : MonoBehaviour
            {
				#region Variables

				public bool DebugMode = false;
 
                // Public Variables
    
                // Private Variables
                
                [Header("Player")]
                [Tooltip("Move speed of the character in m/s")]
                [SerializeField] private float _moveSpeed = 4.0f;
                [Tooltip("Sprint speed of the character in m/s")]
                [SerializeField] private float _sprintSpeed = 7.0f;
                [Tooltip("Rotation speed of the character")]
                [SerializeField] private float _rotationSpeed = 20.0f;
                [Tooltip("Acceleration and deceleration")]
                [SerializeField] private float _speedChangeRate = 10.0f;
                
                [Header("Cinemachine")]
                [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
                [SerializeField] private GameObject _cinemachineCameraTarget;
                [Tooltip("How far in degrees can you move the camera up")]
                [SerializeField] private float _topClamp = 70.0f;
                [Tooltip("How far in degrees can you move the camera down")]
                [SerializeField] private float _bottomClamp = -60.0f;

                // Cinemachine
                private float _cinemachineTargetPitch;

                // Player
                private float _speed;
                private float _rotationVelocity;
                
                private PlayerInput _playerInput;
                private CharacterController _controller;
                private ControllerInputs _input;
                private GameObject _mainCamera;
                
                // Timeout deltaTime
                private float _jumpTimeoutDelta;
                private float _fallTimeoutDelta;
                
                private const float _threshold = 0.01f;

                private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

                #endregion Variables

                #region Unity Methods

                private void Update()
                {
                    Move();
                } // End of Unity - Update
                private void LateUpdate()
                {
                    CameraRotation();
                } // End of Unity - LateUpdate
                
                #endregion Unity Methods

                #region Public Methods

                public void Initialize(float moveSpeed,float rotationSpeed)
                {
	                _rotationSpeed = rotationSpeed;
	                _moveSpeed = moveSpeed;
	                
                    GetComponents();
                } // End of Initialize
                #endregion Public Methods
    
                #region Private Methods

                private void GetComponents()
                {
                    // get a reference to our main camera
                    if (_mainCamera == null)
                    {
                        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                    }

                    _cinemachineCameraTarget = GameObject.FindGameObjectWithTag("CinemachineTarget");
                    _controller = GetComponent<CharacterController>();
                    _input = GetComponent<ControllerInputs>();
			        _playerInput = GetComponent<PlayerInput>();
                } // End of GetComponents

				private void Move()
				{
					// Set target speed based on move speed, sprint speed and if sprint is pressed
					var targetSpeed = _input.Sprint ? _sprintSpeed : _moveSpeed;

					// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
					// if there is no input, set the target speed to 0
					if (_input.MoveCoordinate == Vector2.zero) targetSpeed = 0.0f;
					
					// a reference to the players current horizontal velocity
					var currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
					
					var speedOffset = 0.1f;
					var inputMagnitude = _input.analogMovement ? _input.MoveCoordinate.magnitude : 1f;
					
					// accelerate or decelerate to target speed
					if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
					{
						// creates curved result rather than a linear one giving a more organic speed change
						// note T in Lerp is clamped, so we don't need to clamp our speed
						_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * _speedChangeRate);
					
						// round speed to 3 decimal places
						_speed = Mathf.Round(_speed * 1000f) / 1000f;
					}
					else
					{
						_speed = targetSpeed;
					}
					
					// normalise input direction
					Vector3 inputDirection = new Vector3(_input.MoveCoordinate.x, 0.0f, _input.MoveCoordinate.y).normalized;
					
					// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
					// if there is a move input rotate player when the player is moving
					if (_input.MoveCoordinate != Vector2.zero)
					{
						// move
						inputDirection = transform.right * _input.MoveCoordinate.x + transform.forward * _input.MoveCoordinate.y;
					}
					
					// move the player
					_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) );
				} // End of Move
				
				private void CameraRotation()
				{
					// if there is an input
					if (_input.LookCoordinate.sqrMagnitude >= _threshold)
					{
						//Don't multiply mouse input by Time.deltaTime
						var deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
						
						_cinemachineTargetPitch += -_input.LookCoordinate.y * _rotationSpeed * deltaTimeMultiplier;
						_rotationVelocity = _input.LookCoordinate.x * _rotationSpeed * deltaTimeMultiplier;
					
						// clamp our pitch rotation
						_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
					
						// Update Cinemachine camera target pitch
						_cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
					
						// rotate the player left and right
						transform.Rotate(Vector3.up * _rotationVelocity);
					}
				} // End of CameraRotation
				
				private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
				{
					if (lfAngle < -360f) lfAngle += 360f;
					if (lfAngle > 360f) lfAngle -= 360f;
					return Mathf.Clamp(lfAngle, lfMin, lfMax);
				} // End of ClampAngle
				
                #endregion Private Methods

                #region Debug
                
                private void Log(string msg)
                {
	                if(!DebugMode) return;
            
	                Logger.Log<SimpleFirstPersonController>( msg);
                }
            
                private void LogWarning(string msg)
                {
	                if(!DebugMode) return;
                        
	                Logger.LogWarning<SimpleFirstPersonController>(msg);
                }

                #endregion Debug
            }
        }
    }
}


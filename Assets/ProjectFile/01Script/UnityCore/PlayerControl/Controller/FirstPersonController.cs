using GlobalType;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityCore
{
    namespace PlayerControl
    {
        namespace Controller
        {
            [RequireComponent(typeof(CharacterController))]
            [RequireComponent(typeof(PlayerInput))]
            [RequireComponent(typeof(ControllerInputs))]
            public class FirstPersonController : MonoBehaviour
            {
                #region Variables
 
                public DebugModeType DebugMode = DebugModeType.Global;
 
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

                [Space(10)]
                [Tooltip("The height the player can jump")]
                [SerializeField] private float _jumpHeight = 1.2f;
                [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
                [SerializeField] private float _gravity = -15.0f;

                [Space(10)]
                [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
                [SerializeField] private float _jumpTimeout = 0.1f;
                [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
                [SerializeField] private float _fallTimeout = 0.15f;
                
                [Header("Player Grounded")]
                [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
                [SerializeField] private bool _grounded = true;
                [Tooltip("Useful for rough ground")]
                [SerializeField] private float _groundedOffset = -0.14f;
                [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
                [SerializeField] private float _groundedRadius = 0.5f;
                [Tooltip("What layers the character uses as ground")]
                [SerializeField] private LayerMask _groundLayers;
                
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
                private float _verticalVelocity;
                private float _terminalVelocity = 53.0f;
                
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
                    JumpAndGravity();
                    GroundedCheck();
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
                    ResetTimeout();
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
                private void ResetTimeout()
                {
                    _jumpTimeoutDelta = _jumpTimeout;
                    _fallTimeoutDelta = _fallTimeout;
                } // End of RestTimeout

                // Update
                private void JumpAndGravity()
                {
	                if (_grounded)
	                {
	                	// reset the fall timeout timer
	                	_fallTimeoutDelta = _fallTimeout;
	                
	                	// stop our velocity dropping infinitely when grounded
	                	if (_verticalVelocity < 0.0f)
	                	{
	                		_verticalVelocity = -2f;
	                	}
	                
	                	// Jump
	                	if (_input.jump && _jumpTimeoutDelta <= 0.0f)
	                	{
	                		// the square root of H * -2 * G = how much velocity needed to reach desired height
	                		_verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
	                	}
	                
	                	// jump timeout
	                	if (_jumpTimeoutDelta >= 0.0f)
	                	{
	                		_jumpTimeoutDelta -= Time.deltaTime;
	                	}
	                }
	                else
	                {
	                	// reset the jump timeout timer
	                	_jumpTimeoutDelta = _jumpTimeout;
	                
	                	// fall timeout
	                	if (_fallTimeoutDelta >= 0.0f)
	                	{
	                		_fallTimeoutDelta -= Time.deltaTime;
	                	}
	                
	                	// if we are not grounded, do not jump
	                	_input.jump = false;
	                }
	                
	                // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
	                if (_verticalVelocity < _terminalVelocity)
	                {
	                	_verticalVelocity += _gravity * Time.deltaTime;
	                }
                } // End of JumpAndGravity
                private void GroundedCheck()
				{
					// set sphere position, with offset
					Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
					_grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers, QueryTriggerInteraction.Ignore);
				} // End of CheckGround
				private void Move()
				{
					// Set target speed based on move speed, sprint speed and if sprint is pressed
					var targetSpeed = _input.sprint ? _sprintSpeed : _moveSpeed;

					// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
					// if there is no input, set the target speed to 0
					if (_input.move == Vector2.zero) targetSpeed = 0.0f;
					
					// a reference to the players current horizontal velocity
					var currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
					
					var speedOffset = 0.1f;
					var inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
					
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
					Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
					
					// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
					// if there is a move input rotate player when the player is moving
					if (_input.move != Vector2.zero)
					{
						// move
						inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
					}
					
					// move the player
					_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
				} // End of Move
				
				private void CameraRotation()
				{
					// if there is an input
					if (_input.look.sqrMagnitude >= _threshold)
					{
						//Don't multiply mouse input by Time.deltaTime
						float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
						
						_cinemachineTargetPitch += -_input.look.y * _rotationSpeed * deltaTimeMultiplier;
						_rotationVelocity = _input.look.x * _rotationSpeed * deltaTimeMultiplier;
					
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
                
                private void OnDrawGizmosSelected()
                {
	                Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
	                Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);
                
	                if (_grounded) Gizmos.color = transparentGreen;
	                else Gizmos.color = transparentRed;
                
	                // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
	                Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z), _groundedRadius);
                }
                
                private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;
                private void Log(string msg)
                {
                    if(CheckDebugMode) return;
                
                    Debug.Log("[Controller - FirstPerson]: " + msg);
                }
                
                private void LogWarning(string msg)
                {
                    if(CheckDebugMode) return;
                            
                    Debug.Log("[Controller - FirstPerson]: " + msg);
                }

                #endregion Debug
            }
        }
    }
}


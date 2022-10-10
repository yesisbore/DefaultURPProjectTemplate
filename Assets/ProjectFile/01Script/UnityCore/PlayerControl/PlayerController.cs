using GlobalType;
using UnityCore.PlayerControl.Controller;
using UnityEngine;

namespace UnityCore
{
    namespace PlayerControl
    {
        public class PlayerController : MonoBehaviour
        {
            #region Variables

            public DebugModeType DebugMode = DebugModeType.Global;

            // Public Variables

            
            // Private Variables
            [SerializeField] private ControllerType _controllerType = ControllerType.FirstPerson;
            [SerializeField] private float _moveSpeed = 4.0f;
            [SerializeField] private float _rotationSpeed = 20.0f;

            #endregion Variables

            #region Unity Methods

            private void Start()
            {
                Initialize();
            } // End of Unity - Start

            #endregion Unity Methods

            #region Public Methods

            #endregion Public Methods

            #region Private Methods

            private void Initialize()
            {
                SetController();
            } // End of Initialize

            private void SetController()
            {
                switch (_controllerType)
                {
                    case ControllerType.SimpleFirstPerson:
                        gameObject.AddComponent<SimpleFirstPersonController>().Initialize(_moveSpeed,_rotationSpeed);
                        break;
                    case ControllerType.FirstPerson:
                        gameObject.AddComponent<FirstPersonController>().Initialize(_moveSpeed,_rotationSpeed);
                        break;
                    case ControllerType.ThirdPerson:
                        //gameObject.AddComponent<FirstPersonController>().Initialize(_useJoyStick,_moveSpeed);
                        break;
                    case ControllerType.TopDown:
                        //gameObject.AddComponent<FirstPersonController>().Initialize(_useJoyStick,_moveSpeed);
                        break;
                    case ControllerType.Default2D:
                        //gameObject.AddComponent<FirstPersonController>().Initialize(_useJoyStick,_moveSpeed);
                        break;
                    case ControllerType.Horizontal2D:
                        //gameObject.AddComponent<FirstPersonController>().Initialize(_useJoyStick,_moveSpeed);
                        break;
                    case ControllerType.Vertical2D:
                        //gameObject.AddComponent<FirstPersonController>().Initialize(_useJoyStick,_moveSpeed);
                        break;
                }
            }

            #endregion Private Methods

            #region Debug

            private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;

            private void Log(string msg)
            {
                if (CheckDebugMode) return;

                Debug.Log("[SimpleCameraController]: " + msg);
            }

            private void LogWarning(string msg)
            {
                if (CheckDebugMode) return;

                Debug.Log("[SimpleCameraController]: " + msg);
            }

            #endregion Debug
        }
    }
}
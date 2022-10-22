using UnityCore.PlayerControl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UnityCore
{
    namespace Shooter
    {
        public class Shooter : MonoBehaviour
        {
            #region Variables
         
            public bool DebugMode = false;

            // Public Variables
            
            // Private Variables
            [SerializeField] private AssetReference _bulletPrefab;
            [SerializeField] private float _damage;
            [SerializeField] private float _bulletSpeed;
            
            private Camera _cam;
            
            private Vector2 InputPosition => ControllerInputs.Instance.InputPosition;
            
            #endregion Variables
        
            #region Unity Methods
        
            private void Start() { Initialize();} // End of Unity - Start

            #endregion Unity Methods
        
            #region Public Methods
        
            #endregion Public Methods
            
            #region Private Methods

            private void Initialize()
            {
                GetComponents();
                SubscribeInputEvent();
                
                Log("Initialized");
            } // End of Initialize
            private void GetComponents()
            {
                _cam = GetComponent<Camera>();
            } // End of GetComponents
            
            private void SubscribeInputEvent()
            {
                ControllerInputs.Instance.OnScreenPressed .AddListener(Shoot);
            } // End of SubscribeInputEvent
            
            private void Shoot()
            {
                GenerateBullet();
                Log("Shoot!");
            } // End of Shoot

            private void GenerateBullet()
            {
                _bulletPrefab.InstantiateAsync().Completed += (op) =>
                {
                    var bullet = op.Result;
                    var bulletLength = bullet.transform.lossyScale.z;
                    var bulletPosition = _cam.ScreenToWorldPoint(new Vector3(InputPosition.x, InputPosition.y, _cam.nearClipPlane +bulletLength));
                    var bulletForward = _cam.ScreenToWorldPoint(new Vector3(InputPosition.x, InputPosition.y, _cam.farClipPlane)) ; 

                    bullet.transform.forward = bulletForward.normalized;
                    bullet.transform.position = bulletPosition;
                    bullet.GetComponent<Bullet>().Shoot(_damage,_bulletSpeed);
                    Log("Generated Bullet");
                };
            } // End of GenerateBullet
            
            #endregion Private Methods
        
            #region Debug
        
            private void Log(string msg)
            {
                if(!DebugMode) return;
            
                Logger.Log<Shooter>( msg);
            }
            
            private void LogWarning(string msg)
            {
                if(!DebugMode) return;
                        
                Logger.LogWarning<Shooter>(msg);
            }
        
            #endregion Debug
        }
    }
}


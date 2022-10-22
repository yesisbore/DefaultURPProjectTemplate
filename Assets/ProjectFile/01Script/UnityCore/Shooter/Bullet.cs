using System;
using GlobalInterface;
using UnityEngine;

namespace UnityCore
{
    namespace Shooter
    {
        [RequireComponent(typeof(Collider))]
        public class Bullet : MonoBehaviour
        {
            #region Variables
         
            public bool DebugMode = false;
            
            // Public Variables
            
            // Private Variables
            private Transform _transform;
            private float _damage;
            private float _moveSpeed;
            private bool _isShoot = false;
            private bool _isEntered = false;
            
            #endregion Variables
        
            #region Unity Methods
            private void Update() { Move();} // End of Unity - Update

            private void OnCollisionEnter(Collision collision)
            {
                if(_isEntered) return;

                _isEntered = true;
                ContactPoint contact = collision.contacts[0];
                IDamgeable damgeableObject = collision.transform.GetComponent<IDamgeable>();
                damgeableObject?.TakeHit(_damage,contact.point);

                Log("Hit : " + collision.gameObject.name );
                Destroy(this.gameObject);
            }

            #endregion Unity Methods
        
            #region Public Methods

            public void Shoot(float damage, float moveSpeed)
            {
                Initialize();

                _damage = damage;
                _moveSpeed = moveSpeed;
                _isShoot = true;
                gameObject.SetActive(true);
            } // End of Shoot
            
            #endregion Public Methods
            
            #region Private Methods
        
            private void Initialize(){GetComponents();} // End of Initialize
            private void GetComponents()
            {
                _transform = GetComponent<Transform>();
            } // End of GetComponents

            private void Move()
            {
                if(!_isShoot) return;

                transform.position += transform.forward * (_moveSpeed * Time.deltaTime);
            } // End of Move
            #endregion Private Methods
        
            #region Debug
        
            private void Log(string msg)
            {
                if(!DebugMode) return;
            
                Logger.Log<Bullet>( msg);
            }
            
            private void LogWarning(string msg)
            {
                if(!DebugMode) return;
                        
                Logger.LogWarning<Bullet>(msg);
            }
        
            #endregion Debug
        }
    }
}


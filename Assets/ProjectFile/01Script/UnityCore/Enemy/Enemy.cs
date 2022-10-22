using GlobalInterface;
using UnityEngine;

namespace UnityCore
{
    namespace Enemy
    {
       public class Enemy : MonoBehaviour,IDamgeable
       {
           #region Variables
        
           public bool DebugMode = false;
        
           // Public Variables
           
           // Private Variables
           
           #endregion Variables
       
           #region Unity Methods
       
           //private void Start() { Initialize();} // End of Unity - Start
       
           //private void Update(){} // End of Unity - Update
       
           #endregion Unity Methods
       
           #region Public Methods
       
           public void TakeHit(float damage, Vector3 localHitPos)
           {
               var hitWorldPosition = transform.position + localHitPos;
               
               Log("HitWorldPosition : " + hitWorldPosition);
               Log(gameObject.name + " - Damaged : " + damage);
           }
           
           #endregion Public Methods
           
           #region Private Methods
       
           //private void Initialize(){GetComponents();} // End of Initialize
           
           //private void GetComponents(){} // End of GetComponents

           #endregion Private Methods
       
           #region Debug
       
           private void Log(string msg)
           {
               if(!DebugMode) return;
           
               Logger.Log<Enemy>( msg);
           }
           
           private void LogWarning(string msg)
           {
               if(!DebugMode) return;
                       
               Logger.LogWarning<Enemy>(msg);
           }
       
           #endregion Debug



       } 
    }
}


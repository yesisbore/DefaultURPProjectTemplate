using System.Collections;
using System.Collections.Generic;
using UnityCore.PlayerControl;
using UnityEngine;

namespace Effect
{
    public class SpawnEffect : MonoBehaviour
    {
   
        #region Variables
 
        public bool DebugMode = false;
 
        // Public Variables
    
        // Private Variables
        [SerializeField] private AnimationCurve _spawnCurve;
        [SerializeField] private float _spawnTime = 3.0f;
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private float _enterPositionMultiple = 10.0f;
        
        // Hash
        private static readonly int SpawnProgress = Shader.PropertyToID("_SpawnProgress");
        private static readonly int Landing = Animator.StringToHash("Landing");

        private Renderer _renderer;
        private Animator _animator;
        private Coroutine _spawnCoroutine;
        private bool _isSpawned = false;
        private bool _isLanding = false;
        private float _multipleTime;
        private Vector3 _enterPosition;

        #endregion Variables

        #region Unity Methods

        private void Start() { Initialize();} // End of Unity - Start

        //private void Update(){} // End of Unity - Update

        #endregion Unity Methods

        #region Public Methods

        #endregion Public Methods
    
        #region Private Methods

        private void Initialize()
        {
          GetComponents();
          SubscribeInputEvent();
          PlaceObjectInitPosition();
          
          _renderer.material.SetFloat(SpawnProgress, 1.0f);
        } // End of Initialize

        private void GetComponents()
        {
            _renderer = GetComponentInChildren<Renderer>();
            _animator = GetComponent<Animator>();
        } // End of GetComponents
        private void SubscribeInputEvent()
         {
             ControllerInputs.Instance.OnSpaceBarPressed.AddListener(SpawnProcess);
         } // End of SubscribeInputEvent
        private void PlaceObjectInitPosition()
        {
            _enterPosition = _spawnTransform.position + Vector3.up * _enterPositionMultiple;
            transform.position = _enterPosition;
        } // End of PlaceObjectInitPosition
        
         private void SpawnProcess()
         {
             Log("Start Spawn Process");

             var targetPosition = _isSpawned ? _enterPosition :_spawnTransform.position;
             var targetValue = _isSpawned ? 1.0f : 0.0f;
             _isSpawned = !_isSpawned;
             _isLanding = false;
             
             if(_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);

             _spawnCoroutine = StartCoroutine(Coroutine_Spawn(targetPosition,targetValue));
         } // End of SpawnProcess

         private IEnumerator Coroutine_Spawn(Vector3 targetPosition,float targetValue)
         {
             var startPosition = transform.position;
             var startValue = _renderer.material.GetFloat(SpawnProgress);
             var lerp = 0.0f;

             _multipleTime = 1.0f / _spawnTime;

             while (lerp < 1)
             {
                 transform.position = Vector3.Lerp(startPosition, targetPosition, _spawnCurve.Evaluate(lerp));
                 _renderer.material.SetFloat(SpawnProgress, Mathf.Lerp(startValue,targetValue,_spawnCurve.Evaluate(lerp)));
                 lerp += Time.deltaTime * _multipleTime;
                 if (_isSpawned && lerp >=0.65f)
                 {
                     AnimateLanding();
                 }
                 yield return null;
             }
             _renderer.material.SetFloat(SpawnProgress, targetValue);
             Log("End Spawn Process");
         } // End of Coroutine_Spawn


        #endregion Private Methods

        #region Animation

        private void AnimateLanding()
        {
            if(_isLanding) return;
            
            _isLanding = true;
            _animator.SetTrigger(Landing);
        } // End of Landing

        #endregion
        #region Debug

        private void Log(string msg)
        {
            if(!DebugMode) return;
        
            Logger.Log<SpawnEffect>( msg);
        }
        
        private void LogWarning(string msg)
        {
            if(!DebugMode) return;
                    
            Logger.LogWarning<SpawnEffect>(msg);
        }

        #endregion Debug
    }
}


using System.Collections;
using GlobalInterface;
using UnityCore.PlayerControl;
using UnityEngine;

namespace Effect
{
    public class Shield : MonoBehaviour,IDamgeable
    {
        #region Variables
        
        public bool DebugMode = false;
        
        // Public Variables
           
        // Private Variables    
        [SerializeField] private AnimationCurve _displacementCurve;
        [SerializeField] private float _displacementMagnitude;
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private float _disolveSpeed;
        
        private Renderer _renderer;
        Coroutine _disolveCoroutine;
        private bool _isShieldOn = false;
        private static readonly int Disolve = Shader.PropertyToID("_Disolve");
        private static readonly int HitPos = Shader.PropertyToID("_HitPos");
        private static readonly int DisplacementStrength = Shader.PropertyToID("_DisplacementStrength");

        #endregion Variables
       
        #region Unity Methods
       
        private void Start() { Initialize();} // End of Unity - Start

        #endregion Unity Methods
       
        #region Public Methods
       
        public void TakeHit(float damage, Vector3 hitPos)
        {
            HitShield(hitPos);
            
            Log("Bullet : " + hitPos);
            Log(gameObject.name + " - Damaged : " + damage);
        }
           
        #endregion Public Methods
           
        #region Private Methods

        private void Initialize()
        {
            GetComponents();
            SubscribeInputEvent();
        } // End of Initialize

        private void GetComponents()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.SetFloat(Disolve, 1.0f);
            _displacementMagnitude *= transform.localScale.magnitude * 0.5f;
        } // End of GetComponents

        private void SubscribeInputEvent()
        {
            ControllerInputs.Instance.OnSpaceBarPressed .AddListener(ShieldControl);
        } // End of SubscribeInputEvent
        private void ShieldControl()
        {
            Log("Shield Activate");
            _isShieldOn = !_isShieldOn;
            var target = _isShieldOn ? 0.0f : 1.0f;
            if (_disolveCoroutine != null)
            {
                StopCoroutine(_disolveCoroutine);
            }
            _disolveCoroutine = StartCoroutine(Coroutine_DisolveShield(target));
        } // End of ShieldControl
        
        IEnumerator Coroutine_DisolveShield(float target)
        {
            float start = _renderer.material.GetFloat(Disolve);
            float lerp = 0;
            while (lerp < 1)
            {
                _renderer.material.SetFloat(Disolve, Mathf.Lerp(start,target,lerp));
                lerp += Time.deltaTime * _disolveSpeed;
                yield return null;
            }
        }
        
        // Hit Method 
        private void HitShield(Vector3 hitPos)
        {
            Log("Hit Shield");
            _renderer.material.SetVector(HitPos, hitPos);
            StopAllCoroutines();
            StartCoroutine(Coroutine_HitDisplacement());
        }
        IEnumerator Coroutine_HitDisplacement()
        {
            float lerp = 0;
            while (lerp < 1)
            {
                _renderer.material.SetFloat(DisplacementStrength, _displacementCurve.Evaluate(lerp) * _displacementMagnitude);
                lerp += Time.deltaTime*_lerpSpeed;
                yield return null;
            }
        }
        #endregion Private Methods
       
        #region Debug
       
        private void Log(string msg)
        {
            if(!DebugMode) return;
           
            Logger.Log<Shield>( msg);
        }
           
        private void LogWarning(string msg)
        {
            if(!DebugMode) return;
                       
            Logger.LogWarning<Shield>(msg);
        }
       
        #endregion Debug
    }
}


using UnityEngine;

namespace Effect
{
    public class SimpleLiquid : MonoBehaviour
    {
        #region Variables

        // Public Variables

        // Private Variables

        [SerializeField] private GameObject _liquid;

        [Header("Setting")]
        [SerializeField] private float _maxWobble = 0.01f;
        [SerializeField] private float _wobbleSpeed = 1f;
        [SerializeField] private float _recovery = 1f;
        
        private Material _liquidRenderer;
        private readonly int _idWobbleX =  Shader.PropertyToID("_WobbleX"); 
        private readonly int _idWobbleZ = Shader.PropertyToID("_WobbleZ");
        
        private Vector3 _lastPos;
        private Vector3 _velocity;
        private Vector3 _lastRot;
        private Vector3 _angularVelocity;
        
        private float _wobbleAmountX;
        private float _wobbleAmountZ;
        private float _wobbleAmountToAddX;
        private float _wobbleAmountToAddZ;
        private float _pulse;
        private float _time = 0.5f;

        #endregion Variables

        #region Unity Methods

        private void Start()
        {
            Initialize();
        } // End of Unity - Start

        private void Update()
        {
            Wobble();
        } // End of Unity - Update

        #endregion Unity Methods

        #region Private Methods

        private void Initialize()
        {
            GetComponents();
            InitValue();
        } // End of Initialize
        private void GetComponents()
        {
            _liquidRenderer = _liquid.GetComponent<Renderer>().material;
        } // End of GetComponents

        private void InitValue()
        {
            _pulse = 2 * Mathf.PI * _wobbleSpeed;
        } // End of InitValue
        private void Wobble()
        {
            _time += Time.deltaTime;
            
            DecreaseWobble();
            SetValueShaderProperty();
            CalcVelocity();
            
            // keep last position
            _lastPos = transform.position;
            _lastRot = transform.rotation.eulerAngles;
        } // End of Wobble

        private void DecreaseWobble()
        {
            _wobbleAmountToAddX = Mathf.Lerp(_wobbleAmountToAddX, 0, Time.deltaTime * (_recovery));
            _wobbleAmountToAddZ = Mathf.Lerp(_wobbleAmountToAddZ, 0, Time.deltaTime * (_recovery));
            
            // make a sine wave of the decreasing wobble
            _wobbleAmountX = _wobbleAmountToAddX * Mathf.Sin(_pulse * _time);
            _wobbleAmountZ = _wobbleAmountToAddZ * Mathf.Sin(_pulse * _time);
        } // End of DecreaseWobble
        private void SetValueShaderProperty()
        {
            _liquidRenderer.SetFloat(_idWobbleX, _wobbleAmountX);
            _liquidRenderer.SetFloat(_idWobbleZ, _wobbleAmountZ);
        } // End of SetShaderPropertyValue
        private void CalcVelocity()
        {
            // velocity
            _velocity = (_lastPos - transform.position) / Time.deltaTime;
            _angularVelocity = transform.rotation.eulerAngles - _lastRot;

            // add clamped velocity to wobble
            _wobbleAmountToAddX += Mathf.Clamp((_velocity.x + (_angularVelocity.z * 0.2f)) * _maxWobble, -_maxWobble, _maxWobble);
            _wobbleAmountToAddZ += Mathf.Clamp((_velocity.z + (_angularVelocity.x * 0.2f)) * _maxWobble, -_maxWobble, _maxWobble);
        } // End of CalcVelocity
        
        #endregion Private Methods
    }
}
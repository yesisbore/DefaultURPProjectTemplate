using UnityEngine;
using UnityEngine.UIElements;

public class UIEventHandler : MonoBehaviour
{
    #region Variables

    public bool DebugMode = false;

    // Public Variables

    // Private Variables
    [SerializeField] private UIDocument _uiDocument;

    private Label _label;
    private int _buttonClickCount = 0;
    private Toggle _toggle;
    private Button _button;

    #endregion Variables

    #region Unity Methods

    private void Start()
    {
        Initialize();
    } // End of Unity - Start

    //private void Update(){} // End of Unity - Update

    private void OnDestroy()
    {
        //_button.clickable.clicked -= OnClicked;
        //_toggle.UnregisterValueChangedCallback(OnToggleValueChanged);
    } // End of Unity - OnDestroy

    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    private void Initialize()
    {
        GetComponents();
        //SubscribeInputEvent();
    } // End of Initialize
    
    // private void SubscribeInputEvent()
    // {
    //     ControllerInputs.Instance.OnSpaceBarPressed .AddListener(ShieldControl);
    // } // End of SubscribeInputEvent
    
    private void GetComponents()
    {
        var rootElement = _uiDocument.rootVisualElement;

        // “EventButton”이라는 이름의 VisualElement 버튼을 검색하는 스크립트입니다.
        // rootElement.Query<> 및 rootElement.Q<> 모두 사용 가능합니다.
        //_button = rootElement.Q<Button>("EventButton");

        // 버튼과 같은 값이 없는 요소는
        // clickable.clicked와 함께 콜백을 등록할 수 있습니다.
        //_button.clickable.clicked += OnButtonClicked;

        // “ColorToggle”이라는 이름의 VisualElement 토글을 검색하는 스크립트입니다. 
        //_toggle = rootElement.Query<Toggle>("ColorToggle");

        // toggle, TextField 등 변화하는 값을 포함하는 요소는 
        // INotifyValueChanged 인터페이스를 구현합니다. 
        // 즉, 이러한 요소는 RegisterValueChangedCallback 및 
        // UnRegisterValueChangedCallback을 사용합니다. 
        //_toggle.RegisterValueChangedCallback(OnToggleValueChanged);

        // 반복적으로 액세스할 것이므로 레이블에 대한 레퍼런스를 캐싱합니다.
        // 이렇게 하면 레이블을 업데이트할 때마다 
        // 비교적 많은 리소스가 소모되는 VisualElement 검색을 피할 수 있습니다.
        //_label = rootElement.Q<Label>("IncrementLabel");
        //_label.text = _buttonClickCount.ToString();
    } // End of GetComponents

    private void OnToggleValueChanged(ChangeEvent<bool> evt)
    {
        Debug.Log("New toggle value is: " + evt.newValue);
    } // End of OnToggleValueChanged
    
    private void OnButtonClicked()
    {
        _buttonClickCount++;
        _label.text = _buttonClickCount.ToString();
    } // End of OnButtonClicked

    #endregion Private Methods

    #region Debug

    private void Log(string msg)
    {
        if (!DebugMode) return;

        Logger.Log<UIEventHandler>(msg);
    }

    private void LogWarning(string msg)
    {
        if (!DebugMode) return;

        Logger.LogWarning<UIEventHandler>(msg);
    }

    #endregion Debug

}
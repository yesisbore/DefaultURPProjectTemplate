using GlobalType;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    #region Variables

    //Public Variables

    public bool DebugMode = false;
    
    // Private Variables

    [Header("Audio Setting Sliders")] 
    [SerializeField] private Slider _globalVolumeMaster;
    [SerializeField] private Slider _globalVolumeBGM;
    [SerializeField] private Slider _globalVolumeEffect;
    
    private AudioSetting _audioSetting;
    
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
        AddEvent();
        
        InitSliderValue();
    } // End of Initialize

    private void InitSliderValue()
    {
        if (_globalVolumeMaster != null) _globalVolumeMaster.value = GameSetting.Instance.VolumeMaster;
        if (_globalVolumeBGM    != null) _globalVolumeBGM.value = GameSetting.Instance.VolumeBGM;
        if (_globalVolumeEffect != null) _globalVolumeEffect.value = GameSetting.Instance.VolumeEffect;
    } // End of InitSliderValue
    private void GetComponents()
    {
        if (_audioSetting == null) _audioSetting = gameObject.AddComponent<AudioSetting>();
    } // End of GetComponents

    private void AddEvent()
    {
        if (_globalVolumeMaster != null) _globalVolumeMaster.onValueChanged.AddListener(_audioSetting.GlobalVolumeControlMaster);
        if (_globalVolumeBGM    != null) _globalVolumeBGM.onValueChanged.AddListener(_audioSetting.GlobalVolumeControlBGM);
        if (_globalVolumeEffect != null) _globalVolumeEffect.onValueChanged.AddListener(_audioSetting.GlobalVolumeControlEffect);
    } // End of AddEvent
    

    private void Log(string msg)
    {
        if(!DebugMode) return;
     
        Logger.Log<SettingMenu>( msg);
    }
     
    private void LogWarning(string msg)
    {
        if(!DebugMode) return;
                 
        Logger.LogWarning<SettingMenu>(msg);
    }
    
    #endregion Private Methods
    

}

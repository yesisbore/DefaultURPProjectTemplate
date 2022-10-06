using GlobalType;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    #region Variables

    //Public Variables
    
    public DebugModeType DebugMode = DebugModeType.Global;
    
    // Private Variables
    
    #endregion Variables

    #region Public Methods

    // Volume Control Methods
    public void GlobalVolumeControlMaster(float value) => AudioController.Instance.GlobalVolumeControlMaster(value);
    public void GlobalVolumeControlBGM(float value) => AudioController.Instance.GlobalVolumeControlBGM(value);
    public void GlobalVolumeControlEffect(float value) => AudioController.Instance.GlobalVolumeControlEffect(value);
    
    // Mute Methods
    public void MasterAudioMute() => AudioController.Instance.GlobalVolumeControlMaster(0f);
    public void BGMAudioMute() => AudioController.Instance.GlobalVolumeControlBGM(0f);
    public void EffectAudioMute() => AudioController.Instance.GlobalVolumeControlEffect(0f);
    
    #endregion Public Methods
    
    #region Private Methods
    
    private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;
    private void Log(string msg)
    {
        if(CheckDebugMode) return;
                
        Debug.Log("[Setting Menu]: " + msg);
    }
    
    private void LogWarning(string msg)
    {
        if(CheckDebugMode) return;
                
        Debug.Log("[Setting Menu]: " + msg);
    }
    
    #endregion Private Methods
    

}

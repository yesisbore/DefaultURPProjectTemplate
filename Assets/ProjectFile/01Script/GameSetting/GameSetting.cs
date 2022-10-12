using GlobalType;
using UnityEngine;
using DeviceType = GlobalType.DeviceType;

public class GameSetting : SingletonScriptableObject<GameSetting>
{
    public bool DebugMode = false;
    
    [Header("Play Setting")]
    //public string name = "";

    public TargetResolution TargetResolution = GlobalType.TargetResolution.FHD;

    public DeviceType DeviceType = DeviceType.PC; 
    
    [Header("Volume Setting")] 
    [Range(0.0f,1.0f)]
    public float VolumeMaster = 1.0f;
    [Range(0.0f,1.0f)]
    public float VolumeBGM = 1.0f;
    [Range(0.0f,1.0f)]
    public float VolumeEffect = 1.0f;
    
} // End of GameSetting

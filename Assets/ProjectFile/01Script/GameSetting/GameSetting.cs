using UnityEngine;

public class GameSetting : SingletonScriptableObject<GameSetting>
{
    public string name = "";

    [Header("Volume Setting")] 
    [Range(0.0f,1.0f)]
    public float VolumeMaster = 1.0f;
    [Range(0.0f,1.0f)]
    public float VolumeBGM = 1.0f;
    [Range(0.0f,1.0f)]
    public float VolumeEffect = 1.0f;
    
} // End of GameSetting

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static string ClassName = typeof(T).Name.ToString();
    private const string SettingFileDirectory = "Assets/Resources";
    private static readonly string SettingFilePath = "Assets/Resources/" + ClassName + ".asset";

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = Resources.Load<T>(ClassName);
            
#if UNITY_EDITOR
            if (_instance == null)
            {
                if (!AssetDatabase.IsValidFolder(SettingFileDirectory))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                _instance = AssetDatabase.LoadAssetAtPath<T>(SettingFilePath);

                if (_instance == null)
                {
                    _instance = CreateInstance<T>();
                    AssetDatabase.CreateAsset(_instance,SettingFilePath);
                }
            }
#endif
            return _instance;
        }
    } // End of Instance
    
} // End of SingletonScriptableObject

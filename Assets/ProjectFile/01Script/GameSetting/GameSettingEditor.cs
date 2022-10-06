#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GameSetting))]
public class GameSettingEditor : Editor
{
    [MenuItem("Assets/Open Game Setting")]
    public static void OpenInspector()
    {
        Selection.activeObject = GameSetting.Instance;
    } // End of OpenInspector
} // End of GameSettingEditor
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void CreateEventSystem()
    {
        GameObject eventSystem = GameObject.Find("EventSystem");
        if (eventSystem != null)
            return;

        GameObject newEventSystem = new GameObject("EventSystem");
        newEventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
        newEventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
    }

    public static GameObject CreatePrefab(string prefabName, Transform p = null)
    {
        GameObject clone = Resources.Load<GameObject>("AllPrefabs/" + prefabName);
        clone = GameObject.Instantiate(clone, p);
        clone.name = prefabName;
        return clone;
    }

    public static void CreateEditorUtils()
    {
        GameObject clone = Resources.Load<GameObject>("EditorUtilities/First Person Camera Controller");
        GameObject.Instantiate(clone);
    }

}
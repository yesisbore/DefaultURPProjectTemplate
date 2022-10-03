using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Singleton

    private static SceneController _instance;
    public static SceneController Instance
    {
        get
        {
            if (_instance) return _instance;
            
            var go = new GameObject("SceneController", typeof(SceneController));
            DontDestroyOnLoad(go);
            _instance = go.GetComponent<SceneController>();
            return _instance;
        }
    }

    #endregion Singleton
    
    #region Variables
    
    private string _fadePrefab = "Fade UI";
    
    private string _targetScene;
    private float _fadeInTime;
    private float _fadeOutTime;
    private bool _isChanging = false;

    private CanvasGroup _fadeUI;
    
    #endregion Variables

    #region Scene Load Methods

    /// <summary>
    /// Calling this function creates a Fade UI prefab asynchronously in the resource folder,
    /// loads the scene asynchronously using the two parameters received,
    /// and deletes the instance when the previous scene is unloaded.
    /// </summary>
    /// <param name="sceneName">Name of the scene you want to load as a string</param>
    /// <param name="fadeInTime"></param>
    /// <param name="fadeOutTime"></param>
    public void LoadSceneAsync(string sceneName,float fadeInTime,float fadeOutTime)
    {
        if(_isChanging) return;

        _isChanging = true;
        _targetScene = sceneName;
        _fadeInTime = fadeInTime;
        _fadeOutTime = fadeOutTime;
        
        // Load Fade UI
        Addressables.InstantiateAsync(_fadePrefab,transform).Completed += (op) =>
        {
            var go = op.Result;
            _fadeUI =  go.GetComponent<CanvasGroup>();
            StartCoroutine(CO_LoadScene());
        };
        
    } // End of LoadMainScene

    #endregion Scene Load Methods

    #region CoRoutines

    private IEnumerator CO_LoadScene()
    {
        yield return StartCoroutine(CO_FadeIn());
        yield return StartCoroutine(CO_SceneChange());
        yield return StartCoroutine(CO_FadeOut());
        Addressables.Release(_fadeUI.gameObject);
        Destroy(gameObject);
    } // End of CO_LoadScene
    
    private IEnumerator CO_FadeIn()
    {
        yield return StartCoroutine(CO_FadeAnimation(1f,_fadeInTime));
    } // End of CO_FadeIn
    
    private IEnumerator CO_FadeOut()
    {
        yield return StartCoroutine(CO_FadeAnimation(0f,_fadeOutTime));
    } // End of CO_FadeOut
    
    private IEnumerator CO_FadeAnimation(float finalAlpha,float fadeTime)
    {
        while (!Mathf.Approximately(_fadeUI.alpha, finalAlpha))
        {
            _fadeUI.alpha = Mathf.MoveTowards(_fadeUI.alpha, finalAlpha, fadeTime * Time.deltaTime);
            yield return null;
        }
        
        _fadeUI.alpha = finalAlpha;
    } // End of CO_FadeAnimation

    private IEnumerator CO_SceneChange()
    {
        yield return StartCoroutine(CO_LoadTargetScene());
        yield return StartCoroutine(CO_UnLoadCurrentScene());
    } // End of CO_SceneChange
    
    private IEnumerator CO_LoadTargetScene()
    {
        // Disable current scene camera audio listener
        var mainCam = Camera.main;
        if (mainCam) mainCam.GetComponent<AudioListener>().enabled = false;
        
        var ao = SceneManager.LoadSceneAsync(_targetScene, LoadSceneMode.Additive);
        // Avoid showing the scene until the current scene is loaded.
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            Log("Load scene progress : " + ao.progress);
            
            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
        
    } // End of CO_LoadTargetScene
    
    private IEnumerator CO_UnLoadCurrentScene()
    {
        var curSceneName = SceneManager.GetActiveScene().name;
        yield return SceneManager.UnloadSceneAsync(curSceneName);
    } // End of CO_UnLoadCurrentScene

    #endregion CoRoutines

    #region Private Methods

    private void Log(string msg)
    { 
        if(!GameSetting.Instance.DebugMode) return;
        
        Debug.Log("[Scene Controller]: " + msg);
    }
    
    private void LogWarning(string msg)
    {
        if(!GameSetting.Instance.DebugMode) return;
        
        Debug.Log("[Scene Controller]: " + msg);
    }

    #endregion
}
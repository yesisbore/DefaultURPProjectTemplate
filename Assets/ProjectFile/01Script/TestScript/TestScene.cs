using System.Collections;
using System.Collections.Generic;
using UnityCore.Scene;
using UnityCore.Scnene;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScene : MonoBehaviour
{
    #region Variables


    #endregion Variables

    #region Help Methods

    public void OnLoadMainSceneTest()
    {
        SceneController.Instance.LoadSceneAsync(SceneType.DefaultScene,()=>{Debug.Log("DefaultSceneLoad");});
    } // End of OnLoadSceneTest
    
    public void OnLoadTestSceneTest()
    {
        SceneController.Instance.LoadSceneAsync(SceneType.TestScene,()=>{Debug.Log("TestSceneLoad");});
    } // End of OnLoadSceneTest
    
    #endregion Help Methods
}

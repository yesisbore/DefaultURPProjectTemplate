using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempTestMethods : MonoBehaviour
{
    #region Variables


    #endregion Variables

    #region Help Methods

    public void OnLoadMainSceneTest()
    {
        SceneController.Instance.LoadSceneAsync("MainScene",1f,1f);
    } // End of OnLoadSceneTest
    
    public void OnLoadTestSceneTest()
    {
        SceneController.Instance.LoadSceneAsync("00 Default Scene",1f,1f);
    } // End of OnLoadSceneTest
    
    #endregion Help Methods
}

using UnityCore.Scene;
using UnityCore.Scnene;
using UnityEngine;

namespace TestScript
{
    public class TestScene : MonoBehaviour
    {
        #region Variables

        public string Info = "This script is test for scene controller";
        #endregion Variables

        #region Help Methods

        public void OnLoadMainSceneTest()
        {
            SceneController.Instance.LoadSceneAsyncWithFade(SceneType.DefaultScene,()=>{Debug.Log("DefaultSceneLoad");});
        } // End of OnLoadSceneTest
    
        public void OnLoadTestSceneTest()
        {
            SceneController.Instance.LoadSceneAsync(SceneType.TestScene,()=>{Debug.Log("TestSceneLoad");});
        } // End of OnLoadSceneTest
    
        #endregion Help Methods
    }
}


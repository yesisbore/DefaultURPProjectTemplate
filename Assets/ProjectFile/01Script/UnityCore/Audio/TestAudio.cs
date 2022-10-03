using UnityEngine;

namespace UnityCore
{
    namespace Audio
    {
        public class TestAudio : MonoBehaviour
        {
            #region Variables

            public AudioController AudioController;

            #endregion Variables

            #region Unity Methods

#if UNITY_EDITOR
            private void Update()
            {
                if (Input.GetKeyUp(KeyCode.T))
                {
                    AudioController.PlayAudio(AudioType.ST_01,true,1.0f);
                }
                if (Input.GetKeyUp(KeyCode.G))
                {
                    AudioController.StopAudio(AudioType.ST_01);
                }
                if (Input.GetKeyUp(KeyCode.B))
                {
                    AudioController.RestartAudio(AudioType.ST_01);
                }
                if (Input.GetKeyUp(KeyCode.R))
                {
                    AudioController.PauseAudio(AudioType.ST_01,true,1.0f);
                }
                
                if (Input.GetKeyUp(KeyCode.Y))
                {
                    AudioController.PlayAudio(AudioType.SFX_01);
                }
                if (Input.GetKeyUp(KeyCode.H))
                {
                    AudioController.StopAudio(AudioType.SFX_01);
                }
                if (Input.GetKeyUp(KeyCode.N))
                {
                    AudioController.RestartAudio(AudioType.SFX_01);
                }
            } // End of Unity - Update 
            
#endif

            #endregion Unity Methods


        }
    }
}


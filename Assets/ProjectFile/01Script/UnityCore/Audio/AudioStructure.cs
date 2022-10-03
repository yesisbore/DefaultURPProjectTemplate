using UnityEngine;

namespace UnityCore
{
    namespace Audio
    {
        public enum AudioType
        {
            None,
            ST_01,
            ST_02,
            SFX_01,
            SFX_02
            // ..Custom audio types here
            // Ex) SFX_Reload
        }

        [System.Serializable]
        public class AudioObject
        {
            public AudioType Type;
            public AudioClip Clip;
        }

        [System.Serializable]
        public class AudioTrack
        {
            public AudioSource Source;
            public AudioObject[] Audio;
        }

        public class AudioJob
        {
            public AudioAction Action;
            public AudioType Type;
            public bool Fade;
            public float Delay;
            
            public AudioJob(AudioAction action, AudioType type, bool fade, float delay)
            {
                Action = action;
                Type = type;
                Fade = fade;
                Delay = delay;
            }
        }

        public enum AudioAction
        {
            START,
            STOP,
            PAUSE,
            RESTART
        }
    }
}
using System;
using System.Collections;
using GlobalType;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityCore
{
    namespace Audio
    {
        public class AudioController : MonoBehaviour
        {
            #region Variables

            // Public Variables
            
            public static AudioController Instance;

            public DebugModeType DebugMode = DebugModeType.Global;
            public AudioTrack[] Tracks;
            
            // Private Variables
            private Hashtable _audioTable; // Relationship between audio types (key) and audio tracks (value)
            private Hashtable _jobTable;   // Relationship between audio types (key) and jobs (value) (Coroutine, IEnumerator)

            
            // Mixer Control
            private AudioMixer _globalMixer;
            
            private const string _volumeNameMaster = "MasterVolume";
            private const string _volumeNameBGM    = "BGMVolume";
            private const string _volumeNameEffect = "EffectVolume";
            
            #endregion Variables

            #region Unity Methods
            
            private void Awake()
            {
                if (!Instance)
                {
                    Configure();
                }
            } // End of Unity - Awake

            private void OnDisable()
            {
                Dispose();
            } // End of Unity - OnDisable

            #endregion Unity Methods
            
            #region Public Methods

            // Play Methods
            public void PlayAudio(AudioType type, bool fade = false, float delay = 0.0f)
            {
                AddJob(new AudioJob(AudioAction.START, type, fade, delay));
            }

            public void StopAudio(AudioType type, bool fade = false, float delay = 0.0f)
            {
                AddJob(new AudioJob(AudioAction.STOP, type, fade, delay));
            }
            
            public void PauseAudio(AudioType type, bool fade = false, float delay = 0.0f)
            {
                AddJob(new AudioJob(AudioAction.PAUSE, type, fade, delay));
            }
            
            public void RestartAudio(AudioType type, bool fade = false, float delay = 0.0f)
            {
                AddJob(new AudioJob(AudioAction.RESTART, type, fade, delay));
            }

            public void GlobalVolumeControlMaster(float value)
            {
                GameSetting.Instance.VolumeMaster = value;
                SetMixerVolume(_volumeNameMaster, value);
            } // End of GlobalVolumeControlMaster
            
            public void GlobalVolumeControlBGM(float value)
            {
                GameSetting.Instance.VolumeBGM = value;
                SetMixerVolume(_volumeNameBGM, value);
            } // End of GlobalVolumeControlBGM
            
            public void GlobalVolumeControlEffect(float value)
            {
                GameSetting.Instance.VolumeEffect = value;
                SetMixerVolume(_volumeNameEffect, value);
            } // End of GlobalVolumeControlEffect
            
            #endregion Public Methods

            #region Private Methods

            private void Configure()
            {
                Instance = this;
                _audioTable = new Hashtable();
                _jobTable = new Hashtable();

                InitializeVolumeSetting();
                GenerateAudioTable();
            } // End of Configure

            private void Dispose()
            {
                if(_jobTable == null) return;
                
                foreach (DictionaryEntry entry in _jobTable)
                {
                    var job = (IEnumerator) entry.Value;
                    StopCoroutine(job);
                }
            } // End of Dispose

            private void GenerateAudioTable()
            {
                foreach (var track in Tracks)
                {
                    foreach (var audioObj in track.Audio)
                    {
                        // Do not duplicate keys
                        if (_audioTable.ContainsKey(audioObj.Type))
                        {
                            LogWarning("You trying to register audio ["+audioObj.Type+"] that has already been registered" );
                        }
                        else
                        {
                            _audioTable.Add(audioObj.Type , track);
                            Log("Registering audio [" + audioObj.Type + "]");
                        }
                    }
                }
            } // End of GenerateAudioTable

            private void InitializeVolumeSetting()
            {
                // Set Mixer
                _globalMixer = Tracks[0].Source.outputAudioMixerGroup.audioMixer;

                GlobalVolumeControlMaster(GameSetting.Instance.VolumeMaster);
                GlobalVolumeControlBGM(GameSetting.Instance.VolumeBGM);
                GlobalVolumeControlEffect(GameSetting.Instance.VolumeEffect);
            } // End of InitializeVolumeSetting

            private void SetMixerVolume(string volumeKey,float value)
            {
                var volume = GetMappingVolume(value);
                
                _globalMixer.SetFloat(volumeKey, volume);
                Log(volumeKey + " : " + volume);
            } // End of SetMixerMasterVolume

            private float GetMappingVolume(float value) => Mathf.Lerp(-80.0f, 5.0f, value);

            private void AddJob(AudioJob job)
            {
                // Remove conflicting job
                RemoveConflictingJobs(job.Type);
                
                // Start job - Coroutine
                var jobRunner = RunAudioJob(job);
                _jobTable.Add(job.Type,jobRunner);
                StartCoroutine(jobRunner);
                
                Log("Starting job on ["+ job.Type + "] with operation " + job.Action);
            } // End of AddJob

            private IEnumerator RunAudioJob(AudioJob job)
            {
                Log("Job count : " + _jobTable.Count);

                yield return new WaitForSeconds(job.Delay);
                
                var track = (AudioTrack) _audioTable[job.Type];
                var source = track.Source;
                
                source.clip = GetAudioClipFromAudioTrack(job.Type, track);

                switch (job.Action)
                {
                    case AudioAction.START:
                        source.Play();
                        break;
                    case AudioAction.STOP:
                        if (!job.Fade)
                        {
                            source.Stop();
                        }
                        break;
                    case AudioAction.PAUSE:
                        if (!job.Fade)
                        {
                            source.Pause();
                            yield break;
                        }
                        break;
                    case AudioAction.RESTART:
                        source.Stop();
                        source.Play();
                        break;
                }

                if (job.Fade)
                {
                    var initialValue = job.Action == AudioAction.START || job.Action == AudioAction.RESTART ?  0.0f : 1.0f;
                    var targetValue = initialValue == 0.0f ? 1.0f : 0.0f;
                    var duration = 1.0f;
                    var timer = 0.0f;
                    
                    
                    while (timer < duration)
                    {
                        source.volume = Mathf.Lerp(initialValue, targetValue, timer / duration);
                        timer += Time.deltaTime;
                        yield return null;
                    }

                    if (job.Action == AudioAction.STOP)
                    {
                        source.Stop();
                    }

                    if (job.Action == AudioAction.PAUSE)
                    {
                        source.Pause();
                        yield break;
                    }
                }

                while (source.isPlaying)
                {
                    yield return null;
                }

                _jobTable.Remove(job.Type);
                Log("Job count : " + _jobTable.Count);
                yield return null;
            } // End of RunAudioJob

            private AudioClip GetAudioClipFromAudioTrack(AudioType type,AudioTrack track)
            {
                foreach (var audioObj in track.Audio)
                {
                    if (audioObj.Type == type)
                    {
                        return audioObj.Clip;
                    }
                }

                return null;
            } // End of GetAudioClipFromAudioTrack            
            
            private void RemoveConflictingJobs(AudioType type)
            {
                if (_jobTable.ContainsKey(type))
                {
                    RemoveJob(type);
                }

                var conflictAudio = AudioType.None;
                foreach (DictionaryEntry entry in _jobTable)
                {
                    var audioType = (AudioType) entry.Key;
                    var audioTrackInUse = (AudioTrack) _audioTable[audioType];
                    var audioTrackNeeded = (AudioTrack) _audioTable[type];

                    if (audioTrackNeeded.Source == audioTrackInUse.Source)
                    {
                        // Conflict
                        conflictAudio = audioType;
                    }
                }

                if (conflictAudio != AudioType.None)
                {
                    RemoveJob(conflictAudio);
                }
            } // End of RemoveConflictingJobs

            private void RemoveJob(AudioType type)
            {
                if (!_jobTable.ContainsKey(type))
                {
                    LogWarning("You trying to stop a job ["+type + "] that is not running");
                    return;
                }

                var runningJob = (IEnumerator) _jobTable[type];
                StopCoroutine(runningJob);
                _jobTable.Remove(type);
            } // End of RemoveJob
            
            private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;
            private void Log(string msg)
            {
                if(CheckDebugMode) return;
                
                Debug.Log("[Audio Controller]: " + msg);
            }

            private void LogWarning(string msg)
            {
                if(CheckDebugMode) return;
                
                Debug.Log("[Audio Controller]: " + msg);
            }
            
            #endregion Private Methods
        }
    }
}


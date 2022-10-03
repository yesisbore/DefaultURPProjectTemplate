using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : MonoBehaviour
{
    #region Variables

    // Public 
    public bool UseLog;
    public TMP_Text Log;
    
    public bool UseRemote = false;
    public bool ForceClearCache = false;
    public bool LoadOnStart = false;
    
    public int MaxResourceCount = 2;
    
    // Private
    private enum ModeType
    {
        Wait,
        ClearCache,
        DownloadAsset,
        LoadAsset,
        End
    }

    private ModeType _updateMode = ModeType.Wait;

    private List<GameObject> _pools = new List<GameObject>(100);
    private int _doneCount = 0;
    private int _downloadCount = 0;

    private string[] _stringKeys = new string[]
    {
        "Fade UI",
        "Assets/ProjectFile/02Prefab/RemoteGroup/TestCube.prefab"
    };
    
    private int[] _stringHashes = new int[]
    {
        "Fade UI".GetHashCode(),
        "TestCube".GetHashCode()
    };
    
    #endregion Variables

    #region Unity Methods

    private void Start()
    {
        Initialize();
    } // End of Unity - Start

    private void Update()
    {
        UpdateState();
    } // End of Unity - Update

    #endregion Unity Methods

    #region Help Methods

    private void Initialize()
    {
        if(!UseLog) Log.gameObject.SetActive(false);
        
        var async = Addressables.InitializeAsync();
        async.Completed += (op) =>
        {
            _updateMode = ModeType.ClearCache;
            Addressables.Release(async);
        };
    } // End of Initialize

    private void UpdateState()
    {
        switch (_updateMode)
        {
            case ModeType.Wait:
                SetLog("Wait");
                break;
            case ModeType.ClearCache:
                SetLog("ClearCache");
                ClearCache();
                break;
            case ModeType.DownloadAsset: 
                SetLog("DownloadAsset");
                DownloadAsset();
                break;
            case ModeType.LoadAsset:
                SetLog("LoadAsset");
                LoadAsset();
                break;
            case ModeType.End:
                SetLog("End");
                break;
        }
    } // End of UpdateState

    private void ClearCache()
    {
        if (ForceClearCache)
        {
            // Forced deletion of downloaded assets
            Caching.ClearCache();
        }

        if (LoadOnStart)
        {
            if (UseRemote)
            {
                _updateMode = ModeType.DownloadAsset;
            }
            else
            {
                _updateMode = ModeType.LoadAsset;
            }
        }
        else
        {
            _updateMode = ModeType.End;
        }
    } // End of ClearCache

    private void DownloadAsset()
    {
        for (int i = 0; i < MaxResourceCount; i++)
        {
            string key = _stringKeys[i];
            // Get size to download
            Addressables.GetDownloadSizeAsync(key).Completed += (opSize) =>
            {
                if (opSize.Status == AsyncOperationStatus.Succeeded && opSize.Result > 0)
                {
                    Addressables.DownloadDependenciesAsync(key, true).Completed += (opDownload) =>
                    {
                        if (opDownload.Status != AsyncOperationStatus.Succeeded)
                            return;

                        OnDownloadDone();
                    };
                }
                else
                {
                    // Result value 0 means already been downloaded
                    OnDownloadDone();
                }
            };
        }

        _updateMode = ModeType.Wait;
    } // End of DownloadAsset

    private void LoadAsset()
    {
        for (int i = 0; i < MaxResourceCount; i++)
        {
            Addressables.LoadAssetAsync<GameObject>(_stringKeys[i]).Completed += (op) =>
            {
                if (op.Status != AsyncOperationStatus.Succeeded) return;

                OnLoadDone(op);
            };
            _updateMode = ModeType.Wait;
        }
    } // End of LoadAsset


    private void OnDownloadDone()
    {
        ++_downloadCount;
        if (_doneCount == MaxResourceCount)
        {
            _updateMode = ModeType.LoadAsset;
        }
    } // End of OnDownloadDone
    
    private void OnLoadDone(AsyncOperationHandle<GameObject> go)
    {
        ++_doneCount;
        if (_doneCount == MaxResourceCount)
        {
            _updateMode = ModeType.End;
        }
        
    } // End of OnLoadDone

    private void SetLog(string log)
    {
        if (!UseLog) return;
        
        Log.text = log;
    } // End of Log
    
    #endregion Help Methods

    #region Spawn Methods

    private void Spawn(int index)
    {
        if(index < 0 || index >= _stringKeys.Length) return;

        Addressables.InstantiateAsync(_stringKeys[index]).Completed += (op) =>
        {
            _pools.Add(op.Result);
        };
    } // End of Spawn 

    public void Clear()
    {
        foreach (var go in _pools)
        {
            Destroy(go);
        }
        _pools.Clear();
        AssetBundle.UnloadAllAssetBundles(true);

        for (int i = 0; i < MaxResourceCount; i++)
        {
            Addressables.ClearDependencyCacheAsync(_stringKeys[i]);
        }

        _downloadCount = 0;
        _doneCount = 0;
        
        SetLog("Cleared");
    } // End of Clear

    public void ReLoad()
    {
        Clear();

        if (UseRemote)
        {
            _updateMode = ModeType.DownloadAsset;
        }
        else
        {
            _updateMode = ModeType.LoadAsset;
        }
    } // End of ReLoad
    
    public void Spawn1()
    {
        Spawn(0);
    }
    
    public void Spawn2()
    {
        Spawn(1);
    }
    
    #endregion Spawn Methods
}
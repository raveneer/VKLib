using System;
using ShopWars.Native;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using VKLib;
using VKLib.Native;
using Zenject;

public class AdManager : MonoBehaviour
{
    [Inject] private EventManager _eventManager;
    [Inject] private GameSetting _gameSetting;
    private Action _adFailedAction;
    private Action _adFinishAction;
    private Action _adSkippedActon;

    private void Start()
    {
        Initialize();
        _eventManager.SceneLoadAfterAdShow+= OnSceneLoadAfterAdShow;
    }

    private void OnSceneLoadAfterAdShow()
    {
        if (IsAdCondition())
        {
            ShowAd(ReloadScene, ReloadScene, ReloadScene);
        }
        else
        {
            Debug.LogWarning("Ad condition not met, cancel Ad show");
            ReloadScene();
        }
    }

    //todo : 책임소재가 틀렸음. 다른데로 보낼 것.
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool IsAdCondition()
    {
        return PlayerPrefs.GetFloat(PlayerPrefString.TotalPlayTimeSec) >= _gameSetting.NeededGamePlaySecForAdShow
               || PlayerPrefs.GetInt(PlayerPrefString.TotalPlayGameCount) >= _gameSetting.NeededGamePlayCountForAdShow;
    }

    private void Initialize()
    {
    #if UNITY_ANDROID
        Advertisement.Initialize(android_game_id);
    #elif UNITY_IOS
        Advertisement.Initialize(ios_game_id);
    #endif
    }

    private void ShowAd(Action adFinished, Action adSkipped, Action adFailed)
    {
        _adFinishAction = adFinished;
        _adSkippedActon = adSkipped;
        _adFailedAction = adFailed;

        if (Advertisement.IsReady(rewarded_video_id))
        {
            var options = new ShowOptions {resultCallback = HandleShowResult};

            Advertisement.Show(rewarded_video_id, options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
            {
                Debug.Log("The ad was successfully shown.");

                // to do ...
                // 광고 시청이 완료되었을 때 처리
                _adFinishAction?.Invoke();

                break;
            }
            case ShowResult.Skipped:
            {
                Debug.Log("The ad was skipped before reaching the end.");

                // to do ...
                // 광고가 스킵되었을 때 처리
                _adSkippedActon?.Invoke();
                break;
            }
            case ShowResult.Failed:
            {
                Debug.LogError("The ad failed to be shown.");

                // to do ...
                // 광고 시청에 실패했을 때 처리
                _adFailedAction?.Invoke();
                break;
            }
        }
    }

    private const string android_game_id = "3839745";
    //private const string ios_game_id = "3839744";
    private const string rewarded_video_id = "video";
}
using UnityEngine;

namespace VKLib.Network
{
    /// <summary>
    /// 유니티 광고를 처리하는 클래스. 애드몹도 같이 처리하면 좋겠으나
    /// </summary>
    public class UnityAdManager : MonoBehaviour
    {
        /*
        [Inject] private EventManager _eventManager;
        [Inject] private IADPolicy _iadPolicy;
        private Action _adFailedAction;
        private Action _adFinishAction;
        private Action _adSkippedActon;

        private void Start()
        {
            Initialize();
            _eventManager.SceneLoadAfterAdShow += OnSceneLoadAfterAdShow;
            _eventManager.ShowAd += ShowAd;
        }

        private void Initialize()
        {
        #if UNITY_ANDROID || UNITY_EDITOR
            Advertisement.Initialize(_iadPolicy.AndroidGameID);
            Debug.Log("googleAdPolicy init");
#elif UNITY_IOS
            Advertisement.Initialize(_iadPolicy.AppleGameID);
            Debug.Log("appleAdPolicy init");
#endif
        }

        private void OnSceneLoadAfterAdShow()
        {
            if (_iadPolicy.IsConditionMet())
            {
                ShowAd(ReloadScene, ReloadScene, ReloadScene);
            }
            else
            {
                Debug.LogWarning("Ad condition not met, cancel Ad show");
                ReloadScene();
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

        //todo : 책임소재가 틀렸음. 다른데로 보낼 것.
        private void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void ShowAd(Action adFinished, Action adSkipped, Action adFailed)
        {
            _adFinishAction = adFinished;
            _adSkippedActon = adSkipped;
            _adFailedAction = adFailed;

            if (Advertisement.IsReady(_iadPolicy.RewardVideoID))
            {
                var options = new ShowOptions {resultCallback = HandleShowResult};

                Advertisement.Show(_iadPolicy.RewardVideoID, options);
            }
            else
            {
                TDebug.LogWarning("준비된 광고가 없습니다!");
            }
        }
        */


    }

    public interface IADPolicy
    {
        bool IsConditionMet();
        string RewardVideoID { get; }
        string AndroidGameID { get; }
        string AppleGameID { get; }
    }
}
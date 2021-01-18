using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VKLib.Native;
using Zenject;

namespace VKLib.VKLib.UI
{
    /// <summary>
    /// 씬을 로딩한다. 만약 슬라이더가 있을 시, 로딩량에 따라 슬라이더의 길이를 변화시킨다.
    /// 텍스트가 있을 시, 텍스트로 출력해준다.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [Inject] private EventManager _eventManager;
        public string TargetScene;
        public Image FillImage;
        public TextMeshProUGUI TMP_loadInfo;
        public GameObject Panel;
        /// <summary>
        /// 로딩 완료시 자동으로 씬 전환할지 여부
        /// </summary>
        public bool IsAutoStartScene = true;
        /// <summary>
        /// 로딩 자체를 자동실행할 지 여부. 
        /// </summary>
        public bool IsAutoStartLoading = false;
        private AsyncOperation _asyncOperation;

        void Awake()
        {
            _eventManager.LoadScene += OnLoadScene;

            Panel.gameObject.SetActive(false);

            if (IsAutoStartLoading)
            {
                StartLoadManually();
            }
        }

        private void OnLoadScene(string obj)
        {
            TargetScene = obj;
            StartLoadManually();
        }

        //유니티에서 호출하도록.
        public void StartLoadManually()
        {
            StartCoroutine(StartLoad(TargetScene, IsAutoStartScene));
        }

        private IEnumerator StartLoad(string strSceneName, bool isAutoStartScene)
        {
            TDebug.Log($"SceneLoader start load : {strSceneName}");
            Panel.gameObject.SetActive(true);
            Debug.Assert(strSceneName != string.Empty);
            _asyncOperation = SceneManager.LoadSceneAsync(strSceneName);
            _asyncOperation.allowSceneActivation = isAutoStartScene;
            while (_asyncOperation.progress < 1f)
            {
                if (FillImage != null)
                    FillImage.fillAmount = _asyncOperation.progress;
                if (TMP_loadInfo != null)
                    TMP_loadInfo.text = "Loading... " + ((int)(_asyncOperation.progress * 100)).ToString() + "%";

                yield return new WaitForSeconds(.1f);
            }

            yield break;
        }
    }
}
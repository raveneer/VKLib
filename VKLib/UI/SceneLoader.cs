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
    ///     ���� �ε��Ѵ�. ���� �����̴��� ���� ��, �ε����� ���� �����̴��� ���̸� ��ȭ��Ų��.
    ///     �ؽ�Ʈ�� ���� ��, �ؽ�Ʈ�� ������ش�.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [Inject] private EventManager _eventManager;
        public Image FillImage;
        /// <summary>
        ///     �ε� ��ü�� �ڵ������� �� ����.
        /// </summary>
        public bool IsAutoStartLoading = false;
        /// <summary>
        ///     �ε� �Ϸ�� �ڵ����� �� ��ȯ���� ����
        /// </summary>
        public bool IsAutoStartScene = true;
        public GameObject Panel;
        public string TargetScene;
        public TextMeshProUGUI TMP_loadInfo;
        private AsyncOperation _asyncOperation;

        private void Awake()
        {
            _eventManager.LoadScene += OnLoadScene;

            if (Panel != null)
            {
                Panel.gameObject.SetActive(false);
            }

            if (IsAutoStartLoading)
            {
                StartLoadManually();
            }
        }

        //����Ƽ���� ȣ���ϵ���.
        public void StartLoadManually()
        {
            StartCoroutine(StartLoad(TargetScene, IsAutoStartScene));
        }

        private IEnumerator StartLoad(string strSceneName, bool isAutoStartScene)
        {
            TDebug.Log($"SceneLoader start load : {strSceneName}");
            if (Panel != null)
            {
                Panel.gameObject.SetActive(true);
            }

            Debug.Assert(strSceneName != string.Empty);
            _asyncOperation = SceneManager.LoadSceneAsync(strSceneName);
            _asyncOperation.allowSceneActivation = isAutoStartScene;
            while (_asyncOperation.progress < 1f)
            {
                if (FillImage != null)
                    FillImage.fillAmount = _asyncOperation.progress;
                if (TMP_loadInfo != null)
                    TMP_loadInfo.text = "Loading... " + ((int) (_asyncOperation.progress * 100)).ToString() + "%";

                yield return new WaitForSeconds(.1f);
            }

            yield break;
        }

        private void OnLoadScene(string obj)
        {
            TargetScene = obj;
            StartLoadManually();
        }
    }
}
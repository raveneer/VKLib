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
    /// ���� �ε��Ѵ�. ���� �����̴��� ���� ��, �ε����� ���� �����̴��� ���̸� ��ȭ��Ų��.
    /// �ؽ�Ʈ�� ���� ��, �ؽ�Ʈ�� ������ش�.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [Inject] private EventManager _eventManager;
        public string TargetScene;
        public Image FillImage;
        public TextMeshProUGUI TMP_loadInfo;
        public GameObject Panel;
        /// <summary>
        /// �ε� �Ϸ�� �ڵ����� �� ��ȯ���� ����
        /// </summary>
        public bool IsAutoStartScene = true;
        /// <summary>
        /// �ε� ��ü�� �ڵ������� �� ����. 
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

        //����Ƽ���� ȣ���ϵ���.
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
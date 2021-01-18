using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VKLib.VKLib.UI
{
    /// <summary>
    /// ���� �ε��Ѵ�. ���� �����̴��� ���� ��, �ε����� ���� �����̴��� ���̸� ��ȭ��Ų��.
    /// �ؽ�Ʈ�� ���� ��, �ؽ�Ʈ�� ������ش�.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
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

        void Start()
        {
            Panel.gameObject.SetActive(false);

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
            Panel.gameObject.SetActive(true);
            Debug.Assert(strSceneName != string.Empty);
            _asyncOperation = SceneManager.LoadSceneAsync(strSceneName);
            _asyncOperation.allowSceneActivation = isAutoStartScene;
            while (_asyncOperation.progress < 0.9f)
            {
                if (FillImage != null)
                    FillImage.fillAmount = _asyncOperation.progress;
                if (TMP_loadInfo != null)
                    TMP_loadInfo.text = "Loading... " + ((int)_asyncOperation.progress * 100).ToString() + "%";

                yield return true;
            }
        }
    }
}
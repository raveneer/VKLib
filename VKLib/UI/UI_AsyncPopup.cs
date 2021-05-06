using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VKLib.Native;
using Zenject;

namespace VKLib.UI
{
    public class UI_AsyncPopup : MonoBehaviour
    {
        [Inject] private EventManager _eventManager;
        public Image Image_Loading;
        public Transform Panel;

        public TextMeshProUGUI TMP_Message;

        private void Awake()
        {
            _eventManager.AsyncPanelClose += OnAsyncPanelClose;
            _eventManager.AsyncPanelOpen += OnAsyncPanelOpen;
            Panel.gameObject.SetActive(false);
        }
        
        private void OnAsyncPanelOpen(string message)
        {
            TMP_Message.text = message;
            Panel.gameObject.SetActive(true);
            StartCoroutine(RollingLoadingImageRoutine());
        }

        private void OnAsyncPanelClose()
        {
            Panel.gameObject.SetActive(false);
            StopAllCoroutines();
        }

        private IEnumerator RollingLoadingImageRoutine()
        {
            while (true)
            {
                Image_Loading.transform.Rotate(Vector3.forward, -12);
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}
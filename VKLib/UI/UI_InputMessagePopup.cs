using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VKLib.Native;
using Zenject;

namespace VKLib.UI
{
    /// <summary>
    /// 유저에게 입력을 받는다. 취소는 불가능함. (단순하게, 단순하게 만듭시다)
    /// </summary>
    public class UI_InputMessagePopup :MonoBehaviour
    {
        [Inject] private readonly EventManager _eventManager;
        public InputField InputField;
        public TextMeshProUGUI TMP_Message;
        public Button Button_Confirm;
        public Button Button_Close;
        public GameObject Panel;
        public RectTransform InputFieldRect;

        private void Awake()
        {
            _eventManager.InputMessagePopup += OnInputMessagePopup;
            Button_Close.onClick.AddListener(()=>Panel.gameObject.SetActive(false));
            Panel.gameObject.SetActive(false);
        }

        private void OnInputMessagePopup(string message, Action<string> action, bool isLargeInputField)
        {
            Panel.gameObject.SetActive(true);
            TMP_Message.text = message;
            InputField.text = ""; //초기화.
            Button_Confirm.onClick.RemoveAllListeners();
            Button_Confirm.onClick.AddListener(()=>
            {
                Panel.gameObject.SetActive(false);
                //액션의 순서에 주의. 이렇게 꺼준 후 해줘야 inputpopup이 또  inputpopup 부를때 연결이 됨.
                action(InputField.text);
            });

            if (isLargeInputField)
            {
                InputFieldRect.sizeDelta = new Vector2(InputFieldRect.sizeDelta.x, 150);
            }
            else
            {
                InputFieldRect.sizeDelta = new Vector2(InputFieldRect.sizeDelta.x, 50);
            }
        }
    }
}
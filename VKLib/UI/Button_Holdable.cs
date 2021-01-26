using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VKLib.VKLib.UI
{
    public class Button_Holdable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action FireEvent;
        public Button Button;
        public UnityEvent FireUnityEvent;
        public Image BG;
        private float contiueRemainSec;
        public TextMeshProUGUI ButtonText;
        
        /// <summary>
        ///     홀드 상태가 되면, 이 시간마다 이벤트를 쏜다.
        /// </summary>
        private readonly float contiueTermSec = 0.05f;

        [SerializeField]
        private Image fillImage;
        private bool pointerDown;
        private float pointerDownTimer;

        [SerializeField]
        private float requiredHoldTime = 1f;

        private void Start()
        {
            Reset();
            FireEvent += () => FireUnityEvent?.Invoke();
        }

        /// <summary>
        ///     일정시간 이상 누르고 있으면 일정 텀으로 연사함.
        /// </summary>
        private void Update()
        {
            if (pointerDown)
            {
                fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
                pointerDownTimer += Time.deltaTime;
                if (pointerDownTimer >= requiredHoldTime)
                {
                    contiueRemainSec += Time.deltaTime;
                    if (contiueRemainSec >= contiueTermSec)
                    {
                        contiueRemainSec -= contiueTermSec;
                        FireEvent?.Invoke();
                        //TDebug.Log("LongLongClicks fire!");
                    }
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;
            //Debug.Log("OnPointerDown");
        }

        /// <summary>
        ///     뗄떼도 한번은 발동 해 주어야 함
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            FireEvent?.Invoke();

            Reset();
            //Debug.Log("OnPointerUp");
        }

        private void Reset()
        {
            pointerDown = false;
            pointerDownTimer = 0;
            fillImage.fillAmount = 0;
        }

        public void SetPushable(bool isPushAble)
        {
            if (isPushAble)
            {
                BG.color = Color.white;
            }
            else
            {
                BG.color = Color.gray;
            }
        }

    }
}